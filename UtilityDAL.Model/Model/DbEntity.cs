using SQLite;
using System;
using System.Collections.Generic;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Model
{
    public abstract class DbEntity :  IDbEntity
    {
        protected DbEntity()
        {
        }

        protected DbEntity(Guid guid)
        {
            Guid = guid;
        }

        protected DbEntity(long id, Guid guid) : this(guid)
        {
            Id = id;
        }

        [PrimaryKey]
        public long Id { get; set; }

        [Unique]
        public Guid Guid { get; set; }

        public DateTime AddedTime { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as DbEntity);
        }

        public bool Equals(IDbEntity other)
        {
            return other != null &&
                   Guid.Equals((other as IGuid).Guid);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public static bool operator ==(DbEntity left, DbEntity right)
        {
            return EqualityComparer<DbEntity>.Default.Equals(left, right);
        }

        public static bool operator !=(DbEntity left, DbEntity right)
        {
            return !(left == right);
        }
    }
}

