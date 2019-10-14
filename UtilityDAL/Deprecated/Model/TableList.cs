//using System;
//using System.Collections.Generic;
//using System.Data.SQLite;
//using System.Text;

//namespace UtilityDAL.Model
//{
//    public class TableList<T> where T : IEquatable<T>, new()
//    {
//        List<T> list;

//        public TableList(SQLite.SQLiteConnection _conn)
//        {
//            list= _conn.Table<T>().ToList();

//        }

//        public List<T> GetList() => list;

//        public void SetList(List<T> list) => this.list=list;
//    }
//}