using System.Collections;
using System.ComponentModel;

namespace UtilityDAL.View
{
    [DescriptionAttribute("csv")]
    public class CsvFileParser : FileParser
    {
        public override ICollection Parse(string path) => UtilityDAL.CSV.CsvHelper.Parse(path);
    }
}