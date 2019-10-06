using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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



    public class KeyValueLite
    {
        string directory;

        public KeyValueLite(string directory)
        {
            this.directory = directory;
            System.IO.Directory.CreateDirectory(directory);
        }
        public KeyValueLite() : this("../../Data")
        {
        }

        public int Insert(KeyValuePair<string, long>[] kvps)
        {
            int i = 0;
            using (var x = new SQLite.SQLiteConnection(directory + "/KeyValueStore.sqlite"))
            {
                x.CreateTable<KeyValueNumeric>();
                foreach (var y in kvps.Select(_ => new KeyValueNumeric { Key = _.Key, Value = _.Value }))
                    i += x.InsertOrReplace(y);
            }
            return i;
        }

        public int Insert(KeyValuePair<string, string>[] kvps)
        {
            int i = 0;
            using (var x = new SQLite.SQLiteConnection(directory + "/KeyValueStore.sqlite"))
            {
                x.CreateTable<KeyValueString>();
                foreach (var y in kvps.Select(_ => new KeyValueString { Key = _.Key, Value = _.Value }))
                    i += x.InsertOrReplace(y);
            }
            return i;
        }


        public string FindString(string key)
        {
            using (var x = new SQLite.SQLiteConnection(directory + "/KeyValueStore.sqlite"))
            {
                x.CreateTable<KeyValueString>();
                var xx = x.Find<KeyValueString>(key); ;
                return xx?.Value;
            }
        }

        public long FindNumeric(string key)
        {
            using (var x = new SQLite.SQLiteConnection(directory + "/KeyValueStore.sqlite"))
            {
                x.CreateTable<KeyValueNumeric>();
                var xx = x.Find<KeyValueNumeric>(key); ;
                return xx?.Value ?? 0;
            }

        }
    }
}
