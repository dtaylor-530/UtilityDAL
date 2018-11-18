using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using System.IO;
using System.Threading.Tasks;

namespace UtilityDAL
{
    public class CSVReaderFactory
    {


        public async static Task<CsvReader> BuildAsync(string path)
        {

            var text = System.IO.File.OpenText(path);

            var csv = await Task.Run(() => new CsvReader(text));


            csv.Configuration.MissingFieldFound = (a, b, c) =>
            {


            };
            //csv.Configuration.BadDataFound = null;
            csv.Configuration.HeaderValidated = (a, b, c, d) =>
            {

                if (!a)
                    Console.WriteLine(b[c] + " not validated");
            };

            // Remove underscores
            csv.Configuration.PrepareHeaderForMatch = header => header.Replace("_", string.Empty);
            // Ignore case
            csv.Configuration.PrepareHeaderForMatch = header => header.ToLower();
            // Trim
            csv.Configuration.PrepareHeaderForMatch = header => header?.Trim();
            // Remove whitespace
            csv.Configuration.PrepareHeaderForMatch = header => header.Replace(" ", string.Empty);
            // Remove points
            csv.Configuration.PrepareHeaderForMatch = header => header.Replace(".", string.Empty);

            return csv;
        }


        public static CsvReader Build(string basepath, string file)
        {

            var text = System.IO.File.OpenText(Path.Combine(basepath, (file)));

            var csv = new CsvReader(text);


            csv.Configuration.MissingFieldFound = (a, b, c) =>
            {


            };
            //csv.Configuration.BadDataFound = null;
            csv.Configuration.HeaderValidated = (a, b, c, d) =>
            {

                if (!a)
                    Console.WriteLine(b[c] + " not validated");
            };
            // Remove underscores
            csv.Configuration.PrepareHeaderForMatch = header => header.Replace("_", string.Empty);
            // Ignore case
            csv.Configuration.PrepareHeaderForMatch = header => header.ToLower();
            // Trim
            csv.Configuration.PrepareHeaderForMatch = header => header?.Trim();
            // Remove whitespace
            csv.Configuration.PrepareHeaderForMatch = header => header.Replace(" ", string.Empty);

            csv.Configuration.PrepareHeaderForMatch = header => header.Replace(".", string.Empty);

            return csv;
        }
    }
}
