using System;
using System.Collections.Generic;
using System.Text;
using UtilityInterface;
using UtilityInterface.Generic;

namespace UtilityDAL
{
    public struct UrlHtml
    {
        public string Url { get; set; }
        public string Html { get; set; }
    }

    public class TeatimeSaveLoadService<T> : IPermanent<IList<T>> where T : struct //,IChildRow
    {
        static readonly string providerName = "TeaTime";

        readonly string dbName;
        //readonly string _key;
        public TeatimeSaveLoadService(/*string key,*/ string path = null)
        {
            //_key = key;
            if (path == null)
                dbName = DbEx.GetConnectionString(providerName, false);
            else if (path == String.Empty || path == "")
                dbName = System.IO.Directory.GetCurrentDirectory();
            else
                dbName = path;

        }

        public bool Save(IList<T>  o)
        {
            var xx = (IList<T>)o;
            Teatime.ToDb(xx, typeof(T).Name, dbName);
            return true;
        }

        public IList<T> Load()
        {
            return Teatime.FromDb<T>(typeof(T).Name,dbName);

        }
    }


}
