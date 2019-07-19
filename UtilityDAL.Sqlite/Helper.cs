using System;
using System.Collections.Generic;
using System.Text;
using UtilityStruct;

namespace UtilityDAL.Sqlite
{
    public static class Helper
    {

        public static IEnumerable<T> FilterByDay<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new()
            => conn.Query<T>($"select *  from {typeof(T).Name} " +
                             $"where  strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch') = ? ; ",
                                             ((DateTime)day).ToString("yyyy-MM-dd"));



    }
}
