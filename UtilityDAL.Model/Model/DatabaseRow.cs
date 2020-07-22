using SQLite;
using System;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Model
{
    public class DatabaseRow : IId
    {
        private long id;
        //long parentId;

        public DatabaseRow(Int64 id)
        {
            Id = id;
        }

        public DatabaseRow()
        {
        }

        [PrimaryKey, AutoIncrement]
        public Int64 Id { get => id; set => id = value; }
    }
}