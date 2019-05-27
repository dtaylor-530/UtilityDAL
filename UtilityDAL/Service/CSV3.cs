
using GenericParsing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using UtilityDAL.Contract;

namespace UtilityDAL
{
    public class CSV3 : IFileDbService
    {


      //  static readonly string dbName;

        static readonly string providerName = "CSV";
        private readonly string dbName;

        //static CSV3()
        //{
        //    dbName = DbEx.GetConnectionString(providerName, false);
        //}

        public  CSV3(string name=null)
        {
            if (string.IsNullOrEmpty(name))
                dbName = DbEx.GetConnectionString(providerName, false);
            else
                dbName = name;
        }

        public CSV3()
        {
          
        }

        public System.Collections.ICollection FromDb(string name)
        {

            // Using an XML Config file. 
           return new CSV(dbName).FromDb(name);
        }



        public bool ToDb(ICollection lst, string name)
        {
            var lst2 = lst.Cast<object>().Select(_ => new RecordWrap2(UtilityHelper.ObjectToDictionaryMapper.ToDictionary(_))); 

            using (StreamWriter writer = File.CreateText(Path.Combine(dbName, name + ".csv")))
            {
                CoreTechs.Common.Text.CsvWriter.Write(lst2, writer);
            }
            return true;
        }



        public List<string> SelectIds()
        {
            return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
        }


    }






    public class RecordWrap2 : CoreTechs.Common.Text.ICsvWritable
    {
        public IDictionary<string, object> Record { get;}

        public RecordWrap2(IDictionary<string,object> record)
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


    public class CSVParserFactory
    {


        //public static void Build(ref GenericParser parser)
        //{
 
        //        //parser.SetDataSource("MyData.txt");

        //    parser.ColumnDelimiter = "\t".ToCharArray();
        //    parser.FirstRowHasHeader = true;
        //    parser.SkipStartingDataRows = 10;
        //    parser.MaxBufferSize = 4096;
        //    parser.MaxRows = 500;
        //    parser.TextQualifier = '\"

        //    return csv;
        //}


   
    }





    //public class XML<T> : IDbService<T>
    //{
    //    public List<T> FromDb()
    //    {
    //        using (GenericParserAdapter parser = new GenericParserAdapter("MyData.txt"))
    //        {
    //            parser.Load("MyData.xml");
    //            dsResult = parser.GetDataSet();
    //        }
    //    }
    //}
}



//https://www.codeproject.com/Articles/11698/A-Portable-and-Efficient-Generic-Parser-for-Flat-F

//// Using an XML Config file. 
//using (GenericParserAdapter parser = new GenericParserAdapter("MyData.txt"))
//{
//    parser.Load("MyData.xml");
//    dsResult = parser.GetDataSet();
//}

//// Or... programmatically setting up the parser for TSV. 
//string strID, strName, strStatus;
//using (GenericParser parser = new GenericParser())
//{
//    parser.SetDataSource("MyData.txt");

//    parser.ColumnDelimiter = "\t".ToCharArray();
//parser.FirstRowHasHeader = true;
//    parser.SkipStartingDataRows = 10;
//    parser.MaxBufferSize = 4096;
//    parser.MaxRows = 500;
//    parser.TextQualifier = '\"';

//    while (parser.Read())
//    {
//      strID = parser["ID"];
//      strName = parser["Name"];
//      strStatus = parser["Status"];

//      // Your code here ...
//    }
//}

//// Or... programmatically setting up the parser for Fixed-width. 
//  using (GenericParser parser = new GenericParser())
//  {
//    parser.SetDataSource("MyData.txt");

//    parser.ColumnWidths = new int[4] {10, 10, 10, 10};
//    parser.SkipStartingDataRows = 10;
//    parser.MaxRows = 500;

//    while (parser.Read())
//    {
//      strID = parser["ID"];
//      strName = parser["Name"];
//      strStatus = parser["Status"];

//      // Your code here ...
//    }
//}