using System;
using System.Collections.Generic;
using UtilityDAL.Common;
using UtilityDAL.Contract.Generic;

namespace UtilityDAL
{
    public abstract class FileDbService<T> : IFileDatabase<T>
    {
        public FileDbService(string providerName = null, string path = null, bool ignoremissing = true)
        {
            if (!System.IO.Directory.Exists(path) & ignoremissing)
                dbName = path;
            else if (System.IO.Directory.Exists(path))
                dbName = path;
            else if (path == null)
                dbName = DbEx.GetConnectionString(providerName, false);
            else if (path == String.Empty || path == "")
                dbName = System.IO.Directory.GetCurrentDirectory();
            else
                throw new System.IO.DirectoryNotFoundException(path + " does not exist");
        }

        protected readonly string dbName;

        public abstract IList<T> From(string name);

        public abstract bool To(IList<T> lst, string name);

        public abstract List<string> SelectIds();

        public bool Clear(string name)
        {
            throw new NotImplementedException();
        }

        //static CSV(string path = null)
        //{
        //    if (path == null)
        //        dbName = Helper.GetConnectionString(providerName, false);
        //    else
        //        dbName = path;

        //}
    }
}