using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UtilityStruct;

namespace UtilityDAL.Sqlite.Utility
{

    public static class ComparisonType
    {
        public static readonly string Equality = "=";
        public static readonly string InEquality = "<>";
        public static readonly string GreaterThan = ">";
        public static readonly string LessThan = "<";

    }


    public static class DateTimeFilter
    {
        public const string DateFormat = "yyyy-MM-dd hh:mm:ss.FFF";

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, Day day) where T : new() => conn.WhereEqual<T>(day, GetDateProperty<T>());

        public static List<T> WhereGreater<T>(this SQLite.SQLiteConnection conn, Day day) where T : new() => conn.WhereGreater<T>((DateTime)day, GetDateProperty<T>());

        public static List<T> WhereLess<T>(this SQLite.SQLiteConnection conn, Day day) where T : new() => conn.WhereLess<T>((DateTime)day, GetDateProperty<T>());

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DateTime date) where T : new() => conn.WhereEqual<T>(date, GetDateProperty<T>());

        public static List<T> WhereGreater<T>(this SQLite.SQLiteConnection conn, DateTime date) where T : new() => conn.WhereGreater<T>(date, GetDateProperty<T>());

        public static List<T> WhereLess<T>(this SQLite.SQLiteConnection conn, DateTime date) where T : new() => conn.WhereLess<T>(date, GetDateProperty<T>());

        public static List<T> WhereCompare<T>(this SQLite.SQLiteConnection conn, DateTime date, string comparison) where T : new()
          => conn.WhereCompare<T>(date, comparison, GetDateProperty<T>());

        public static List<T> WhereUnitCompare<T>(this SQLite.SQLiteConnection conn, int unit, string comparison, string format) where T : new()
               => conn.WhereUnitCompare<T>(unit, comparison, format, GetDateProperty<T>());

        public static List<T> WhereYearCompare<T>(this SQLite.SQLiteConnection conn, int year, string comparison) where T : new()
            => conn.WhereUnitCompare<T>(year, comparison, "Y", GetDateProperty<T>());

        public static List<T> WhereMonthCompare<T>(this SQLite.SQLiteConnection conn, int month, string comparison) where T : new()
                  => conn.WhereUnitCompare<T>(month, comparison, "M", GetDateProperty<T>());

        public static List<T> WhereDayCompare<T>(this SQLite.SQLiteConnection conn, int day, string comparison) where T : new()
                  => conn.WhereUnitCompare<T>(day, comparison, "d", GetDateProperty<T>());

        public static List<T> WhereHourCompare<T>(this SQLite.SQLiteConnection conn, int hour, string comparison) where T : new()
                  => conn.WhereUnitCompare<T>(hour, comparison, "H", GetDateProperty<T>());
        public static List<T> WhereMinuteCompare<T>(this SQLite.SQLiteConnection conn, int minute, string comparison) where T : new()
                  => conn.WhereUnitCompare<T>(minute, comparison, "m", GetDateProperty<T>());

        public static List<T> WhereSecondCompare<T>(this SQLite.SQLiteConnection conn, int second, string comparison) where T : new()
                  => conn.WhereUnitCompare<T>(second, comparison, "S", GetDateProperty<T>());


        public static List<T> WhereBetween<T>(this SQLite.SQLiteConnection conn, DateTime dateStart, DateTime dateEnd) where T : new()
            => conn.Query<T>(GetSelectStatement<T>() +
                $"where ({FormatByProperty(GetDateProperty<T>())}) Between ? AND ?;",
                         dateStart.ToString(DateFormat), dateEnd.ToString(DateFormat));

        public static List<T> Where<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, string comparison) where T : new()
=> conn.Query<T>(GetSelectStatement<T>() +
               $"where {FormatByProperty(GetDateProperty<T>())} {comparison} ? ;",
                               ((byte)day).ToString());

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DayOfWeek day) where T : new() => conn.Where<T>(day, ComparisonType.Equality, GetDateProperty<T>());

        public static List<T> Take<T>(this SQLite.SQLiteConnection conn, int number) where T : new() => conn.Query<T>($"select * from {typeof(T).GetSqliteName()} limit {number}");

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, Day day, PropertyInfo property) where T : new() => conn.WhereCompare<T>(day, ComparisonType.Equality, property);

        public static List<T> WhereGreater<T>(this SQLite.SQLiteConnection conn, Day day, PropertyInfo property) where T : new() => conn.WhereCompare<T>(day, ComparisonType.GreaterThan, property);

        public static List<T> WhereLess<T>(this SQLite.SQLiteConnection conn, Day day, PropertyInfo property) where T : new() => conn.WhereCompare<T>(day, ComparisonType.LessThan, property);

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DateTime date, PropertyInfo property) where T : new() => conn.WhereCompare<T>(date, ComparisonType.Equality, property);

        public static List<T> WhereGreater<T>(this SQLite.SQLiteConnection conn, DateTime date, PropertyInfo property) where T : new() => conn.WhereCompare<T>(date, ComparisonType.GreaterThan, property);

        public static List<T> WhereLess<T>(this SQLite.SQLiteConnection conn, DateTime date, PropertyInfo property) where T : new() => conn.WhereCompare<T>(date, ComparisonType.LessThan, property);

        public static List<T> WhereCompare<T>(this SQLite.SQLiteConnection conn, DateTime date, string comparison, PropertyInfo property) where T : new()
            => conn.Query<T>(GetSelectStatement<T>() +
                $"where {FormatByProperty(property)} {comparison} ? ;",
                date.ToString(DateFormat));

        public static List<T> WhereUnitCompare<T>(this SQLite.SQLiteConnection conn, int unit, string comparison, string format, PropertyInfo property) where T : new()
