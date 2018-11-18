using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL
{
    public class DbRow : UtilityInterface.Database.IId
    {
        long id;
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



    public class DbChildRow : DbRow,UtilityInterface.Database.IChildRow
    {

        [Indexed]
        [SQLiteNetExtensions.Attributes.ForeignKey(typeof(DbRow))]
        public Int64 ParentId
        {
            get { return Parent.Id; }
            set
            {
                Parent = Parent ?? new DbRow(value);

            }
        }

        [Ignore]
        public DbRow Parent
        {
            get;
            set;
        }
    }
}
