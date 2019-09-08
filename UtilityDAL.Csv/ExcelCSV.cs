using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Reflection;

namespace UtilityDAL.CSV
{
    //using LumenWorks.Framework.IO.Csv;
    //using ExcelDataReader;

    public static class ExcelDataReaderWrapper
    {
        public static DataSet ReadFromExcel(string databasefile)
        {
            //DataSet DataSet;
            using (var stream = File.Open(databasefile, FileMode.Open, FileAccess.Read))
            {
                //using (var reader = ExcelReaderFactory.CreateReader(stream))
                //{
                //    DataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                //    {

                //        // Gets or sets a value indicating whether to set the DataColumn.DataType 
                //        // property in a second pass.
                //        UseColumnDataType = false,
                //        // probably would have been easier to set this to false but it works as true


                //        // Gets or sets a callback to obtain configuration options for a DataTable. 
                //        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration() { UseHeaderRow = true }
                //    });

                //}
            }

            //return DataSet;
            return null;
        }


        public static DataTable ReadFromCsv(string databasefile)
        {
            //DataTable csvTable = new DataTable();
            //using (CsvReader csvReader =
            //    new CsvReader(new StreamReader(databasefile), true))
            //{
            //    csvTable.Load(csvReader);
            //}
            //return csvTable;
            return null;
        }

    }

}
