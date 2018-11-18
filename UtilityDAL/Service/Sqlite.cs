﻿using System;
using System.Collections.Generic;
using System.Linq;
//using System.Data.SQLite;
using System.Text;
using UtilityInterface.Database;

namespace UtilityDAL
{
    public class Sqlite<T> : Sqlite<T, IConvertible> where T : new()
    {
        public Sqlite(Func<T, IConvertible> getkey, string dbname = null) : base(getkey, dbname)
        {

        }
    }
        public class Sqlite<T,R> : IDbService<T,R> where T:new()
    {


        //static readonly string _dbName;
        private static  SQLite.SQLiteConnection _connection;
        private Func<T, R> _getkey;
        static readonly string providerName = "SQLite";

        static Sqlite()
        {
            var dbName = DbEx.GetConnectionString(providerName, false);
            _connection = new SQLite.SQLiteConnection(dbName);
         
        }

        public Sqlite(Func<T,R> getkey, string dbname = null)
        {
            _getkey = getkey;
            if (dbname != null)
            {
                _connection = new SQLite.SQLiteConnection(dbname);
            }
        }

        public bool Delete(T item)
        {
            _connection.Delete(item);
            return true;
        }

        public IEnumerable<T> FindAll()
        {
            return _connection.Table<T>().ToList();
        }

        public T Find(T item)
        {
            return _connection.Table<T>().SingleOrDefault(_ => _.Equals(item));
        }
        public T FindById(R item)
        {
            return _connection.Table<T>().SingleOrDefault(_ =>_getkey(_).Equals(item));
        }
        public bool Insert(T item)
        {
            _connection.Insert(item);
            return true;
        }

        public int InsertBulk(IList<T> items)
        {
            return _connection.Insert(items);
        }

        public bool Update(T item)
        {
            _connection.Update(item);
            return true;
        }

        public void Dispose()
        {
            _connection.Dispose();
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