=> conn.Query<T>(GetSelectStatement<T>() +
$"where  strftime('%{format}', {FormatByProperty(property) }) {comparison} '{unit}';");

        public static List<T> WhereYearCompare<T>(this SQLite.SQLiteConnection conn, int year, string comparison, PropertyInfo property) where T : new()
            => conn.WhereUnitCompare<T>(year, comparison, "Y", property);


        public static List<T> WhereMonthCompare<T>(this SQLite.SQLiteConnection conn, int month, string comparison, PropertyInfo property) where T : new()
                  => conn.WhereUnitCompare<T>(month, comparison, "m", property);

        public static List<T> WhereDayCompare<T>(this SQLite.SQLiteConnection conn, int day, string comparison, PropertyInfo property) where T : new()
                  => conn.WhereUnitCompare<T>(day, comparison, "d", property);

        public static List<T> WhereHourCompare<T>(this SQLite.SQLiteConnection conn, int hour, string comparison, PropertyInfo property) where T : new()
                  => conn.WhereUnitCompare<T>(hour, comparison, "H", property);
        public static List<T> WhereMinuteCompare<T>(this SQLite.SQLiteConnection conn, int minute, string comparison, PropertyInfo property) where T : new()
                  => conn.WhereUnitCompare<T>(minute, comparison, "M", property);

        public static List<T> WhereSecondCompare<T>(this SQLite.SQLiteConnection conn, int second, string comparison, PropertyInfo property) where T : new()
                  => conn.WhereUnitCompare<T>(second, comparison, "S", property);


        public static List<T> WhereBetween<T>(this SQLite.SQLiteConnection conn, DateTime dateStart, DateTime dateEnd, PropertyInfo property) where T : new()
            => conn.Query<T>(GetSelectStatement<T>() +
                $"where  {FormatByProperty(property)} Between ? AND ?;",
                              dateStart.ToString(DateFormat), dateEnd.ToString(DateFormat));

        public static List<T> Where<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, string comparison, PropertyInfo property) where T : new()
=> conn.Query<T>(GetSelectStatement<T>() +
               $"where {FormatByProperty(property)} {comparison} ? ;",
                               ((byte)day).ToString());

        public static List<T> WhereEqual<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, PropertyInfo property) where T : new() => conn.Where<T>(day, "=", property);




        public static List<T> By<T>(this SQLite.SQLiteConnection conn, string match, string property, params Func<string, string>[] convertProperty) where T : new()
=> conn.Query<T>(GetSelectStatement<T>() +
               $"where {string.Join(" Or ", convertProperty.Select(a => $"{a(property)} = ?"))};",
                               match.ToString());

        // public static IEnumerable<T> ByDayOfWeek<T>(this SQLite.SQLiteConnection conn, Day day, string property = "Ticks") where T : new() =>
        //By<T>(conn,((DateTime)day).ToString(DateFormat),property,_ => $" strftime('%Y-%m-%d', {property}/ 10000000 - 62135596800,  'unixepoch')");
        // public static IEnumerable<T> ByDayOfWeek<T>(this SQLite.SQLiteConnection conn, DayOfWeek day, string property = "Ticks") where T : new() => By<T>(conn,((byte)day).ToString(),property,_ => $"strftime('%w', {_})");


        private static string FormatByProperty(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(DateTime))
            {
                return GetDateTimeString(propertyInfo.Name);
            }
            if (propertyInfo.PropertyType == typeof(long) && propertyInfo.Name.Contains("Tick"))
            {
                return GetDateTimeString(propertyInfo.Name);
            }
            //else if (propertyInfo.PropertyType == typeof(DateTime))
            //{
            //    return $"{propertyInfo.Name}";
            //}
            else
                throw new Exception($"Property {propertyInfo.Name} must be either DateTime or long (in which case needs to contain Tick).");
        }

        private static PropertyInfo GetDateProperty<T>()
        {
            foreach (var propertyInfo in typeof(T).GetProperties())
            {
                if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    return propertyInfo;
                }
                else if (propertyInfo.PropertyType == typeof(long) && propertyInfo.Name.Contains("Tick"))
                {
                    return propertyInfo;
                }
            }

            throw new Exception($"Type {typeof(T).Name} must contain a  DateTime property or long property with a name that contains 'Tick'.");
        }

        private static string GetDateTimeString(string field) => $"datetime({field}/ 10000000 - 62135596800,  'unixepoch')";

        private static string GetSelectStatement<T>() => $"select * from {typeof(T).GetSqliteName()} ";
    }
}