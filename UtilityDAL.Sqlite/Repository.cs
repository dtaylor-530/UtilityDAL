using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UtilityDAL.Common;
using UtilityInterface.Generic.Database;

namespace UtilityDAL.Sqlite
{
    public class Repository<T> : Repository<T, IConvertible> where T : new()
    {
        public Repository(Func<T, IConvertible> getkey, string dbname = null) : base(getkey, dbname)
        {
        }
    }

    public class Repository<T, R> : IDatabaseService<T, R> where T : new()
    {
        private SQLite.SQLiteConnection connection;
        private Func<T, R> getId;
        private static readonly string providerName = "SQLite";

        public Repository(Func<T, R> getId, string dbname = null)
        {
            var directory = Directory.CreateDirectory(Constants.DefaultDbDirectory);
            dbname = dbname ?? DbEx.GetConnectionString(providerName, false);
            this.getId = getId;
            this.connection = new SQLite.SQLiteConnection(string.IsNullOrEmpty(dbname) || string.IsNullOrWhiteSpace(dbname) ?
                Path.Combine(directory.FullName, typeof(T).Name + "." + Constants.SqliteDbExtension) :
                dbname);
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