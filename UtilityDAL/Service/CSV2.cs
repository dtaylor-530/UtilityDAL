
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UtilityDAL.Contract;

namespace UtilityDAL
{
    public class CSV2 : IFileDbService<RecordWrap>
    {

        //static TextReader text;



        static readonly string dbName;

        static readonly string providerName = "CSV";



        static CSV2()
        {
            dbName = DbEx.GetConnectionString(providerName, false);

        }


        public IList<RecordWrap> FromDb(string name)
        {

            var text = File.OpenText(Path.Combine(dbName, name+".csv"));
            return CoreTechs.Common.Text.CsvParsingExtensions.ReadCsvWithHeader(text).ToList().Select(_=>new RecordWrap(_)).ToList();
    

        }


        public bool ToDb(IList<RecordWrap> lst, string name)
        {
            using (StreamWriter writer = File.CreateText(Path.Combine(dbName, name + ".csv")))
            {
                CoreTechs.Common.Text.CsvWriter.Write(lst, writer);
            }
            return true;
        }


        public List<string> SelectIds()
        {
            return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
        }

      
    }



    public class  RecordWrap : CoreTechs.Common.Text.ICsvWritable
    {
        public CoreTechs.Common.Text.Record Record { get; set; }

        public RecordWrap(CoreTechs.Common.Text.Record record)
        {
            Record = record;
        }



        public IEnumerable<object> GetCsvHeadings()
        {
            return Record.Keys;
        }

        public IEnumerable<object> GetCsvFields()
        {
            return Record.Values;
        }
    }


    
}


