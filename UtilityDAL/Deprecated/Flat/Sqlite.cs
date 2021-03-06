﻿//using System;
//using System.Collections.Generic;
//using System.Linq;
////using System.Data.SQLite;
//using System.Text;
//using UtilityDAL.Common;
//using UtilityDAL.Contract;
//using UtilityInterface.Generic.Database;

//namespace UtilityDAL
//{
//    public class Sqlite<T> : Sqlite<T, IConvertible> where T : new()
//    {
//        public Sqlite(Func<T, IConvertible> getkey, string dbname = null) : base(getkey, dbname)
//        {
//        }
//    }

//    public class Sqlite<T, R> : IDatabaseService<T, R> where T : new()
//    {
//        //static readonly string _dbName;
//        private static SQLite.SQLiteConnection _connection;
//        private Func<T, R> getId;
//        static readonly string providerName = "SQLite";

//        static Sqlite()
//        {
//            System.IO.Directory.CreateDirectory("../../Data");
//            var dbName = DbEx.GetConnectionString(providerName, false);
//            _connection = new SQLite.SQLiteConnection(dbName == string.Empty ? @"../../Data/" + typeof(T).Name + "." + Constants.SqliteDbExtension : dbName);
//            _connection.CreateTable<T>();
//        }

//        public Sqlite(Func<T, R> getId, string dbname = null)
//        {
//            this.getId = getId;
//            if (dbname != null)
//            {
//                _connection = new SQLite.SQLiteConnection(@"../../Data/" + dbname);
//            }
//        }

//        public bool Delete(T item)
//        {
//            _connection.Delete(item);
//            return true;
//        }

//        //public IEnumerable<T> FindAll()
//        //{
//        //    return _connection.Table<T>().ToList();
//        //}

//        //public T Find(T item)
//        //{
//        //}
//        //public T FindById(R item)
//        //{
//        //
//        //}
//        public bool Insert(T item)
//        {
//            _connection.Insert(item);
//            return true;
//        }

//        public int InsertBulk(IList<T> items)
//        {
//            return _connection.Insert(items);
//        }

//        public bool Update(T item)
//        {
//            _connection.Update(item);
//            return true;
//        }

//        public void Dispose()
//        {
//            _connection.Dispose();
//        }

//        public IEnumerable<T> SelectAll()
//        {
//            return _connection.Table<T>().ToList();
//        }

//        public T Select(T item)
//        {
//            return _connection.Table<T>().SingleOrDefault(_ => _.Equals(item));
//        }

//        public T SelectById(R id)
//        {
//            return _connection.Table<T>().SingleOrDefault(_ => getId(_).Equals(id));
//        }

//        public int InsertBulk(IEnumerable<T> item)
//        {
//            throw new NotImplementedException();
//        }

//        public int UpdateBulk(IEnumerable<T> item)
//        {
//            throw new NotImplementedException();
//        }

//        public int DeleteBulk(IEnumerable<T> item)
//        {
//            throw new NotImplementedException();
//        }

//        public bool DeleteById(R id)
//        {
//            return _connection.Delete(_connection.Table<T>().SingleOrDefault(_ => getId(_).Equals(id))) >0;
//        }

//        //public ICollection<T> FromDb<T>(string name) where T: IChildRow, new()
//        //{
//        //    return UtilityDAL.SqliteEx.FromDb<T>();

//        //}

//        //public bool ToDb<T>(ICollection<T> lst, string name) where T : IChildRow, new()
//        //{
//        //    return UtilityDAL.SqliteEx.ToDb<T>(lst);
//        //}

//        //public List<string> SelectIds()
//        //{
//        //    return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
//        //}

//    }

//    //public class Sqlite2<T> : UtilityInterface.Generic.Database.IDatabaseService<T> where T : new()
//    //{
//    //    //static readonly string _dbName;
//    //    private static SQLite.SQLiteConnection _connection;

//    //    static readonly string providerName = "SQLite";

//    //    static Sqlite2()
//    //    {
//    //        System.IO.Directory.CreateDirectory("Data");
//    //        var dbName = DbEx.GetConnectionString(providerName, false);
//    //        _connection = new SQLite.SQLiteConnection(dbName == string.Empty ? @"Data/" + typeof(T).Name + "." + Constants.SqliteDbExtension : dbName);
//    //        _connection.CreateTable<T>();
//    //    }

//    //    public Sqlite2( string dbname = null)
//    //    {
//    //        if (dbname != null)
//    //        {
//    //            _connection = new SQLite.SQLiteConnection(@"Data/" + dbname);
//    //        }
//    //    }

//    //    public bool Delete(T item)
//    //    {
//    //        _connection.Delete(item);
//    //        return true;
//    //    }

//    //    public IEnumerable<T> FindAll()
//    //    {
//    //        return _connection.Table<T>().ToList();
//    //    }

//    //    public T Find(T item)
//    //    {
//    //        return _connection.Table<T>().SingleOrDefault(_ => _.Equals(item));
//    //    }

//    //    public bool Insert(T item)
//    //    {
//    //        _connection.Insert(item);
//    //        return true;
//    //    }

//    //    public int InsertBulk(IList<T> items)
//    //    {
//    //        return _connection.Insert(items);
//    //    }

//    //    public bool Update(T item)
//    //    {
//    //        _connection.Update(item);
//    //        return true;
//    //    }

//    //    public void Dispose()
//    //    {
//    //        _connection.Dispose();
//    //    }

//    //    //public ICollection<T> FromDb<T>(string name) where T: IChildRow, new()
//    //    //{
//    //    //    return UtilityDAL.SqliteEx.FromDb<T>();

//    //    //}

//    //    //public bool ToDb<T>(ICollection<T> lst, string name) where T : IChildRow, new()
//    //    //{
//    //    //    return UtilityDAL.SqliteEx.ToDb<T>(lst);
//    //    //}

//    //    //public List<string> SelectIds()
//    //    //{
//    //    //    return System.IO.Directory.GetFiles(dbName).Select(_ => System.IO.Path.GetFileNameWithoutExtension(_)).ToList();
//    //    //}

//    //}
//}