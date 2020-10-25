using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UtilityDAL.Common;
using UtilityInterface.Generic.Database;

namespace UtilityDAL.Sqlite
{
    public class Repository_Legacy<T> : SQLiteConnectionyWrapper<T, IConvertible> where T : new()
    {
        public Repository_Legacy(Func<T, IConvertible> getkey, string dbname = null) : base(getkey, dbname)
        {
        }
    }

    public class SqliteConnectionWrapperIId<T, R> : SQLiteConnectionyWrapper<T, R> where T : IId<R>, new()
    {
        public SqliteConnectionWrapperIId(string dbname = null, string dbDirectory = null) : base(getId: t => t.Id, dbname, dbDirectory)
        {
        }
    }

    public class SQLiteConnectionyWrapper<T, R> : IDatabaseService<T, R> where T : new()
    {
        private readonly SQLite.SQLiteConnection connection;
        private static readonly string providerName = "SQLite";
        private readonly Func<T, R> getId;


        public SQLiteConnectionyWrapper(Func<T, R> getId, string dbName = null, string dbDirectory = null)
        {
            this.getId = getId;
            dbDirectory = dbDirectory ?? Directory.CreateDirectory(Constants.DefaultDirectory).FullName;
            dbName = dbName ?? DbEx.GetConnectionString(providerName, false) ?? typeof(T).Name;
            this.connection = new SQLite.SQLiteConnection(string.IsNullOrEmpty(dbName) || string.IsNullOrWhiteSpace(dbName) ?
                Path.Combine(dbDirectory, typeof(T).Name + "." + Constants.SqliteExtension) :
                dbName);
            connection.CreateTable<T>();
        }


        public bool Delete(T item)
        {
            connection.Delete(item);
            return true;
        }

        public bool Insert(T item)
        {
            connection.Insert(item);
            return true;
        }

        public int InsertBulk(IList<T> items)
        {
            return connection.InsertAll(items);
        }

        public bool Update(T item)
        {
            connection.Update(item);
            return true;
        }

        public void Dispose()
        {
            connection.Dispose();
        }

        public IEnumerable<T> SelectAll()
        {
            return connection.Table<T>().ToList();
        }

        public T Select(T item)
        {
            return connection.Table<T>().SingleOrDefault(_ => _.Equals(item));
        }

        public T SelectById(R id)
        {
            return connection.Table<T>().SingleOrDefault(_ => getId(_).Equals(id));
        }

        public int InsertBulk(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }

        public int UpdateBulk(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }

        public int DeleteBulk(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(R id)
        {
            return connection.Delete(connection.Table<T>().SingleOrDefault(_ => getId(_).Equals(id))) > 0;
        }

        //public ICollection<T> FromDb<T>(string name) where T: IChildRow, new()
        //{
        //    return UtilityDAL.SqliteEx.FromDb<T>();

        //}

        //public bool ToDb<T>(ICollection<T> lst, string name) where T : IChildRow, new()
        //{
        //    return UtilityDAL.SqliteEx.ToDb<T>(lst);
        //}

        //public List<string> SelectIds()
        //{
        //    return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
        //}
    }

}