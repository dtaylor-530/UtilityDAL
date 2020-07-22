using System;

namespace UtilityDAL.Model
{
    public class KeyValueDate
    {
        public KeyValueDate()
        {
        }

        [SQLite.PrimaryKey]
        public string Key { get; set; }

        public string Value { get; set; }

        public DateTime Date { get; set; }
    }
}