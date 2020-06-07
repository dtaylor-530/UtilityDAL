using Optional;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityDAL
{
    public static class SqliteEx
    {

        public static string GetName(this Type type) => ((SQLite.TableAttribute[])type.GetCustomAttributes(typeof(SQLite.TableAttribute), false)).FirstOrDefault()?.Name ?? type.Name;


        public static long GetNextId<T>(this SQLiteConnection db, Func<T, long> expression) where T : new()=> db.GetMaxId(expression) + 1;

        public static long GetMaxId<T>(this SQLiteConnection db, Func<T, long> expression) where T : new()
        {
            return db.Table<T>().Max(expression);
        }

        public static bool TableExists<T>(this SQLiteConnection connection) => TableExists(connection, GetName(typeof(T)));

        public static bool TableExists(this SQLiteConnection connection, string tableName) => connection.Query<NameTable>($"SELECT name FROM sqlite_master WHERE name='{tableName}'").Count > 0;

        class NameTable
        {
            string Name { get; set; }
        }

        public static bool RemoveDuplicates<T>(this SQLiteConnection connection) where T : IEquatable<T>, new()
        {
            return RemoveDuplicates<T>(connection, ()=>connection.Table<T>().ToList().Distinct());
        }

        public static bool RemoveDuplicates<T>(this SQLiteConnection connection, IEqualityComparer<T> equalityComparer ) where T : new()
        {
            return RemoveDuplicates<T>(connection,()=> connection.Table<T>().ToList().Distinct(equalityComparer));
        }

        static bool RemoveDuplicates<T>(this SQLiteConnection connection, Func<IEnumerable<T>> funcEnumerable) where T : new()
        {
            var dir = System.IO.Directory.GetParent(connection.DatabasePath);

            connection.Backup(dir.FullName + "\\Backup");
            var table = funcEnumerable().ToArray(); 

            connection.DropTable<T>();

            if (connection.CreateTable<T>() == CreateTableResult.Created)
            {
                connection.InsertAll(table);
                return true;
            }
            else
            {
                return false;
            }
        }



        public static Option<Option<object, Exception>[][], Exception> ToDataSet(this SQLiteConnection sqlConnection, string query, bool includeColumnNamesAsFirstRow = true)
        {
            var stQuery = SQLite3.Prepare2(sqlConnection.Handle, query);
            var colLength = SQLite3.ColumnCount(stQuery);
            try
            {
                return SelectRows().ToArray().Some().WithException(new Exception());
            }
            catch (Exception e)
            {
                return Option.None<Option<object, Exception>[][]>().WithException(new Exception($"Unexpected type encountered in for query {stQuery}"));
            }
            finally
            {
                if (stQuery != null)
                {
                    SQLite3.Finalize(stQuery);
                }
            }

            IEnumerable<Option<object, Exception>[]> SelectRows()
            {
                if (includeColumnNamesAsFirstRow)
                {
                    yield return SelectColumnNames().ToArray();
                }

                while (SQLite3.Step(stQuery) == SQLite3.Result.Row)
                {
                    yield return SelectColumns().ToArray();
                }

                IEnumerable<Option<object, Exception>> SelectColumnNames()
                {
                    for (int i = 0; i < colLength; i++)
                    {
                        yield return SQLite3.ColumnName(stQuery, i).Some<object>().WithException(new Exception()); 
                    }
                }

                IEnumerable<Option<object,Exception>> SelectColumns()
                {
                    for (int i = 0; i < colLength; i++)
                    {
                        switch (SQLitePCL.raw.sqlite3_column_decltype(stQuery, i))
                        {
                           case "text": yield return SQLite3.ColumnString(stQuery, i).Some<object>().WithException(new Exception()); break;
                           case "integer" : yield return SQLite3.ColumnInt(stQuery, i).Some<object>().WithException(new Exception()); break;
                            case "bigint" : yield return SQLite3.ColumnInt64(stQuery, i).Some<object>().WithException(new Exception()); break;
                            case "real" : yield return SQLite3.ColumnDouble(stQuery, i).Some<object>().WithException(new Exception()); break;
                            case "blob": yield return SQLite3.ColumnBlob(stQuery, i).Some<object>().WithException(new Exception()); break;
                            case "null": yield return Option.None<object>().WithException(new Exception("Case is null")); break;
                            default: Option.None<object>().WithException(new Exception($"Unexpected type encountered in for query {stQuery}")); break;
                        }
                    }
                }
            }
        }      
    }
}