using System;
using System.Collections.Generic;

namespace UtilityDAL.Model
{
    public class TableList<T> where T : IEquatable<T>, new()
    {
        private List<T> list;

        public TableList(SQLite.SQLiteConnection _conn)
        {
            list = _conn.Table<T>().ToList();
        }

        public List<T> GetList() => list;

        public void SetList(List<T> list) => this.list = list;
    }
}