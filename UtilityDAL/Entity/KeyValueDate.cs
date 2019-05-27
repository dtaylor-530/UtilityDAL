using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Entity
{
    public class KeyValueDate
    {
        public KeyValueDate()
        {

        }
        [SQLite.PrimaryKey]
        public string Key { get; set; }
        public string Value { get; set; }
        public long Date { get; set; }
    }
}
