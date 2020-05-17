using Optional;
using Optional.Collections;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityDAL.Sqlite
{

    public class LookUpTable
    {

        private readonly HashSet<string> fields;
        private readonly SQLiteConnection conn;
        private readonly string table;
        private readonly Dictionary<(string dataSource, string rowHeader, string alternateDataSource), Option<string, Exception>> dictionary = new Dictionary<(string dataSource, string rowHeader, string alternateDataSource), Option<string, Exception>>();
        private readonly Dictionary<(string dataSource, string rowHeader), Option<int, Exception>> dictionaryId = new Dictionary<(string dataSource, string rowHeader), Option<int, Exception>>();

        public LookUpTable(SQLiteConnection sQLiteConnection, string table)
        {
            this.conn = sQLiteConnection;
            this.fields = new HashSet<string>(conn.Query<Output>($"PRAGMA table_info('{table}')").Select(a => a.Name));
            this.table = table;
        }


        public Option<string, Exception> Get(string dataSource, string rowHeader, string alternateDataSource)
        {
            if (fields.Contains(dataSource) && fields.Contains(alternateDataSource))
            {
                try
                {
                    var ss = dictionary.GetValueOrNone((dataSource, rowHeader, alternateDataSource))
                        .FlatMap(a =>
                         conn
                         .Query<Output>(GetQuery())
                         .SingleOrNone()
                         .Map(va => va.Name)
                         .WithException(new Exception("No match for query " + GetQuery())))
                        .WithException(new Exception("No match in dictionary"));

                    dictionary[(dataSource, rowHeader, alternateDataSource)] = ss;
                    return ss;
                }
                catch (Exception ex)
                {
                    return Option.Some(string.Empty).WithException(ex);
                }
            }
            else if (fields.Contains(dataSource) == false)
            {
                return Option.None<string, Exception>(new Exception("DataSource does not contain field " + dataSource));
            }
            else if (fields.Contains(alternateDataSource) == false)
            {
                return Option.None<string, Exception>(new Exception("DataSource does not contain field " + alternateDataSource));
            }

            return Option.None<string, Exception>(new Exception("Unknown"));

            string GetQuery() => $"Select {Escape(alternateDataSource)} as {nameof(Output.Name)} from {table} where {Escape(dataSource)} = '{Escape(rowHeader)}'";
        }



        public Option<IEnumerable<string>, Exception> GetAll(string dataSource)
        {
            if (fields.Contains(dataSource))
            {
                return conn
                .Query<Output>(GetQuery())
                .Select(a => a.Name)
                .Some()
                .WithException(default(Exception));
            }
            else
            {
                return Option.None<IEnumerable<string>, Exception>(new Exception("DataSource does not contain field " + dataSource));
            }

            string GetQuery() => $"Select {Escape(dataSource)} as  {nameof(Output.Name)} from {table}";
        }

        public Option<int, Exception> GetId(string dataSource, string rowHeader)
        {
            if (fields.Contains(dataSource))
            {
                try
                {
                    var ss = dictionaryId.GetValueOrNone((dataSource, rowHeader))
                     .FlatMap(a =>
                     conn
                    .Query<Output>(GetQuery())
                    .Select(va => va.Id)
                    .SingleOrNone()
                    .WithException(new Exception("No match for query, " + GetQuery())))
                       .WithException(new Exception("No match in dictionary"));

                    dictionaryId[(dataSource, rowHeader)] = ss;
                    return ss;
                }
                catch (Exception ex)
                {
                    return Option.Some(default(int)).WithException(ex);
                }
            }
            else
            {
                return Option.None<int, Exception>(new Exception("DataSource does not contain field " + dataSource));
            }

            string GetQuery() => $"Select Id from {table} where {Escape(dataSource)} = '{Escape(rowHeader)}'";
        }

        static string Escape(string sql)
        {
            return sql.Replace("'", "''");
        }

        class Output
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
