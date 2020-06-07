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
    [Description("tea")]
    public class TeaFileParser : FileParser
    {
        public override ICollection Parse(string path)
        {
            using (var tf = TeaTime.TeaFile<Price>.OpenRead(path))
            {
                return tf.Items.ToList();//.Select(a => new { Date = new DateTime(a.Date.Ticks), a.Bid, a.Offer }).ToList();
                //return new[]{new Model.Log { Date = DateTime.Now, Issue = Model.Issue.Error, Key = "dd" },
                //    new Model.Log { Date = DateTime.Now, Issue = Model.Issue.Error, Key = "dd" },
                //    new Model.Log { Date = DateTime.Now, Issue = Model.Issue.Error, Key = "dd" }};
                return tf.Items.ToList().Select(a => new { Date= new DateTime(a.Date.Ticks), a.Bid, a.Offer }).ToList();
            }
        }
    }
}