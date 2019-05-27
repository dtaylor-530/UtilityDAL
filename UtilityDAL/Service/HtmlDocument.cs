using CsvHelper;
using GenericParsing;
using HtmlAgilityPack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace UtilityDAL
{
    public class HtmlDocument<T> //: IDbService<T>
    {

        readonly string dbName;

        static readonly string providerName = "HtmlDocument";
        private Func<HtmlDocument, T> _parse;


        //}
        public HtmlDocument(Func<HtmlDocument, T> parse,string path = null)
        {
            _parse = parse;

            if (path == null)
                dbName = DbEx.GetConnectionString(providerName, false);
            else if (path == String.Empty || path == "")
                dbName = System.IO.Directory.GetCurrentDirectory();
            else
                if (System.IO.Directory.Exists(path))
                dbName = path;
            else
                throw new System.IO.DirectoryNotFoundException(path + " does not exist");
        }

        public List<String> SelectIds()
        {
            throw new Exception();
        }


        public T FromDb()
        {
            return HtmlDocumentEx.Deserialise(dbName, _parse);
        }

        public bool ToDb(HtmlDocument blob, string name)
        {

            blob.Save(System.IO.Path.Combine(dbName,name+ ".html"));
            return true;
        }
    
    }
    
}
