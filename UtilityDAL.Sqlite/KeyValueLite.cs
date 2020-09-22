using Optional;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityDAL.Sqlite
{ 
    public class KeyValueLite
    {
        private string directory;
        private const string Name = "/KeyValueStore.sqlite";
        public KeyValueLite(string directory)
        {
            this.directory = directory;
            System.IO.Directory.CreateDirectory(directory);
        }

        public KeyValueLite() : this("../../Data")
        {
        }

        public int Insert(params KeyValuePair<string, long>[] kvps)
        {
            int i = 0;
            using (var x = new SQLite.SQLiteConnection(directory + Name))
            {
                x.CreateTable<KeyValueNumeric>();
                foreach (var y in kvps.Select(_ => new KeyValueNumeric { Key = _.Key, Value = _.Value }))
                    i += x.InsertOrReplace(y);
            }
            return i;
        }

        public int Insert(params KeyValuePair<string, string>[] kvps)
        {
            int i = 0;
            using (SQLiteConnection x = new SQLite.SQLiteConnection(directory + Name))
            {
                x.CreateTable<KeyValueString>();
                foreach (var y in kvps.Select(_ => new KeyValueString { Key = _.Key, Value = _.Value }))
                    i += x.InsertOrReplace(y);
            }
            return i;
        }
        public int Insert(params KeyValuePair<string, DateTime>[] kvps)
        {
            int i = 0;
            using (SQLiteConnection x = new SQLiteConnection(directory + Name))
            {
                x.CreateTable<KeyValueNumeric>();
                foreach (var y in kvps.Select(_ => new KeyValueNumeric { Key = _.Key, Value = _.Value.Ticks }))
                    i += x.InsertOrReplace(y);
            }
            return i;
        }

        public Optional.Option<string> FindString(string key)
        {
            using (var x = new SQLite.SQLiteConnection(directory + Name))
            {
                x.CreateTable<KeyValueString>();
                var xx = x.Find<KeyValueString>(key);
                return xx.SomeNotNull().Map(a => a.Value);
            }
        }

        public Optional.Option<long> FindNumeric(string key)
        {
            using (var x = new SQLiteConnection(directory + Name))
            {
                x.CreateTable<KeyValueNumeric>();
                var xx = x.Find<KeyValueNumeric>(key);
                return xx.SomeNotNull().Map(a => a.Value);
            }
        }

        public Optional.Option<DateTime> FindDate(string key)
        {
            using (var x = new SQLiteConnection(directory + Name))
            {
                x.CreateTable<KeyValueNumeric>();
                var xx = x.Find<KeyValueNumeric>(key);
                return xx.SomeNotNull().Map(a => new DateTime(a.Value));
            }
        }
    }
}