using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Webkit;
using Android.Widget;
using Android.OS;
using Mehrsan.Android.WV.Views;
using Mehrsan.Android.WV.Models;
using System.IO;
using Android.Content.Res;
using SQLite;
using System.Collections;
using System.Collections.Generic;

namespace Mehrsan.Android.WV
{
    [Activity(Label = "Memorizer", MainLauncher = true)]
    public class MainActivity : Activity
    {
        public static SQLite.SQLiteConnection database;




        public void LoadData(SQLite.SQLiteConnection connection)
        {
            LoadWords(connection);
            LoadHistories(connection);

        }

        private void LoadHistories(SQLiteConnection connection)
        {
            AssetManager assets = this.Assets;
            using (StreamReader source = new StreamReader(assets.Open("history.csv")))
            {
                var content = source.ReadToEnd();
                string[] lineStrs = content.Split('\n');
                List<string> lines = new List<string>();
                lines.AddRange(lineStrs);
                for (int i = 1; i < lines.Count; i++)
                {
                    string[] parts = lines[i].Split(',');
                    parts = RemoveNulls(parts);

                    History h = new History();
                    if (parts.Length != 8)
                        continue;
                    h.HistoryId = int.Parse(parts[0]);
                    h.ReviewTime = DateTime.Parse(parts[1]);
                    h.ReviewPeriod = int.Parse(parts[2]);
                    h.Result = parts[3] == "1";
                    h.WordId = long.Parse(parts[4]);
                    h.ReviewTimeSpan = long.Parse(parts[5]);
                    h.UpdatedWord = parts[6];
                    h.UpdatedMeaning = parts[7];
                    DAL.AddHistory(h, false);

                }
            }
        }

        private void LoadWords(SQLiteConnection connection)
        {
            AssetManager assets = this.Assets;

            using (StreamReader source = new StreamReader(assets.Open("word.csv")))
            {
                var content = source.ReadToEnd();
                string[] lineStrs = content.Split('\n');
                List<string> lines = new List<string>();
                lines.AddRange(lineStrs);
                List<string> errors = new List<string>();
                for (int i = 1; i < lines.Count; i++)
                {
                    string[] parts = lines[i].Split(',');
                    parts = RemoveNulls(parts);
                    if (parts.Length != 16)
                    {
                        errors.Add(parts[0]);
                        continue;
                    }

                    Word w = new Word();

                    w.WordId = int.Parse(parts[0]);
                    w.TargetWord = parts[1];
                    w.Meaning = parts[2];
                    w.NextReviewDate = DateTime.Parse(parts[5]);

                    if (!string.IsNullOrEmpty(parts[8]))
                        w.IsAmbiguous = parts[8] == "1";

                    if (!string.IsNullOrEmpty(parts[9]))
                        w.NofSpace = short.Parse(parts[9]);

                    if (!string.IsNullOrEmpty(parts[10]))
                        w.WrittenByMe = parts[10] == "1";

                    if (!string.IsNullOrEmpty(parts[11]))
                        w.IsMovieSubtitle = parts[11] == "1";

                    if (!string.IsNullOrEmpty(parts[12]))
                        w.StartTime = TimeSpan.Parse(parts[12]);

                    if (!string.IsNullOrEmpty(parts[13]))
                        w.EndTime = TimeSpan.Parse(parts[13]);

                    w.MovieName = parts[14];
                    w.UserId = parts[15];

                    connection.Insert(w);
                }
            }
        }

        private string[] RemoveNulls(string[] parts)
        {
            for (int j = 0; j < parts.Length; j++)
            {
                string part = parts[j];
                if (part.ToLower() == "null")
                    parts[j] = string.Empty;
            }
            return parts;
        }

        public static void SetConnection()
        {
            //var path = "/users/jesseliberty/Data/ToDoAndroid.db";
            var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            path = System.IO.Path.Combine(path, "Word.db3");
            var conn = new SQLite.SQLiteConnection(path);
            //conn.CreateTable<Test>();

            //conn.CreateTable<Test>(CreateFlags.AllImplicit);

            try
            {

                conn.CreateTable<History>(CreateFlags.AllImplicit);

            }
            catch (Exception)
            {

            }
            try
            {

                conn.CreateTable<Word>();
            }
            catch (Exception)
            {

            }

            database = conn;
        }
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            object[] objArr = new object[30000];
            SetConnection();

            TableQuery<Word> words = database.Table<Word>();
            TableQuery<History> histories = database.Table<History>();
            if (words.Count() < 100 || histories.Count() < 100)
            {
                LoadData(database);
            }
            //database.Execute("select * from Word", objArr);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var webView = FindViewById<WebView>(Resource.Id.webView);
            webView.Settings.JavaScriptEnabled = true;

            // Use subclassed WebViewClient to intercept hybrid native calls
            webView.SetWebViewClient(new HybridWebViewClient());

            // Render the view from the type generated from RazorView.cshtml
            Word model = DAL.GetWordForReview();

            InsertNewWords();
            var template = new RazorView() { Model = model };
            var page = template.GenerateString();

            // Load the rendered HTML into the view with a base URL 
            // that points to the root of the bundled Assets folder
            webView.LoadDataWithBaseURL("file:///android_asset/", page, "text/html", "UTF-8", null);

        }

        private void InsertNewWords()
        {
            var histories = DAL.GetMobileHistories();
            int index = 0;
            var str = string.Empty;
            foreach (var history in histories)
            {
                index++;

                str += history.UpdatedWord + "\n";
                str += history.ReviewTime + "\n";

            }
        }
        
        private class HybridWebViewClient : WebViewClient
        {
            public override bool ShouldOverrideUrlLoading(WebView webView, string url)
            {

                // If the URL is not our own custom scheme, just let the webView load the URL as usual
                var scheme = "hybrid:";

                if (!url.StartsWith(scheme))
                    return false;

                // This handler will treat everything between the protocol and "?"
                // as the method name.  The querystring has all of the parameters.
                var resources = url.Substring(scheme.Length).Split('?');
                var method = resources[0];
                var parameters = System.Web.HttpUtility.ParseQueryString(resources[1]);
                var splitter = "1****2";

                var wordId = int.Parse(parameters["txtId"]);
                if (method == "btnOK" || method == "btnOK")
                {
                    //var textbox = parameters["textbox"];
                    var result = method == "btnOK";
                    DAL.UpdateWordStatus(result, wordId, 10000);

                    var word = DAL.GetWordForReview();
                    int nofTodayHistories = DAL.GetNofTodayHistories();
                    var prepended = "ShowWord" + splitter + word.WordId.ToString() + splitter + word.TargetWord + splitter + word.Meaning + splitter + nofTodayHistories.ToString();
                    var js = string.Format("SetLabelText('{0}');", prepended);
                    webView.LoadUrl("javascript:" + js);


                }
                else if (method == "btnSetAmbiguous")
                {
                    DAL.UpdateWord(wordId, string.Empty, string.Empty, 0, null, null, true);
                }

                return true;
            }

        }
    }

}

