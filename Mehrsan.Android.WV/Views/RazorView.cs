#pragma warning disable 1591
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Mehrsan.Android.WV.Views
{
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.CodeDom.Compiler.GeneratedCodeAttribute("RazorTemplatePreprocessor", "4.0.3.214")]
public partial class RazorView : RazorViewBase
{

#line hidden

#line 1 "RazorView.cshtml"
public Mehrsan.Android.WV.Models.Word Model { get; set; }

#line default
#line hidden


public override void Execute()
{
WriteLiteral("<html>\r\n<head>\r\n    <link");

WriteLiteral(" rel=\'stylesheet\'");

WriteLiteral(" href=\'jquery.mobile-1.4.5.min.css\'");

WriteLiteral(">\r\n\r\n    <link");

WriteLiteral(" rel=\"stylesheet\"");

WriteLiteral(" href=\"style.css\"");

WriteLiteral(" />\r\n    <script");

WriteLiteral(" type=\"text/javascript\"");

WriteLiteral(">\r\n\r\n        ////////////////////////////////////////////////////////////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"/////////////\r\n\r\n        function playText(wordText) {\r\n\r\n            var i = 0;" +
"\r\n            var url = \'/Words/\';\r\n            for (i = 0; i < wordText.length " +
"; i++) {\r\n                url += wordText[i] + \'/\';\r\n            }\r\n\r\n          " +
"  url += wordText + \'.mp3\';\r\n\r\n            if (fileExists(url)) {\r\n             " +
"   var audio = new Audio(url);\r\n                audio.play();\r\n            }\r\n  " +
"      }\r\n\r\n        /////////////////////////////////////////////////////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"////////////////////\r\n\r\n        function fileExists(image_url) {\r\n\r\n            " +
"var http = new XMLHttpRequest();\r\n\r\n            return false;\r\n            http." +
"open(\'HEAD\', image_url, false);\r\n            http.send();\r\n\r\n            return " +
"http.status != 404;\r\n\r\n        }\r\n        //////////////////////////////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"///////////////////////////////////////////\r\n\r\n        // This javascript method" +
" calls C# by setting the browser\r\n        // to a URL with a custom scheme that " +
"is registered in C#.\r\n        // All values are sent to C# as part of the querys" +
"tring\r\n        function InvokeCSharpWithFormValues(elm) {\r\n            var qs = " +
"\"\";\r\n            var elms = elm.form.elements;\r\n\r\n            for (var i = 0; i " +
"< elms.length; i++) {\r\n                qs += \"&\" + elms[i].name + \"=\" + elms[i]." +
"value;\r\n            }\r\n\r\n            if (elms.length > 0)\r\n                qs = " +
"qs.substring(1);\r\n\r\n            location.href = \"hybrid:\" + elm.name + \"?\" + qs;" +
"\r\n        }\r\n\r\n        /////////////////////////////////////////////////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"////////////////////////\r\n\r\n        // This javascript method is called from C#\r" +
"\n        function SetLabelText(text) {\r\n            var splitter = \'1****2\';\r\n\r\n" +
"            var arr = text.split(splitter);\r\n\r\n            if (arr[0] == \"ShowWo" +
"rd\") {\r\n                var lblNOFTodayHistories = document.getElementById(\'lblN" +
"OFTodayHistories\');\r\n                lblNOFTodayHistories.value = arr[4]\r\n\r\n    " +
"            ShowWord(arr[1], arr[2], arr[3]);\r\n            }\r\n            else i" +
"f (arr[0] == \"SetLabelText\") {\r\n                var elm = document.getElementByI" +
"d(\'label\');\r\n                elm.innerHTML = arr[1];\r\n            }\r\n        }\r\n" +
"\r\n        //////////////////////////////////////////////////////////////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"///////////\r\n\r\n        function ShowWord(id, word, meaning) {\r\n\r\n            var" +
" txtId = document.getElementById(\'txtId\');\r\n            var txtWord = document.g" +
"etElementById(\'txtWord\');\r\n            word = getAlignedText(word);\r\n           " +
" meaning = getAlignedText(meaning);\r\n            var txtMeaning = document.getEl" +
"ementById(\'txtMeaning\');\r\n\r\n            txtId.value = id;\r\n            txtWord.v" +
"alue = word;\r\n            txtMeaning.value = meaning;\r\n\r\n            playText(wo" +
"rd);\r\n            showImagesForWord(word);\r\n        }\r\n\r\n        ///////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"//////////////////////////////////////////////////////////////////\r\n\r\n        fu" +
"nction getAlignedText(text) {\r\n            var j = 0;\r\n            var newText =" +
" \'\';\r\n            for (var i = 0; i < text.length ; i+=25) {\r\n\r\n                " +
"newText = newText +\'\\n\' + text.substring(i, i+25) ;\r\n\r\n            }\r\n          " +
"  return newText;\r\n        }\r\n        //////////////////////////////////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"///////////////////////////////////////\r\n\r\n        function getWordDirectory(wor" +
"d) {\r\n            var i = 0;\r\n            var result = \"/words/\"\r\n            fo" +
"r (i = 0; i < word.length ; i = i + 1) {\r\n                result = result + word" +
".substring(i, i + 1) + \"/\";\r\n            }\r\n            return result;\r\n        " +
"}\r\n\r\n        ///////////////////////////////////////////////////////////////////" +
"////////////////////////////////////////////////////////////////////////////////" +
"//////////////\r\n\r\n        function showImagesForWord(currentWord) {\r\n\r\n         " +
"   var targetDirectory = getWordDirectory(currentWord.TargetWord.trim());\r\n\r\n\r\n " +
"           var theSrc = targetDirectory + \"1.jpg\";\r\n\r\n            if (!fileExist" +
"s(theSrc)) {\r\n\r\n                theSrc = \'/images/noImage.jpg\';\r\n\r\n            }" +
"\r\n\r\n            document[\"imgMain\"].src = theSrc;\r\n\r\n\r\n\r\n        }\r\n\r\n    </scri" +
"pt>\r\n</head>\r\n<body");

WriteLiteral(" style=\"background-color:coral;\"");

WriteLiteral(">\r\n    <div");

WriteLiteral(" data-role=\"page\"");

WriteLiteral(" data-theme=\"b\"");

WriteLiteral(">\r\n        <form>\r\n\r\n            <div");

WriteLiteral(" data-role=\"content\"");

WriteLiteral(" class=\"content\"");

WriteLiteral(">\r\n                <input");

WriteLiteral(" id=\"lblNOFTodayHistories\"");

WriteLiteral(" type=\"text\"");

WriteLiteral(" name=\"lblNOFTodayHistories\"");

WriteLiteral(" value=\"00000\"");

WriteLiteral(" style=\"position:absolute;left:5px;top:5px;\"");

WriteLiteral(">\r\n\r\n                <textarea");

WriteLiteral(" rows=\"3\"");

WriteLiteral(" cols=\"25\"");

WriteLiteral(" wrap=\"hard\"");

WriteLiteral(" style=\"visibility:hidden;\"");

WriteLiteral(" id=\"txtId\"");

WriteLiteral(" name=\"txtId\"");

WriteAttribute ("value", " value=\"", "\""

#line 150 "RazorView.cshtml"
                                                                            , Tuple.Create<string,object,bool> ("", Model.WordId

#line default
#line hidden
, false)
);
WriteLiteral(">");


#line 150 "RazorView.cshtml"
                                                                                                                             Write(Model.WordId);


#line default
#line hidden
WriteLiteral("</textarea>\r\n                <div");

WriteLiteral(" data-role=\"content\"");

WriteLiteral(" class=\"content\"");

WriteLiteral(">\r\n                    <textarea");

WriteLiteral(" rows=\"6\"");

WriteLiteral(" cols=\"30\"");

WriteLiteral(" wrap=\"hard\"");

WriteLiteral(" id=\"txtWord\"");

WriteLiteral(" name=\"txtWord\"");

WriteAttribute ("value", " value=", ""

#line 152 "RazorView.cshtml"
                                                        , Tuple.Create<string,object,bool> ("", Model.TargetWord

#line default
#line hidden
, false)
);
WriteLiteral(" placeholder=\"txtWord\"");

WriteLiteral(" onclick=\"InvokeCSharpWithFormValues(this)\"");

WriteLiteral(" class=\"ui-btn ui-btn-inline ui-corner-all\"");

WriteLiteral(">");


#line 152 "RazorView.cshtml"
                                                                                                                                                                                                                        Write(Model.TargetWord);


#line default
#line hidden
WriteLiteral("</textarea>\r\n                </div>\r\n                <div");

WriteLiteral(" data-role=\"content\"");

WriteLiteral(" class=\"content\"");

WriteLiteral(">\r\n                    <textarea");

WriteLiteral(" rows=\"6\"");

WriteLiteral(" cols=\"30\"");

WriteLiteral(" wrap=\"hard\"");

WriteLiteral(" id=\"txtMeaning\"");

WriteLiteral(" name=\"txtMeaning\"");

WriteAttribute ("value", " value=\"", "\""

#line 155 "RazorView.cshtml"
                                                               , Tuple.Create<string,object,bool> ("", Model.Meaning

#line default
#line hidden
, false)
);
WriteLiteral(" placeholder=\"txtMeaning\"");

WriteLiteral(" onclick=\"InvokeCSharpWithFormValues(this)\"");

WriteLiteral(" class=\"ui-btn ui-btn-inline ui-corner-all\"");

WriteLiteral(">");


#line 155 "RazorView.cshtml"
                                                                                                                                                                                                                                Write(Model.Meaning);


#line default
#line hidden
WriteLiteral("</textarea>\r\n                </div>\r\n            </div>\r\n            <div");

WriteLiteral(" data-role=\"content\"");

WriteLiteral(" class=\"content\"");

WriteLiteral(">\r\n\r\n                <span><input");

WriteLiteral(" type=\"button\"");

WriteLiteral(" name=\"btnOK\"");

WriteLiteral(" value=\"OK\"");

WriteLiteral(" onclick=\"InvokeCSharpWithFormValues(this)\"");

WriteLiteral(" class=\"ui-btn ui-btn-inline ui-corner-all\"");

WriteLiteral("></span>\r\n                <span><input");

WriteLiteral(" type=\"button\"");

WriteLiteral(" name=\"btnNOK\"");

WriteLiteral(" value=\"NOK\"");

WriteLiteral(" onclick=\"InvokeCSharpWithFormValues(this)\"");

WriteLiteral(" class=\"ui-btn ui-btn-inline ui-corner-all\"");

WriteLiteral("></span>\r\n                <span><input");

WriteLiteral(" type=\"button\"");

WriteLiteral(" name=\"btnSetAmbiguous\"");

WriteLiteral(" value=\"Ambiguous\"");

WriteLiteral(" onclick=\"InvokeCSharpWithFormValues(this)\"");

WriteLiteral(" class=\"ui-btn ui-btn-inline ui-corner-all\"");

WriteLiteral("></span>\r\n            </div>\r\n\r\n\r\n        </form>\r\n    </div>\r\n</body>\r\n</html>\r\n" +
"\r\n\r\n");

}
}

