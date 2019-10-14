//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace UtilityDAL
//{
//   public class Text
//    {
//    static readonly string dbName;

//    static readonly string providerName = "CSV";

//    //static CSV(string path = null)
//    //{
//    //    if (path == null)
//    //        dbName = Helper.GetConnectionString(providerName, false);
//    //    else
//    //        dbName = path;

//    //}
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
//        var csv = CSVReaderFactory.Build(dbName, name + ".csv");
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
//        // Using an XML Config file.
//        using (GenericParserAdapter parser = new GenericParserAdapter(path))
//        {
//            parser.ColumnDelimiter = ',';
//            parser.FirstRowHasHeader = true;
//            //parser.SkipStartingDataRows = 10;
//            parser.MaxBufferSize = 4096;
//            //parser.MaxRows = 500;
//            parser.TextQualifier = '\"';

//            //  parser.Load("MyData.xml");
//            return parser.GetDataTable().DefaultView;
//        }
//    }

//    public static async Task<IObservable<T>> FromDbAsync<T>(string basepath, string file)
//    {
//        TextReader
//         text = System.IO.File.OpenText(Path.Combine(dbName, file + ".csv"));
//        var csv = await CSVReaderFactory.BuildAsync(basepath, file);

//        csv.Read();

//        var xx = csv.ReadHeader();
//        return await Task.Run(() =>
//        {
//            var obs = Observable.Create<T>(async observer =>
//            {
//                while (true)

//                {
//                    var x = await csv.ReadAsync();
//                    if (x == false) { text.Dispose(); break; }
//                    await Task.Run(() =>
//                    {
//                        try
//                        {
//                            var record = csv.GetRecord<T>();
//                            observer.OnNext(record);
//                        }
//                        catch (Exception ex)
//                        {
//                            Console.WriteLine(ex);
//                        }
//                    });
//                }

//                return Disposable.Empty;
//            });

//            return obs;

//        });
//    }

//}