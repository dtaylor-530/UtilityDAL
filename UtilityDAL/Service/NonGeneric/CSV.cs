using CsvHelper;
using GenericParsing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL.Contract;
using UtilityDAL.Contract.NonGeneric;

namespace UtilityDAL
{
    public class CSV : IFileDbService
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


        public ICollection FromDb(string name)
        {
            var text = Path.Combine(dbName, name.Replace(".csv", "") + ".csv");
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

        public bool ToDb(ICollection lst, string name)
        {
            var lst2 = lst.Cast<object>().Select(_ => new RecordWrap2(UtilityHelper.ObjectToDictionaryMapper.ToDictionary(_)));

            using (StreamWriter writer = File.CreateText(Path.Combine(dbName, name.Replace(".csv", "") + ".csv")))
            {
                CoreTechs.Common.Text.CsvWriter.Write(lst2, writer);
            }
            return true;
        }



        //public IObservable<dynamic> FromDbAsync(string name)
        //{
        //    return Observable.FromAsync(_ =>
        //  new CSV().FromDbAsync(dbName, name + ".csv")).Switch();


        //}
        public async Task<IObservable<T>> FromDbAsync<T>(string basepath, string file)
        {
            return await CsvHelper.ParseAsync<T>(Path.Combine(basepath, file));
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



    }

    //public class CSV //:IDbService
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