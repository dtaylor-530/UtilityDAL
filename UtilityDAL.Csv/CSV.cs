﻿using CsvHelper;
using GenericParsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UtilityDAL.Common;
using UtilityDAL.Abstract.Generic;
using UtilityDAL.Abstract.NonGeneric;
using CsvHelper.Configuration;
using System.Globalization;

namespace UtilityDAL.CSV
{
    public class CSV<T> : IFileDatabase<T>
    {
        //static TextReader text;

        private readonly string dbName;

        private static readonly string providerName = "CSV";

        //static CSV(string path = null)
        //{
        //    if (path == null)
        //        dbName = Helper.GetConnectionString(providerName, false);
        //    else
        //        dbName = path;

        //}
        public CSV(string path = null)
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

        public List<string> SelectIds()
        {
            return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
        }

        public IList<T> From(string name)
        {
            try
            {
                var csv = CSVReaderFactory.Build(dbName, name + ".csv");
                return csv.GetRecords<T>().ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine("file " + name + "/n error: " + e.Message);
                return null;
            }
        }

        public bool To(IList<T> lst, string name)
        {
            using (TextWriter writer = new StreamWriter(Path.Combine(dbName, name + ".csv")))
            {
                var csv = new CsvWriter(writer, new CsvConfiguration(cultureInfo: CultureInfo.InvariantCulture));
                //csv.Configuration.Encoding = Encoding.UTF8;
                csv.WriteRecords(lst); // where values implements IEnumerable
            }
            return true;
        }

        public IObservable<T> FromDbAsync(string name)
        {
            throw new NotImplementedException();
            //  return Observable.FromAsync(_ =>
            //new   CSV().FromDbAsync<T>(dbName, name + ".csv")).Switch();
        }

        public bool Clear(string name)
        {
            throw new NotImplementedException();
        }
    }

    public class CSV : IFileDatabase
    {
        //static TextReader text;

        private readonly string dbName;

        private static readonly string providerName = "CSV";

        //static CSV(string path = null)
        //{
        //    if (path == null)
        //        dbName = Helper.GetConnectionString(providerName, false);
        //    else
        //        dbName = path;

        //}
        public CSV(string path = null)
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

        public ICollection From(string name)
        {
            var text = Path.Combine(dbName, name.Replace(".csv", "") + ".csv");
            if (!File.Exists(text))
            {
                return new object[] { };
            }
            else
                // Using an XML Config file.
                using (GenericParserAdapter parser = new GenericParserAdapter(text))
                {
                    parser.ColumnDelimiter = ',';
                    parser.FirstRowHasHeader = true;
                    //parser.SkipStartingDataRows = 10;
                    parser.MaxBufferSize = 4096;
                    //parser.MaxRows = 500;
                    parser.TextQualifier = '\"';

                    //  parser.Load("MyData.xml");
                    return parser.GetDataTable().DefaultView;
                }
        }

        public bool To(ICollection lst, string name)
        {
            var combinedLists = Helper.CombineCollections(From(name), lst);
            string csvstring = UtilityHelper.CsvHelper.ToCSVString(combinedLists as IEnumerable);
            File.WriteAllText(Path.Combine(dbName, name.Replace(".csv", "") + ".csv"), csvstring);
            return true;
        }

        //public IObservable<dynamic> FromDbAsync(string name)
        //{
        //    return Observable.FromAsync(_ =>
        //  new CSV().FromDbAsync(dbName, name + ".csv")).Switch();

        //}
        public async Task<IObservable<T>> FromDbAsync<T>(string basepath, string file)
        {
            return await UtilityDAL.CSV.CsvHelper.ParseAsync<T>(Path.Combine(basepath, file));
            //TextReader text = System.IO.File.OpenText(Path.Combine(dbName, file + ".csv"));
            //var csv = await CSVReaderFactory.BuildAsync(Path.Combine(basepath, file));

            //csv.Read();

            //var xx = csv.ReadHeader();
            //return await Task.Run(() =>
            //{
            //    var obs = Observable.Create<T>(async observer =>
            //    {
            //        while (true)

            //        {
            //            var x = await csv.ReadAsync();
            //            if (x == false) { text.Dispose(); break; }
            //            await Task.Run(() =>
            //            {
            //                try
            //                {
            //                    var record = csv.GetRecord<T>();
            //                    observer.OnNext(record);
            //                }
            //                catch (Exception ex)
            //                {
            //                    Console.WriteLine(ex);
            //                }
            //            });
            //        }

            //        return Disposable.Empty;
            //    });

            //    return obs;

            //});
        }

        public bool Clear(string name)
        {
            try
            {
                System.IO.File.Delete(Path.Combine(dbName, name.Replace(".csv", "") + ".csv"));

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private class Helper
        {
            public static IEnumerable<object> CombineCollections(params ICollection[] collections)
            {
                foreach (var collection in collections)
                {
                    foreach (var item in collection)
                    {
                        yield return item;
                    }
                }
            }
        }
    }

    //public class CSV //:IDatabaseService
    //{
    //    static readonly string dbName;

    //    static readonly string providerName = "CSV";

    //    static CSV()
    //    {
    //        dbName = Helper.GetConnectionString(providerName, false);

    //    }

    //    public static List<String> SelectIds()
    //    {
    //        return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
    //    }

    //    public static List<T> FromDb<T>(string name)
    //    {
    //        var csv =CSVReaderFactory.Build(dbName, name + ".csv");
    //        return csv.GetRecords<T>().ToList();
    //    }

    //    public static bool ToDb<T>(List<T> lst, string name)
    //    {
    //        using (TextWriter writer = new StreamWriter(Path.Combine(dbName, name + ".csv")))
    //        {
    //            var csv = new CsvWriter(writer);
    //            //csv.Configuration.Encoding = Encoding.UTF8;
    //            csv.WriteRecords(lst); // where values implements IEnumerable
    //        }
    //        return true;
    //    }

    //    public static IObservable<T> FromDbAsync<T>(string file)
    //    {
    //        return Observable.FromAsync(_ =>
    //         FromDbAsync<T>(dbName, file + ".csv")).Switch();
    //    }

    //    public static System.Collections.ICollection FromDb(string path)
    //    {
    //        return CsvHelper.Parse(path);
    //    }

    //    public static async Task<IObservable<T>> FromDbAsync<T>(string basepath, string file)
    //    {
    //        return await CsvHelper.ParseAsync<T>(Path.Combine(basepath,file));
    //        //TextReader text = System.IO.File.OpenText(Path.Combine(dbName, file + ".csv"));
    //        //var csv= await CSVReaderFactory.BuildAsync(Path.Combine(basepath, file));

    //        //csv.Read();

    //        //var xx = csv.ReadHeader();
    //        //return await Task.Run(() =>
    //        //{
    //        //    var obs = Observable.Create<T>(async observer =>
    //        //    {
    //        //        while (true)

    //        //        {
    //        //            var x = await csv.ReadAsync();
    //        //            if (x == false) { text.Dispose(); break; }
    //        //            await Task.Run(() =>
    //        //            {
    //        //                try
    //        //                {
    //        //                    var record = csv.GetRecord<T>();
    //        //                    observer.OnNext(record);
    //        //                }
    //        //                catch (Exception ex)
    //        //                {
    //        //                    Console.WriteLine(ex);
    //        //                }
    //        //            });
    //        //        }

    //        //        return Disposable.Empty;
    //        //    });

    //        //    return obs;

    //        //});
    //    }

    //}
}