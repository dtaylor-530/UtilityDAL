using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace UtilityDAL
{
    public class XMLDb
    {
        private string _dbName;
        private static readonly XmlSerializerHelper xmlSerializerHelper;

        static XMLDb()
        {
            xmlSerializerHelper = new XmlSerializerHelper();
        }

        public XMLDb(string dbName)
        {
            _dbName = dbName;
        }

        public void Write<T>(T obj, string id)
        {
            // Act
            string serializedString = xmlSerializerHelper.SerializeToXml(obj);

            System.IO.File.WriteAllText(Path.Combine(_dbName, id) + ".xml", serializedString);
        }

        public T Read<T>(string id)
        {
            return (T)xmlSerializerHelper.DeserializeFromXml(typeof(T), File.ReadAllText(Path.Combine(_dbName, id) + ".xml"));
        }

        public Task<T> ReadAsync<T>(string id)
        {
            return Deserialize.DeSerializeFile<T>(File.ReadAllText(Path.Combine(_dbName, id)) + ".xml");

            //using (GenericParserAdapter parser = new GenericParserAdapter("MyData.txt"))
            //{
            //    parser.Load("MyData.xml");
            //    var dsResult = parser.GetDataSet();

            //}
        }
    }

    public static class Deserialize
    {
        public async static Task<T> DeSerializeFile<T>(string path, XmlSerializer serializer = null)
        {
            serializer = serializer ?? new XmlSerializer(typeof(T));

            return await Task.Run(() =>
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    T objects = default(T);

                    objects = (T)serializer.Deserialize(reader);

                    GC.Collect();

                    return objects;
                }
            });
        }
    }

    //private List<Transaction> ParseData()
    //{
    //    var xx = this.GetType().Assembly.GetManifestResourceNames();

    //    // The file SampleThings.xml needs to be set to an embedded resource in the properties window
    //    List<Transaction> results = new List<Transaction>();
    //    using (Stream str = this.GetType().Assembly.GetManifestResourceStream("DataService.BankTransactions.xml"))
    //    {
    //        using (XmlReader reader = new XmlTextReader(str))
    //        {
    //            XDocument doc = XDocument.Load(reader);
    //            var q = from item in doc.Descendants("tr") select item;

    //            foreach (var x in q)
    //                try
    //                {
    //                    results.Add(MakeTransaction(x.Elements("td").ToArray()));
    //                }
    //                catch (Exception e)
    //                {
    //                    switch (e.GetType().Name)
    //                    {
    //                        case (nameof(IndexOutOfRangeException)):
    //                            {
    //                                if (x.Elements("td").ToArray().Count() > 1)
    //                                    throw e;
    //                                break;
    //                            }
    //                        default:
    //                            {
    //                                break;
    //                            }

    //                    }
    //                }
    //        }
    //    }
    //    return results;
    //}
}