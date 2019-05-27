using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Model
{


    public class DbChildRow : DbRow,UtilityInterface.Database.IChildRow<DbRow>,IEquatable<DbChildRow>
    {

        [Indexed]
        //[SQLiteNetExtensions.Attributes.ForeignKey(typeof(DbRow))]
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

        public bool Equals(DbChildRow y) => this.Parent == y.Parent;

        public override bool Equals(object y) =>  this.Equals(y as DbChildRow);
     
        public override int GetHashCode() => (int)this.Parent.Id;
 
    }


    //public class DbChildRow<T> : DbRow, UtilityInterface.Database.IChildRow<T> where T : DbRow
    //{

    //    [Indexed]
    //    [SQLiteNetExtensions.Attributes.ForeignKey(typeof(DbRow))]
    //    public Int64 ParentId
    //    {
    //        get { return Parent.Id; }
    //        set
    //        {
    //            Parent = Parent ?? new DbRow(value);

    //        }
    //    }

    //    [Ignore]
    //    public T Parent
    //    {
    //        get;
    //        set;
    //    }
    //}
}
