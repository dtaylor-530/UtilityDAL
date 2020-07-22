using SQLite;
using System;

namespace UtilityDAL.Model
{
    public class DatabaseChildRow : DatabaseRow, UtilityInterface.Generic.Database.IChildRow<DatabaseRow>, IEquatable<DatabaseChildRow>
    {
        [Indexed]
        //[SQLiteNetExtensions.Attributes.ForeignKey(typeof(DbRow))]
        public Int64 ParentId
        {
            get { return Parent.Id; }
            set
            {
                Parent = Parent ?? new DatabaseRow(value);
            }
        }

        [Ignore]
        public DatabaseRow Parent
        {
            get;
            set;
        }

        public bool Equals(DatabaseChildRow y) => this.Parent == y.Parent;

        public override bool Equals(object y) => this.Equals(y as DatabaseChildRow);

        public override int GetHashCode() => (int)this.Parent.Id;
    }
}