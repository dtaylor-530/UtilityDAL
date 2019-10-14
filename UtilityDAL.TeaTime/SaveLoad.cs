using System;
using System.Collections.Generic;
using UtilityDAL.Common;
using UtilityInterface.Generic;

namespace UtilityDAL.Teatime

{
    //public struct UrlHtml
    //{
    //    public string Url { get; set; }
    //    public string Html { get; set; }
    //}

    public class SaveLoad<T> : IPermanent<IList<T>> where T : struct //,IChildRow
    {
        private static readonly string providerName = "TeaTime";

        private readonly string dbName;

        //readonly string _key;
        public SaveLoad(/*string key,*/ string path = null)
        {
            //_key = key;
            if (path == null)
                dbName = DbEx.GetConnectionString(providerName, false);
            else if (path == String.Empty || path == "")
                dbName = System.IO.Directory.GetCurrentDirectory();
            else
                dbName = path;
        }

        public bool Save(IList<T> o)
        {
            var xx = (IList<T>)o;
            TeatimeHelper.ToDb(xx, typeof(T).Name, dbName);
            return true;
        }

        public IList<T> Load()
        {
            return TeatimeHelper.FromDb<T>(typeof(T).Name, dbName);
        }
    }
}