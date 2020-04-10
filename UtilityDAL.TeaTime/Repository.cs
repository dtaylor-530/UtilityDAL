using Optional;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UtilityDAL.Common;
using UtilityDAL.Contract.Generic;
using UtilityDAL.Contract.NonGeneric;

namespace UtilityDAL.Teatime
{
    public class Repository<T> : IFileDatabase<T> where T : struct //,IChildRow
    {
        private static readonly string providerName = "TeaTime";

        private readonly string dbName;

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
            return TeatimeHelper.FromDb<T>(id, dbName).ValueOr(() => null);
        }

        public bool Clear(string name)
        {
            return TeatimeHelper.Clear(name, dbName);
        }
    }

    public class TeatimeFileService<T> : IFileDbService where T : struct
    {
        private static readonly string providerName = "TeaTime";

        private readonly string dbName;

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
            return TeatimeHelper.FromDb<T>(dbName, name).ValueOr(() => null);
        }

        public bool To(ICollection lst, string name)
        {
            IList<T> op = lst.Cast<T>().ToList();
            TeatimeHelper.ToDb(op, name, dbName);
            return true;
        }

        public bool Clear(string name)
        {
            return TeatimeHelper.Clear(name, dbName);
        }
    }
}