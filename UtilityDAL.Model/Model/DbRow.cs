using SQLite;
using System;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Model
{
    public class DbRow : IId
    {
        private long id;
        //long parentId;

        public DbRow(Int64 id)
        {
            Id = id;
        }

        public DbRow()
        {
        }

        [PrimaryKey, AutoIncrement]
        public Int64 Id { get => id; set => id = value; }
    }
}