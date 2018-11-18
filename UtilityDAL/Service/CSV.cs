using CsvHelper;
using GenericParsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Linq;
using System.Text;

namespace UtilityDAL
{
    public class CSV<T> : IFileDbService<T>
    {
        //static TextReader text;

        readonly string dbName;

        static readonly string providerName = "CSV";


        //static CSV(string path = null)
        //{
        //    if (path == null)
        //        dbName = Helper.GetConnectionString(providerName, false);
        //    else
        //        dbName = path;


        //}
        public CSV(string path=null)
        {
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
            return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
        }


        public IList<T> FromDb(string name)
        {
            try
            {
                var csv = CSVReaderFactory.Build(dbName, name + ".csv");
                return csv.GetRecords<T>().ToList();
            }
            catch(Exception e)
            {
                Console.WriteLine("file " + name + "/n error: " +e.Message);
                return null;
            }
        }

        public bool ToDb(IList<T> lst, string name)
        {
            using (TextWriter writer = new StreamWriter(Path.Combine(dbName, name + ".csv")))
            {
                var csv = new CsvWriter(writer);
                //csv.Configuration.Encoding = Encoding.UTF8;
                csv.WriteRecords(lst); // where values implements IEnumerable
            }
            return true;
        }



        public IObservable<T> FromDbAsync(string name)
        {
            return Observable.FromAsync(_ =>
          new   CSV().FromDbAsync<T>(dbName, name + ".csv")).Switch();
        }



    }


}
