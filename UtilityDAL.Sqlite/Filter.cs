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

        public static IEnumerable<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new()
            => conn.Query<T>($"select *  from {typeof(T).GetName()} " +
                             $"where  strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch') = ? ;",
                                             ((DateTime)day).ToString("yyyy-MM-dd"));


        public static IEnumerable<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DateTime date, string property = "Ticks") where T : new()
    => conn.Query<T>($"select *  from {typeof(T).GetName()} " +
                     $"where  strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch') = ? ;",
                                     (date).ToString("yyyy-MM-dd"));


        public static IEnumerable<T> WhereBetween<T>(this SQLite.SQLiteConnection conn, DateTime dateStart, DateTime dateEnd, string property = "Ticks") where T : new()
=> conn.Query<T>($"select *  from {typeof(T).GetName()} " +
             $"where  strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch') Between ? AND ?;",
                             (dateStart).ToString("yyyy-MM-dd"), (dateEnd).ToString("yyyy-MM-dd"));

        // public static IEnumerable<T> ByDayOfWeek<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new() =>
        //By<T>(conn,((DateTime)day).ToString("yyyy-MM-dd"),property,_ => $" strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch')");


        public static IEnumerable<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, string property = "Ticks") where T : new()
      => conn.Query<T>($"select *  from {typeof(T).GetName()} " +
                       $"where  strftime('%w', {property}) = ? ;",
                                       ((byte)day).ToString());

       // public static IEnumerable<T> ByDayOfWeek<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, string property = "Ticks") where T : new() => By<T>(conn,((byte)day).ToString(),property,_ => $"strftime('%w', {_})");


        public static IEnumerable<T> By<T>(this SQLite.SQLiteConnection conn, string match, string property, params Func<string, string>[] convertProperty) where T : new()
=> conn.Query<T>($"select *  from {typeof(T).GetName()} " +
               $"where {string.Join(" Or ", convertProperty.Select(_ => $"{_(property)} = ?"))};",
                               (match).ToString());

        private static string GetName(this Type type) => ((SQLite.TableAttribute[])type.GetCustomAttributes(typeof(SQLite.TableAttribute), false)).FirstOrDefault()?.Name ?? type.Name;

    }
}


