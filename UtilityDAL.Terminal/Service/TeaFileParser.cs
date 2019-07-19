using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL.View;

namespace UtilityDAL.DemoApp
{
    [DescriptionAttribute("tea")]
    public class TeaFileParser : FileParser
    {
        public override ICollection Parse(string path)
        {
            using (var tf = TeaTime.TeaFile<Price>.OpenRead(path))
            {
                return tf.Items.ToList();
            }
        }

        public override string Map(string value) => value;

    }

}
