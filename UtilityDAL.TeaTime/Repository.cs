using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using UtilityDAL.Contract.Generic;
using UtilityDAL.Contract.NonGeneric;
using UtilityDAL.Common;

namespace UtilityDAL.TeaTime
{

    public class Repository<T> : IFileDatabase<T> where T : struct //,IChildRow
    {
        static readonly string providerName = "TeaTime";

        readonly string dbName;

        public Repository(string path = null)
        {
            if (path == null)
                dbName = DbEx.GetConnectionString(providerName, false);
            else if (path == String.Empty || path == "")
                dbName = System.IO.Directory.GetCurrentDirectory();
            else
                dbName = path;

        }

        public List<String> SelectIds()
        {
            return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
        }


        public bool To(IList<T> prices, string id) //, IComparable
        {

            TeatimeHelper.ToDb(prices, id, dbName);
            return true;
        }



        public IList<T> From(string id)   //, IComparable
        {

            return TeatimeHelper.FromDb<T>(id, dbName);

        }

        public bool Clear(string name)
        {
            throw new NotImplementedException();
        }
    }



    public class TeatimeFileService<T> : IFileDbService where T : struct
    {
        static readonly string providerName = "TeaTime";

        readonly string dbName;

        public TeatimeFileService(string path = null)
        {
            if (path == null)
                throw new Exception("path equals null");
            else if (path == String.Empty || path == "")
                dbName = System.IO.Directory.GetCurrentDirectory();
            else
                dbName = path;

        }

        public List<String> SelectIds()
        {
            return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
        }


        public ICollection From(string name)
        {

            var x = (TeatimeHelper.FromDb<T>(dbName));
            if (x.ContainsKey(name))
                return x[name];
            else
                return null;
        }

        public bool To(ICollection lst, string name)
        {
            IList<T> op = lst.Cast<T>().ToList();
            UtilityDAL.TeaTime.TeatimeHelper.ToDb(op, name, dbName);
            return true;
        }

        public bool Clear(string name)
        {
            return UtilityDAL.TeaTime.TeatimeHelper.Clear(name, dbName);
        }
    }


}

