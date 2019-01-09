using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Mehrsan.Business;

namespace Mehrsan.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string targetDir = "d:\\temp\\temp\\";
        static string targetSearchDir = "d:\\temp\\temp\\result\\";

        public MainWindow()
        {


            InitializeComponent();
            //Threading();
            //AnalyseTrackMan();

            SaveUrl();
        }


        List<string> Items { get; set; } = new List<string>();
        int threadWaitTime = 200;
        int waitingNo = 0;
        private void ProduceItem()
        {
            lock (Items)
            {
                index++;
                var item = index + " : " + DateTime.Now.ToString("HH:mm:ss");
                Items.Add(item);
                Thread.Sleep(threadWaitTime);
                Log($"Item {item} produced");

            }

        }

        private void ProduceItem1(object obj)
        {
            try
            {
                int insertIndex = (int)obj;
                var item = index + " : " + DateTime.Now.ToString("HH:mm:ss");
                Items[insertIndex] = item;
                Thread.Sleep(threadWaitTime);
                Log($"Item {insertIndex} produced");

            }
            finally
            {
                lock (fileLock)
                {
                    totalThreadsExecuted++;
                }
            }
        }

        Queue<string> fileLock = new Queue<string>();
        public void Log(string message)
        {
            lock (fileLock)
            {
                fileLock.Enqueue(message);
            }
        }
        bool exitLoop = false;
        int lastProcessedlogIndex = 0;

        public void ProcessLog()
        {
            while (!exitLoop)
            {
                int count = fileLock.Count;
                if (count > lastProcessedlogIndex)
                {
                    var message = string.Empty;

                    lock (fileLock)
                    {
                        var list = fileLock.ToList();
                        string[] newItems = new string[count - lastProcessedlogIndex];
                        list.CopyTo(lastProcessedlogIndex, newItems, 0, newItems.Length);
                        lastProcessedlogIndex = count;
                        message = string.Join("\n", newItems);

                        File.AppendAllText(@"D:\2.txt", message + "\n");
                        Console.WriteLine(message);
                        lastProcessedlogIndex++;
                    }
                    Thread.Sleep(10);
                }
            }

        }

        private void ConsumeItem2(object obj)
        {
            int indexToBeConsumed = (int)obj;
            try
            {


                Log($"Start Consuming {indexToBeConsumed}" + "   " + DateTime.Now.ToString("HH:mm:ss"));
                if (indexToBeConsumed >= 250)
                {
                    Log($"Item {indexToBeConsumed}" + " was empty  " + DateTime.Now.ToString("HH:mm:ss"));

                    return;
                }

                var item = Items[indexToBeConsumed];
                Log($"finished consumption Item index {indexToBeConsumed} containing {item} consumed   {DateTime.Now.ToString("HH:mm:ss")}");

            }
            finally
            {
                Thread.Sleep(threadWaitTime);
                lock (fileLock)
                {
                    totalThreadsExecuted++;
                }
            }

        }
        private void ConsumeItem1(object obj)
        {
            try
            {

                int lastConsumedItem = (int)obj;

                Log($"Start Consuming {lastConsumedItem}" + "   " + DateTime.Now.ToString("HH:mm:ss"));
                if (lastConsumedItem >= 250)
                {
                    Log($"Item {lastConsumedItem}" + " was empty  " + DateTime.Now.ToString("HH:mm:ss"));

                    return;
                }

                int waitIndex = 0;
                while (string.IsNullOrEmpty(Items[lastConsumedItem]))
                {
                    if (waitIndex % 100 == 0)
                    {
                        Log($"Item {lastConsumedItem} is waiting waitindex {waitIndex} ** " + DateTime.Now.ToString("HH:mm:ss"));
                    }
                    waitIndex++;
                    ReExecuteIndexes.Add(lastConsumedItem);
                    return;

                }

                var item = Items[lastConsumedItem];
                Log($"finished consumption Item index {lastConsumedItem} containing {item} consumed   {DateTime.Now.ToString("HH:mm:ss")}");

            }
            finally
            {
                Thread.Sleep(threadWaitTime);
                lock (fileLock)
                {
                    totalThreadsExecuted++;
                }
            }




        }

        List<int> ReExecuteIndexes = new List<int>();

        static Semaphore semaphore = new Semaphore(5, 5);

        private void ConsumeItem()
        {

            bool lockTaken = false;

            try
            {
                Monitor.TryEnter(Items, ref lockTaken);

                if (!lockTaken)
                {
                    DateTime t1 = DateTime.Now;
                    try
                    {
                        waitingNo++;
                        Log($"Lock failed. waitno :{waitingNo} went to wait " + t1.ToString("HH:mm:ss"));

                        Monitor.Enter(Items, ref lockTaken);
                    }
                    finally
                    {
                        var t2 = DateTime.Now;
                        Log($"Lock taken for waitno:{waitingNo} after {(t2 - t1).TotalSeconds} seconds  " + t2.ToString("HH:mm:ss"));

                        waitingNo--;
                    }
                }

                //do work on a collection
                if (lockTaken)
                {
                    if (Items.Count > 0)
                    {
                        var item = Items[0];
                        Log(" the following item Consumed " + item + "   " + DateTime.Now.ToString("HH:mm:ss"));
                        Items.RemoveAt(0);
                    }
                    else
                    {
                        Log("There is no item o consumed " + "   " + DateTime.Now.ToString("HH:mm:ss"));
                    }
                }
                else
                {
                    Log("Went to the log queue" + "   " + DateTime.Now.ToString("HH:mm:ss"));
                }
            }
            finally
            {

                if (lockTaken)
                {
                    Monitor.Exit(Items);
                }

                Thread.Sleep(threadWaitTime);

            }

        }

        private readonly ReaderWriterLockSlim myLock = new ReaderWriterLockSlim();

        private void Threading()
        {

            //ReaderWriterLockSlimMethod();
            //MutexMethod();
            //Semaphore();
            //BackgroundForegroundThreads(true);
            //this.Close();
            //return;
            //BackgroundForegroundThreads(false);
            //this.Close();
            //return;
            Thread th1 = new Thread(ProcessLog);
            th1.Start();

            DateTime t1 = DateTime.Now;

            MonitorCollectionTest();
            //BlockingCollectionTest();
            //EventThreading(t1);//it took total miliseconds :44388,0568
            //ReadSharedMemory();
            //Task t = Task.Run(action:ConsumeYileld);
            //t.Wait();
            //ParallelAndSharedMemory(t1);//it took total miliseconds :525,5951
            //ParallelAndLock(t1);//it took total miliseconds :2200,1148

            //SharedMemoryLessLocksThread(t1); //it took total miliseconds :10435,7176

            while (!exitLoop || true)
            {
                exitLoop = lastProcessedlogIndex >= fileLock.Count;
            }


        }

        private BlockingCollection<string> BlockingItems = new BlockingCollection<string>();
        private void BlockingCollectionTest()
        {
            var readerTask = new Task(ProduceItem4);
            readerTask.Start();

            //Initiate processor task
            var processor = new Task(ConsumeItem4);
            processor.Start();

            //Wait until reading events is complete and mark as finished

            readerTask.Wait();
            BlockingItems.CompleteAdding();

            Task.WaitAll(new Task[] { processor, readerTask });
        }

        private void MonitorCollectionTest()
        {
            var readerTask = new Task(ProduceItem5);
            readerTask.Start();

            //Initiate processor task
            var processor = new Task(ConsumeItem5);
            processor.Start();


            Task.WaitAll(new Task[] { processor, readerTask });
        }
        private void ConsumeItem5()
        {
            while (true)
            {
                Thread.Sleep(threadWaitTime);

                Monitor.Enter(Items);
                while (Items.Count > 0)
                {
                    var item = Items[0];
                    Items.RemoveAt(0);
                    Log($"{item} is consumed" + " : " + DateTime.Now.ToString("HH:mm:ss"));
                    Thread.Sleep(threadWaitTime);
                }
                Monitor.Exit(Items);
            }
        }

        private void ProduceItem5()
        {
            for (int i = 0; i < 500000; i++)
            {
                Thread.Sleep(threadWaitTime );
                Monitor.Enter(Items);
                var item = "item" + i;
                Log($"{item} is produced" + " : " + DateTime.Now.ToString("HH:mm:ss"));
                Items.Add(item);
                Monitor.Exit(Items);
            }
        }

        private void ConsumeItem4()
        {
            Thread.Sleep(2000);
            foreach (var item in BlockingItems.GetConsumingEnumerable())
            {
                Log($"{item} is consumed" + " : " + DateTime.Now.ToString("HH:mm:ss"));
                Thread.Sleep(threadWaitTime);
            }
        }

        private void ProduceItem4()
        {
            for (int i = 0; i < 5000; i++)
            {
                Thread.Sleep(threadWaitTime / 2);
                var item = "item" + i;
                Log($"{item} is produced" + " : " + DateTime.Now.ToString("HH:mm:ss"));
                BlockingItems.Add(item);
            }
        }

        int count = 0;
        private void ReaderWriterLockSlimMethod()
        {
            for (int i = 0; i < 100; i++)
            {
                Parallel.For(0, 20,
                    new Action<int>(
                        (int k) =>
                        {
                            if (k == 1)
                            {
                                PerformTransaction();
                            }
                            else
                            {
                                Console.WriteLine($"LastTransaction {LastTransaction}");
                            }

                        }
                        ));

            }
        }
        public int LastTransaction
        {
            get
            {
                myLock.EnterReadLock();
                int temp = count;

                myLock.ExitReadLock();
                return temp;
            }
        }
        public void Dispose() { myLock.Dispose(); }
        public void PerformTransaction()
        {
            myLock.EnterWriteLock();
            count++;
            Console.WriteLine($"count {count }");

            myLock.ExitWriteLock();
        }

        private void MutexMethod()
        {
            if (!IsSingleInstance())
            {
                Console.WriteLine("More than one instance"); // Exit program.
            }
            else
            {
                Console.WriteLine("One instance"); // Continue with program.
            }
            // Stay open.
            Console.ReadLine();
        }

        static bool IsSingleInstance()
        {
            try
            {
                // Try to open existing mutex.
                Mutex.OpenExisting("PERL");
            }
            catch
            {
                // If exception occurred, there is no such mutex.
                MainWindow._m = new Mutex(true, "PERL");

                // Only one instance.
                return true;
            }
            // More than one instance.
            return false;
        }

        private void Semaphore()
        {
            Task.Factory.StartNew(() =>
            {
                for (int i = 1; i <= 15; ++i)
                {
                    PrintSomething(i);
                    if (i % 5 == 0)
                    {
                        Thread.Sleep(2000);
                    }
                }
            });
            Console.ReadLine();
        }

        private void PrintSomething(int number)
        {
            semaphore.WaitOne();
            Console.WriteLine(number);
            semaphore.Release();
        }

        ObservableCollection<int> IsReadyCollection = new ObservableCollection<int>();
        private void EventThreading(DateTime t1)
        {
            for (int i1 = 0; i1 < 1000; i1++)
            {
                IsReadyCollection.Add(0);
                Items.Add(string.Empty);
            }

            IsReadyCollection.CollectionChanged += IsReadyCollectionChangedHandler;
            for (int i = 0; i < 250; i++)
            {
                DistributeLoad3(i);
            }

            for (int i = 501; i < 1000; i++)
            {
                Thread t = new Thread(new ParameterizedThreadStart(ConsumeItem2));
                t.Start(i);
            }

            while (_threads.Count < 250 || totalThreadsExecuted < 1000)
            {
                Thread.Sleep(5);
            }

            _threads.ForEach(t => t.Join());

            Log($" it took total miliseconds :{(DateTime.Now - t1).TotalMilliseconds}");//it took total miliseconds :15588,7479

        }

        private void IsReadyCollectionChangedHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            Thread t = new Thread(() =>
            {
                ConsumeItem2(e.NewStartingIndex);
            });
            t.Start();
        }



        private void BackgroundForegroundThreads(bool isBackground)
        {
            for (int i = 0; i < 3; i++)
            {
                Thread t = new Thread(() =>
                {
                    Console.WriteLine($" Data is {i} started");
                    Thread.Sleep(20000);
                    Console.WriteLine($" Data is {i} finished");

                })
                { IsBackground = isBackground };

                t.Start();
            }
        }

        private void ReadSharedMemory()
        {
            using (MemoryMappedFile mm = MemoryMappedFile.OpenExisting("sharedMemoryFile"))
            {
                using (MemoryMappedViewAccessor mmv = mm.CreateViewAccessor())
                {
                    int i = 10;
                    while (true)
                    {
                        Parallel.For(0, 100, new Action<int>((m) =>
                        {
                            var k = mmv.ReadChar(m);
                            Log($"item {k } in shared memory location {m}");
                            Thread.Sleep(500);
                        }));
                        Thread.Sleep(500);
                    }
                }
            }
        }

        private IEnumerable<string> ProduceYileld()
        {
            List<string> fullList = new List<string>();
            for (int i = 0; i < 1000; i++)
            {
                fullList.Add(" item " + i);
            }

            foreach (string item in fullList)
            {

                yield return item + " ";

            }
        }

        private void ConsumeYileld()
        {
            foreach (string item in ProduceYileld())
            {
                Log(item + DateTime.Now.ToString("HH:mm:ss"));
                Thread.Sleep(500);
            }
        }

        private void ParallelAndSharedMemory(DateTime t1)
        {
            for (int i1 = 0; i1 < 1000; i1++)
            {
                Items.Add("");
            }

            Parallel.For(0, 1000, (i) =>
            {

                DistributeLoad2(i);
            });

            while (ReExecuteIndexes.Count > 0 || totalThreadsExecuted < 1000)
            {

                if (ReExecuteIndexes.Count == 0)
                {
                    Thread.Sleep(1);
                    continue;
                }
                totalThreadsExecuted -= ReExecuteIndexes.Count;
                int[] tempArray = new int[ReExecuteIndexes.Count];
                ReExecuteIndexes.CopyTo(tempArray);
                ReExecuteIndexes.Clear();
                Parallel.ForEach(tempArray, new Action<int>((i) =>
                {
                    ConsumeItem1(i);
                }));
            }
            Log($" it took total miliseconds :{(DateTime.Now - t1).TotalMilliseconds}");//it took total miliseconds :2588,0771
        }

        private void ParallelAndLock(DateTime t1)
        {
            Parallel.For(0, 1000, (i) =>
            {
                DistributeLoad(i);
            });

            Log($" it took total miliseconds :{(DateTime.Now - t1).TotalMilliseconds}");//it took total miliseconds :2588,0771
        }

        private void SharedMemoryLessLocksThread(DateTime t1)
        {
            for (int i1 = 0; i1 < 1000; i1++) //it took total miliseconds :14596,3832
            {
                Items.Add("");
            }

            //ThreadPool.SetMaxThreads(1100,100);
            for (int i = 0; i < 1000; i++)
            {
                DistributeLoad1(i);
            }

            while (_threads.Count < 1000 || totalThreadsExecuted < 1000)
            {
                Thread.Sleep(10);
            }

            _threads.ForEach(t => t.Join());

            Log($" it took total miliseconds :{(DateTime.Now - t1).TotalMilliseconds}");//it took total miliseconds :15588,7479
        }

        static int totalThreadsExecuted = 0;
        List<Thread> _threads = new List<Thread>();
        private void DistributeLoad1(object obj)
        {

            Thread t = new Thread(
                new ParameterizedThreadStart(
                    (obj1) =>
                    {
                        int i = (int)obj1;

                        try
                        {



                            Log($"Thread index  {i} started");
                            if ((i % 4) == 0)
                            {
                                ProduceItem1(i / 4);
                            }
                            else
                            {
                                int lastIndexToBeConsumed = GetLastIndex(i);
                                ConsumeItem1(lastIndexToBeConsumed);
                            }
                        }
                        catch (Exception e)
                        {

                            Log(" Exception occured " + e.Message);
                        }
                        finally
                        {
                            lock (fileLock)
                            {
                                totalThreadsExecuted++;
                            }
                        }
                    }));
            lock (_threads)
            {
                t.IsBackground = true;
                _threads.Add(t);
                t.Start(obj);
            }
        }

        private void DistributeLoad3(object obj)
        {

            Thread t = new Thread(
                new ParameterizedThreadStart(
                    (obj1) =>
                    {
                        int i = (int)obj1;

                        try
                        {



                            Log($"Thread index  {i} started");

                            ProduceItem1(i);


                        }
                        catch (Exception e)
                        {

                            Log(" Exception occured " + e.Message);
                        }
                        finally
                        {
                            lock (fileLock)
                            {
                                totalThreadsExecuted++;
                                IsReadyCollection[i] = 1;
                            }
                        }
                    }));
            lock (_threads)
            {
                _threads.Add(t);
                t.Start(obj);
            }
        }

        private static int GetLastIndex(int i)
        {
            if ((i % 4) == 0)
            {
                return -1;
            }
            else
            {

                var floor = (int)Math.Floor((double)i / 4);
                int lastIndexTobeConsumed = floor * 4 + (i % 4);
                lastIndexTobeConsumed = floor > 0 ? lastIndexTobeConsumed - floor : lastIndexTobeConsumed;
                lastIndexTobeConsumed--;
                return lastIndexTobeConsumed;
            }
        }

        private void DistributeLoad2(object obj)
        {
            int i = (int)obj;
            Log($"Thread index  {i} started");

            if ((i % 4) == 0)
            {
                ProduceItem1(i / 4);
            }
            else
            {
                int lastIndexToBeConsumed = GetLastIndex(i);
                ConsumeItem1(lastIndexToBeConsumed);
            }

            Log($"Thread index  {i} finished");

        }

        private void DistributeLoad(object obj)
        {
            int i = (int)obj;
            Log($"Thread index  {i} started");

            if ((i % 4) == 0)
            {

                ProduceItem();
            }
            else
            {
                ConsumeItem();
            }
            Log($"Thread index  {i} finished");

        }

        static int sessionCount = 0;
        int index = 0;
        private static Mutex _m;

        private async void AnalyseTrackMan()
        {
            List<string> chars = new List<string>() {
                "1", "2", "3", "4", "5", "6", "7", "8", "9", "0",
            "a","b","c","d","e","f","g","h","i","j","k","l","m","n","o","p",
            "q","r","s","t","u","v","w","x","y","z"};

            foreach (var a in chars)
            {

                foreach (var b in chars)
                {

                    foreach (var c in chars)
                    {

                        foreach (var d in chars)
                        {

                            foreach (var e in chars)
                            {
                                while (sessionCount > 10)
                                {
                                    Thread.Sleep(200);
                                }

                                TryUrl(a, b, c, d, e);

                            }
                        }
                    }
                }
            }
        }

        private async Task<int> TryUrl(string a, string b, string c, string d, string e)
        {
            var result = a + b + c + d + e;

            try
            {
                lock (this)
                {
                    sessionCount++;
                    index++;
                }
                var url = "http://tests.trackmangolf.com/testing/" + result;
                //url = "http://tests.trackmangolf.com/testing/5z5f9";
                string file = $"d:\\temp\\{result}.txt";
                await WordApis.SendCrossDomainCallForBinaryFile(url, file);
                var hitKey = "Total questions";
                var fileContent = File.Exists(file) ? File.ReadAllText(file) : string.Empty;
                string targetFile = "";
                if (fileContent.Contains(hitKey))
                {
                    targetFile = targetDir + $"{result}hit.txt";

                }
                else
                {
                    targetFile = targetDir + $"{result}.txt";

                }

                if (!File.Exists(targetFile) && !string.IsNullOrEmpty(fileContent))
                {
                    File.WriteAllText(targetFile, url + "\n" + fileContent);
                }
                string lastItemFile = $"d:\\temp\\lastItemFile.txt";
                lock (this)
                {
                    if (index % 200 == 0)
                    {
                        File.AppendAllText(lastItemFile, index + "       " + result + "         " + DateTime.Now.ToString("HH:mm:ss") + "\n");
                    }
                }
            }
            catch (Exception ex)
            {
                string targetFile = targetDir + $"{result}.error.txt";
                File.WriteAllText(targetFile, ex.Message);
            }
            finally
            {
                lock (this)
                {
                    sessionCount--;
                }
            }
            return index;
        }

        private static string RemoveHtml(string text)
        {
            var result = string.Empty;
            var openSignSeen = false;
            var closeSignSeen = false;
            for (int i = 0; i < text.Length; i++)
            {
                if (text.Substring(i, 1) == "<")
                {
                    openSignSeen = true;
                    closeSignSeen = false;
                    result += " ";
                    continue;
                }
                if (text.Substring(i, 1) == ">")
                {
                    closeSignSeen = true;
                    openSignSeen = false;
                    result += " ";
                    continue;
                }
                if (closeSignSeen || !openSignSeen)
                {
                    result += text.Substring(i, 1);
                }
            }
            return result;
        }
        private static async Task<int> SaveUrl()
        {

            var text = "";// File.ReadAllText(@"d:\2.txt");
            var lines = text.Split('\n');

            if (!Directory.Exists(targetSearchDir))
            {
                Directory.CreateDirectory(targetSearchDir);
            }
            
            int index = 0;
            foreach (string line in lines)
            {
                index++;
                string url = GetTextBetween(line, "https", ".html");
                string file = targetDir + $"{index}.txt";
                if (!string.IsNullOrEmpty(url))
                {
                    await WordApis.SendCrossDomainCallForBinaryFile(url, file);
                    File.WriteAllText(file, url + "\n" + File.ReadAllText(file));
                }
            }

            List<string> searchKeywords = new List<string>() { "15 ","15m", "20 ", "20m", "18 år", "19 år", "20 år", "21 år" };//"18 år", "19 år", "20 år", "21 år", 
            Directory.GetFiles(targetSearchDir, "*.txt").ToList().ForEach(f => File.Delete(f));
            Dictionary<string, List<string>> targetMatches = new Dictionary<string, List<string>>();
            Dictionary<string, string> telList = new Dictionary<string, string>();
            foreach (var file in Directory.GetFiles(targetDir))
            {
                var content = File.ReadAllText(file);

                foreach (var keyword in searchKeywords)
                {
                    var start = content.IndexOf(keyword);

                    if (start > 0)
                    {
                        var fileName = Path.GetFileName(file);
                        string url = content.Split('\n')[0];
                        var tel = GetTextBetween(content, "tlfnr. ", "\"");
                        tel = tel.Replace("tlfnr. ", "");
                        tel = tel.Replace("\"", "").Replace(" ", "");
                        var value = url + "\n" + fileName + "\n" + tel;
                        if (!telList.Keys.Contains(tel))
                        {
                            telList.Add(tel, url);
                        }
                        var matchPart = content.Substring(start, 100);
                        matchPart = RemoveHtml(matchPart);
                        if (!targetMatches.Keys.Contains(matchPart))
                        {
                            targetMatches.Add(matchPart, new List<string>() { url });
                        }
                        else
                        {
                            targetMatches[matchPart].Add(url);
                        }
                        var targetFile = targetSearchDir + fileName;
                        if (!File.Exists(targetFile))
                        {
                            File.Copy(file, targetFile);
                        }
                    }
                }
            }
            var result = string.Empty;
            foreach (var kvp in targetMatches)
            {
                if (false
                    || kvp.Key.Contains("18 år")
                    || kvp.Key.Contains("19 år")
                    || kvp.Key.Contains("20 år")
                    || kvp.Key.Contains("21 år")
                    //|| kvp.Key.Contains("300")
                    //|| kvp.Key.Contains("400")
                    //|| kvp.Key.Contains("500")
                    )
                {
                    result += $"\n\n\n {kvp.Key}";
                    foreach (var val in kvp.Value)
                    {
                        var q = telList.Where(kp => val == kp.Value);
                        KeyValuePair<string,string> telTmp = new KeyValuePair<string, string>();
                        if (q.Count() > 1)
                        {
                            telTmp = q.First();
                        }
                        else
                        {
                            telTmp = q.SingleOrDefault();
                        }
                        var telTmpval1 = string.IsNullOrEmpty(telTmp.Key) ? "- " : telTmp.Key;
                        result += $"\n {val}\n {telTmpval1}";

                    }
                }
            }

            var newTelList = telList.Keys.ToList();
            var csvString = string.Join("\n", newTelList.ToArray());
            File.WriteAllText(@"d:\1.csv", csvString);

            List<string> selectedTels = new List<string>()
            {
                //"71671089","50372902","91690073","91114363","91956292","91995165","91693818","91774535","53692899","50237902","91962184","52768515","91732226","71425803","91928152","50235635","71356052","50375942","50172653","71352301","71395422","91789560","71345138"
                "31834088","71336595","50371841","91956292","91748582","31171448","71479690","71425803","91732226","50375942","31835496","91408194"
            };
            List<string> selectedUrls = telList.Where(
                t => selectedTels.Contains(t.Key)).Select(kv => kv.Key + "\n" + kv.Value).ToList();
            csvString = string.Join("\n", selectedUrls.ToArray());
            File.WriteAllText(@"d:\u.csv", csvString);

            return 1;

        }

        private static string GetTextBetween(string line, string startItem, string endItem)
        {
            int start = line.IndexOf(startItem);
            if (start > 0)
            {
                int end = line.Substring(start).IndexOf(endItem) + start;
                if (start > 0 && end > 0)
                {
                    return line.Substring(start, end - start + endItem.Length);
                }
            }
            return string.Empty;
        }
    }
}
