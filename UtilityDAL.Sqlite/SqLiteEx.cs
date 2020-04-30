#nullable enable

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

        public static void Create<T>(string path)
        {
            using (SQLiteConnection db = new SQLiteConnection(path))
            {
                var types = UtilityHelper.TypeHelper.GetTypesByAssembly<T>();

                foreach (var type in types)
                {
                    db.CreateTable(type, CreateFlags.AutoIncPK);
                }
            }
        }

        public static void Create(string path, params Type[] types)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            using (SQLiteConnection db = new SQLiteConnection(path))
            {
                foreach (var type in types)
                {
                    var types2 = UtilityHelper.TypeHelper.GetTypesByAssembly(type);

                    foreach (var type2 in types2)
                    {
                        db.CreateTable(type2, CreateFlags.AutoIncPK);
                    }
                }
            }
        }


        public static object?[][]? ToDataSet(this SQLiteConnection sqlConnection, string query, bool includeColumnNamesAsFirstRow = true)
        {
            var stQuery = SQLite3.Prepare2(sqlConnection.Handle, query);
            var colLength = SQLite3.ColumnCount(stQuery);
            try
            {
                return SelectRows().ToArray();
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                if (stQuery != null)
                {
                    SQLite3.Finalize(stQuery);
                }
            }

            IEnumerable<object?[]> SelectRows()
            {
                if (includeColumnNamesAsFirstRow)
                {
                    yield return SelectColumnNames(stQuery, colLength).ToArray();
                }

                while (SQLite3.Step(stQuery) == SQLite3.Result.Row)
                {
                    yield return SelectColumns(stQuery, colLength).ToArray();
                }

                static IEnumerable<object> SelectColumnNames(SQLitePCL.sqlite3_stmt stQuery, int colLength)
                {
                    for (int i = 0; i < colLength; i++)
                    {
                        yield return SQLite3.ColumnName(stQuery, i);
                    }
                }

                static IEnumerable<object?> SelectColumns(SQLitePCL.sqlite3_stmt stQuery, int colLength)
                {
                    for (int i = 0; i < colLength; i++)
                    {
                        var x = SQLitePCL.raw.sqlite3_column_decltype(stQuery, i);
                        yield return x switch
                        {
                            "text" => SQLite3.ColumnString(stQuery, i),
                            "integer" => SQLite3.ColumnInt(stQuery, i),
                            "bigint" => SQLite3.ColumnInt64(stQuery, i),
                            "real" => SQLite3.ColumnDouble(stQuery, i),
                            "blob" => SQLite3.ColumnBlob(stQuery, i),
                            "null" => null,
                            _ => throw new Exception($"Unexpected type encountered in for query {stQuery}")
                        };
                    }
                }
            }
        }      
    }
}