// NOTE: this is the default generated helper class. You may choose to extract it to a separate file 
// in order to customize it or share it between multiple templates, and specify the template's base 
// class via the @inherits directive.
public abstract class RazorViewBase
{

		// This field is OPTIONAL, but used by the default implementation of Generate, Write, WriteAttribute and WriteLiteral
		//
		System.IO.TextWriter __razor_writer;

		// This method is OPTIONAL
		//
		/// <summary>Executes the template and returns the output as a string.</summary>
		/// <returns>The template output.</returns>
		public string GenerateString ()
		{
			using (var sw = new System.IO.StringWriter ()) {
				Generate (sw);
				return sw.ToString ();
			}
		}

		// This method is OPTIONAL, you may choose to implement Write and WriteLiteral without use of __razor_writer
		// and provide another means of invoking Execute.
		//
		/// <summary>Executes the template, writing to the provided text writer.</summary>
		/// <param name="writer">The TextWriter to which to write the template output.</param>
		public void Generate (System.IO.TextWriter writer)
		{
			__razor_writer = writer;
			Execute ();
			__razor_writer = null;
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the template output without HTML escaping it.</summary>
		/// <param name="value">The literal value.</param>
		protected void WriteLiteral (string value)
		{
			__razor_writer.Write (value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes a literal value to the TextWriter without HTML escaping it.</summary>
		/// <param name="writer">The TextWriter to which to write the literal.</param>
		/// <param name="value">The literal value.</param>
		protected static void WriteLiteralTo (System.IO.TextWriter writer, string value)
		{
			writer.Write (value);
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>Writes a value to the template output, HTML escaping it if necessary.</summary>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected void Write (object value)
		{
			WriteTo (__razor_writer, value);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>Writes an object value to the TextWriter, HTML escaping it if necessary.</summary>
		/// <param name="writer">The TextWriter to which to write the value.</param>
		/// <param name="value">The value.</param>
		/// <remarks>The value may be a Action<System.IO.TextWriter>, as returned by Razor helpers.</remarks>
		protected static void WriteTo (System.IO.TextWriter writer, object value)
		{
			if (value == null)
				return;

			var write = value as Action<System.IO.TextWriter>;
			if (write != null) {
				write (writer);
				return;
			}

			//NOTE: a more sophisticated implementation would write safe and pre-escaped values directly to the
			//instead of double-escaping. See System.Web.IHtmlString in ASP.NET 4.0 for an example of this.
			writer.Write(System.Net.WebUtility.HtmlEncode (value.ToString ()));
		}

		// This method is REQUIRED, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to the template output.
		/// </summary>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		protected void WriteAttribute (string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			WriteAttributeTo (__razor_writer, name, prefix, suffix, values);
		}

		// This method is REQUIRED if the template contains any Razor helpers, but you may choose to implement it differently
		//
		/// <summary>
		/// Conditionally writes an attribute to a TextWriter.
		/// </summary>
		/// <param name="writer">The TextWriter to which to write the attribute.</param>
		/// <param name="name">The name of the attribute.</param>
		/// <param name="prefix">The prefix of the attribute.</param>
		/// <param name="suffix">The suffix of the attribute.</param>
		/// <param name="values">Attribute values, each specifying a prefix, value and whether it's a literal.</param>
		///<remarks>Used by Razor helpers to write attributes.</remarks>
		protected static void WriteAttributeTo (System.IO.TextWriter writer, string name, string prefix, string suffix, params Tuple<string,object,bool>[] values)
		{
			// this is based on System.Web.WebPages.WebPageExecutingBase
			// Copyright (c) Microsoft Open Technologies, Inc.
			// Licensed under the Apache License, Version 2.0
			if (values.Length == 0) {
				// Explicitly empty attribute, so write the prefix and suffix
				writer.Write (prefix);
				writer.Write (suffix);
				return;
			}

			bool first = true;
			bool wroteSomething = false;

			for (int i = 0; i < values.Length; i++) {
				Tuple<string,object,bool> attrVal = values [i];
				string attPrefix = attrVal.Item1;
				object value = attrVal.Item2;
				bool isLiteral = attrVal.Item3;

				if (value == null) {
					// Nothing to write
					continue;
				}

				// The special cases here are that the value we're writing might already be a string, or that the
				// value might be a bool. If the value is the bool 'true' we want to write the attribute name instead
				// of the string 'true'. If the value is the bool 'false' we don't want to write anything.
				//
				// Otherwise the value is another object (perhaps an IHtmlString), and we'll ask it to format itself.
				string stringValue;
				bool? boolValue = value as bool?;
				if (boolValue == true) {
					stringValue = name;
				} else if (boolValue == false) {
					continue;
				} else {
					stringValue = value as string;
				}

				if (first) {
					writer.Write (prefix);
					first = false;
				} else {
					writer.Write (attPrefix);
				}

				if (isLiteral) {
					writer.Write (stringValue ?? value);
				} else {
					WriteTo (writer, stringValue ?? value);
				}
				wroteSomething = true;
			}
			if (wroteSomething) {
				writer.Write (suffix);
			}
		}
		// This method is REQUIRED. The generated Razor subclass will override it with the generated code.
		//
		///<summary>Executes the template, writing output to the Write and WriteLiteral methods.</summary>.
		///<remarks>Not intended to be called directly. Call the Generate method instead.</remarks>
		public abstract void Execute ();

}
}
#pragma warning restore 1591
