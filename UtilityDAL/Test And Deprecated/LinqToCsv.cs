using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL
{
    class LinqToCsv
    {
    }
}
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FootballData.Model
//{
//    using LINQtoCSV;
//    using System.IO;


//    public sealed class MatchesStore
//    {

//        public static List<FootballDataMatch> defaultMatches { get; private set; }
//        private static List<FootballDataMatch> matches;

//        private static readonly string fileName = "/S01718.csv";

//        static MatchesStore()
//        {
//            defaultMatches = RetrieveDefinitionsFromCSV();

//        }

//        public static List<FootballDataMatch> Matches
//        {

//            get { return matches ?? defaultMatches; }

//            set { matches = value; }
//        }

//        public static List<FootballDataMatch> RetrieveDefinitionsFromCSV(string filePath = null)
//        {
//            filePath = filePath ?? Directory.GetCurrentDirectory() + "\\" + fileName;
//            int? season=null;

//            //if (Helpers.IsYear(Path.GetFileNameWithoutExtension(filePath)))
//            //    season = int.Parse(Path.GetFileNameWithoutExtension(filePath));

//            if (!File.Exists(filePath))
//            {

//                throw new Exception("file does not exist");
//            }



//            if (new FileInfo(filePath).IsFileLocked())
//            {
//                throw new Exception("file being used by some other process");

//            }


//            // Check Resource can be accessed

//            try
//            {
//                File.Open(filePath, FileMode.Open, FileAccess.Read).Dispose();

//            }
//            catch (IOException)
//            {
//                throw new Exception("Trouble Opening CSV - it could be open in another program or moved from its original folder");
//            }


//            // use external DLL LINQTOCSV to read file into list;
//            CsvFileDescription inputFileDescription = new CsvFileDescription
//            {
//                SeparatorChar = ',',
//                FirstLineHasColumnNames = true,
//                FileCultureName = "en-GB",
//                IgnoreUnknownColumns = true,
//                IgnoreTrailingSeparatorChar = true
//            };

//            CsvContext cc = new CsvContext();
//            var matches = cc.Read<FootballDataMatch>(filePath, inputFileDescription).ToList();
//            //   var dict = defs.ToDictionary(x => x.Variable, x => x);

//            //if (season != null) matches.ForEach(x => x.Season = (int)season);


//            return matches;

//        }



//    }


//}
