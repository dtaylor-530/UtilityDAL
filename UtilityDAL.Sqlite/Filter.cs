using System;
using System.Collections.Generic;
using System.Text;
using UtilityStruct;
using UtilityHelper;
using System.Linq;

namespace UtilityDAL.Sqlite
{
    public static class Filter
    {
        public static List<T> Take<T>(this SQLite.SQLiteConnection conn, int number) where T : new() => conn.Query<T>($"select * from {typeof(T).GetName()} limit {number}");

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new()  => WhereCompare<T>(conn, (DateTime)day, "=", property);

        public static List<T> WhereGreater<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new() => WhereCompare<T>(conn, (DateTime)day, ">", property);

        public static List<T> WhereLess<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new() => WhereCompare<T>(conn, (DateTime)day, "<", property);

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DateTime date, string property = "Ticks") where T : new() => WhereCompare<T>(conn, (DateTime)date, "=", property);

        public static List<T> WhereGreater<T>(this SQLite.SQLiteConnection conn, DateTime date, string property = "Ticks") where T : new() => WhereCompare<T>(conn, (DateTime)date, ">", property);

        public static List<T> WhereLess<T>(this SQLite.SQLiteConnection conn, DateTime date, string property = "Ticks") where T : new() => WhereCompare<T>(conn, (DateTime)date, "<", property);

        public static List<T> WhereCompare<T>(this SQLite.SQLiteConnection conn, DateTime date, string comparison, string property = "Ticks") where T : new()
            => conn.Query<T>($"select *  from {typeof(T).GetName()} " +
                $"where  strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch') {comparison} ? ;",
                (date).ToString("yyyy-MM-dd"));

        public static List<T> WhereBetween<T>(this SQLite.SQLiteConnection conn, DateTime dateStart, DateTime dateEnd, string property = "Ticks") where T : new()
            => conn.Query<T>($"select *  from {typeof(T).GetName()} " +
                $"where  strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch') Between ? AND ?;",
                             (dateStart).ToString("yyyy-MM-dd"), (dateEnd).ToString("yyyy-MM-dd"));
        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, string comparison = "=", string property = "Ticks") where T : new()
=> conn.Query<T>($"select *  from {typeof(T).GetName()} " +
               $"where strftime('%w', {property}) {comparison} ? ;",
                               ((byte)day).ToString());

        public static List<T> By<T>(this SQLite.SQLiteConnection conn, string match, string property, params Func<string, string>[] convertProperty) where T : new()
=> conn.Query<T>($"select *  from {typeof(T).GetName()} " +
               $"where {string.Join(" Or ", convertProperty.Select(_ => $"{_(property)} = ?"))};",
                               (match).ToString());

        // public static IEnumerable<T> ByDayOfWeek<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new() =>
        //By<T>(conn,((DateTime)day).ToString("yyyy-MM-dd"),property,_ => $" strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch')");
        // public static IEnumerable<T> ByDayOfWeek<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, string property = "Ticks") where T : new() => By<T>(conn,((byte)day).ToString(),property,_ => $"strftime('%w', {_})");


    }
}


