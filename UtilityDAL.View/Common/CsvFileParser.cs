using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL.View
{
    [DescriptionAttribute("csv")]
    public class CsvFileParser : FileParser
    {
        public override ICollection Parse(string path) => UtilityDAL.CsvHelper.Parse(path);
        
        public override string Map(string value) => value;

    }



}
