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

namespace UtilityDAL.Dynamic
{




    public class Teatime<T> : IFileDbService<T> where T : struct //,IChildRow
    {
        static readonly string providerName = "TeaTime";

        readonly string dbName;

        public Teatime(string path = null)
        {
            if (path == null)
                dbName = DbEx.GetConnectionString(providerName, false);
            else if(path== String.Empty|| path=="")
                dbName = System.IO.Directory.GetCurrentDirectory();
            else
                dbName = path;

        }

        public List<String> SelectIds()
        {
            return System.IO.Directory.GetFiles(dbName).Select(_=>System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
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

}
  
