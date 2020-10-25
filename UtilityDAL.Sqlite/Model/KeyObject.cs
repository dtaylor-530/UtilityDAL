using SQLite;
using System;
using System.Linq;

namespace UtilityDAL.Sqlite
{
    internal class KeyObject : IEquatable<KeyObject>
    {
        [PrimaryKey]
        public string Key { get; set; }

        public bool Equals(KeyObject other)
        {
            return this.Key == other.Key;
        }

        public override int GetHashCode() => Key.Sum(_ => _);

        public override bool Equals(object obj)
        {
            return this.Equals(obj as KeyObject);
        }

        public override string ToString()
        {
            return this.Key.ToString();
        }
    }

    internal class KeyValueNumeric : KeyObject
    {
        public long Value { get; set; }
    }

    internal class KeyValueString : KeyObject
    {
        public string Value { get; set; }
    }

}