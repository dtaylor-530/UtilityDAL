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
                return conn
                    .Query<Output>($"Select {alternateDataSource} as {nameof(Output.Name)} from {table} where {dataSource} = '{rowHeader}'")
                    .SingleOrNone()
                    .Map(a => a.Name)
                    .WithException(new Exception(""));
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
        }



        public Option<IEnumerable<string>, Exception> GetAll(string dataSource)
        {
            if (fields.Contains(dataSource))
            {
                return conn
                    .Query<Output>($"Select {dataSource} as  {nameof(Output.Name)} from {table}").Select(a => a.Name)
                    .Some()
                    .WithException(new Exception());
            }
            else
            {
                return Option.None<IEnumerable<string>, Exception>(new Exception("DataSource does not contain field " + dataSource));
            }

        }

        public Option<int, Exception> GetId(string dataSource, string rowHeader)
        {
            if (fields.Contains(dataSource))
            {
                return conn
                    .Query<Output>($"Select Id from {table} where {dataSource} = '{rowHeader}'").Select(a => a.Id)
                    .SingleOrNone()
                    .WithException(new Exception($"{dataSource}"));
            }
            else
            {
                return Option.None<int, Exception>(new Exception("DataSource does not contain field " + dataSource));
            }
        }

        class Output
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }
    }
}
