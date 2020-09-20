using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Globalization;
using System.IO;

namespace UtilityDAL.CSV
{
    public class CSVReaderFactory
    {
        public static CsvReader Build(string basepath, string file)
        {
            return Build(Path.Combine(basepath, (file)));
        }

        public static CsvReader BuildFromText(string text)
        {
            var reader = new StringReader(text);

            var csv = new CsvReader(reader, new CsvConfiguration(cultureInfo:CultureInfo.InvariantCulture));

            Build(csv.Configuration);
            return csv;
        }

        public static CsvReader Build(string path)
        {
            var text = System.IO.File.OpenText(path);
            var csv = new CsvReader(text, new CsvConfiguration(cultureInfo: CultureInfo.InvariantCulture));

            Build(csv.Configuration);
            return csv;
        }

        private static IReaderConfiguration Build(IReaderConfiguration configuration)
        {
            configuration.MissingFieldFound = (a, b, c) =>
            {
            };

            configuration.BadDataFound = (a) =>
            {
            };

            configuration.ReadingExceptionOccurred =
                (a) =>
                false;

            //csv.Configuration.BadDataFound = null;
            configuration.HeaderValidated = (a, b, c, d) =>
            {
                if (!a)
                    Console.WriteLine(b[c] + " not validated");
            };
            // Remove underscores
            configuration.PrepareHeaderForMatch = (header, i) => header.Replace("_", string.Empty);
            // Ignore case
            configuration.PrepareHeaderForMatch = (header, i) => header.ToLower();
            // Trim
            configuration.PrepareHeaderForMatch = (header, i) => header?.Trim();
            // Remove whitespace
            configuration.PrepareHeaderForMatch = (header, i) => header.Replace(" ", string.Empty);

            configuration.PrepareHeaderForMatch = (header, i) => header.Replace(".", string.Empty);

            return configuration;
        }
    }
}