using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTime;
using System.Reactive.Linq;
using UtilityInterface;
using System.Collections;
using UtilityDAL.Contract;
using UtilityDAL.Contract.Generic;
using UtilityDAL.Contract.NonGeneric;

namespace UtilityDAL
{




    public class Teatime<T> : IFileDbService<T> where T : struct //,IChildRow
    {
        static readonly string providerName = "TeaTime";

        readonly string dbName;

        public Teatime(string path = null)
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


        public bool ToDb(IList<T> prices, string id) //, IComparable
        {

            Teatime.ToDb(prices, id, dbName);
            return true;
        }



        public IList<T> FromDb(string id)   //, IComparable
        {

            return Teatime.FromDb<T>(id, dbName);

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


        public ICollection FromDb(string name)
        {
            
            var x= (Teatime.FromDb<T>(dbName));
            if (x.ContainsKey(name))
                return x[name];
            else
                return null;
        }

        public bool ToDb(ICollection lst, string name)
        {
            IList<T> op = lst.Cast<T>().ToList();
            UtilityDAL.Teatime.ToDb(op, name, dbName);
            return true;
        }
    }


}

