using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL
{
    public static class HtmlDocumentEx
    {

        public static T Deserialise<T>(string path, Func<HtmlDocument, T> parse)
        {
            return parse.Deserialise(System.IO.File.ReadAllText(path));
        }


        public static T Deserialise<T>(this Func<HtmlDocument, T> parse,string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            return parse(doc);
        }
    }
}
