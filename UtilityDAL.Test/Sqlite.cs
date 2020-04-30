using RandomDataGenerator.Randomizers;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityDAL.Sqlite;
using Xunit;

namespace UtilityDAL.Test
{
    public class SqliteTests
    {
        public SqliteTests()
        {
            System.IO.Directory.CreateDirectory("../../../Data");
            SQLitePCL.Batteries.Init();
        }

        [Fact]
        public void Test1()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                conn.DeleteAll<TestClass>();
                conn.InsertAll(Factory.SelectByDate(100));
                var result = conn.WhereEqual<TestClass>(DayOfWeek.Monday);
                foreach (var r in result)
                    Assert.True(r.Date.DayOfWeek == DayOfWeek.Monday);
            }
        }

        [Fact]
        public void Test2()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                var t = conn.Table<TestClass>();
                try
                {
                    conn.DeleteAll<TestClass>();
                }
                catch (Exception ex)
                {
                }
                var ticks = Factory.SelectByDate(100).ToArray();

                conn.InsertAll(ticks);

                var xx = ticks.Where(td => td.Date > default(DateTime).AddDays(1 * 100) && td.Date < default(DateTime).AddDays(4 * 100));

                var result = conn.WhereBetween<TestClass>(default(DateTime).AddDays(1 * 100), default(DateTime).AddDays(4 * 100));

                Assert.True(result.Count() == 3);
            }
        }

        [Fact]
        public void Test_WhereCompare()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                var t = conn.Table<TestClass>();
                try
                {
                    conn.DeleteAll<TestClass>();
                }
                catch (Exception ex)
                {
                }
                var ticks = Factory.SelectByDate(100).ToArray();

                conn.InsertAll(ticks);

                var xx = ticks.Where(td => td.Date > default(DateTime).AddDays(10 * 100)).ToArray();

                var result = conn.WhereCompare<TestClass>(default(DateTime).AddDays(10 * 100), ">").ToArray();

                Assert.True(result.Length == xx.Length);
            }
        }


        [Fact]
        public void Test_Hour()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                var t = conn.Table<TestClass>();
                try
                {
                    conn.DeleteAll<TestClass>();
                }
                catch (Exception ex)
                {
                }
                var ticks = Factory.SelectByHours(2).ToArray();

                conn.InsertAll(ticks);

                var xx = ticks.GroupBy(a => a.Date.Hour);

                var result = conn.WhereHourCompare<TestClass>(11, "=");

                Assert.True(result.Count() == xx.Single(a => a.Key == 11).Count());
            }

        }

        [Fact]
        public void Test3()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                conn.DeleteAll<TestClass>();
                conn.InsertAll(Factory.SelectByDate(100));
                var result = conn.Take<TestClass>(100);

                Assert.True(result.Count() == 100);
            }
        }


        [Fact]
        public void Test_ToDataSet()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                conn.DeleteAll<TestClass>();
                var testdata = Factory.SelectByDate(100).ToArray();
                conn.InsertAll(testdata);
                var result = conn.ToDataSet("Select * from TestClass", true).Skip(1).Select(TestClass.Parse).ToArray();
                Assert.True(result.SequenceEqual(testdata));
            }
        }

        [Fact]
        public void TestRemoveDuplicates()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                conn.DeleteAll<TestClass>();
                conn.InsertAll(Factory.SelectByDate(100));
                conn.InsertAll(Factory.SelectByDate(100));
                var result = conn.Take<TestClass>(200);
                Assert.True(result.Count() == 200);
                conn.RemoveDuplicates<TestClass>();
                Assert.True(conn.Table<TestClass>().Count() == 100);
            }
        }


        [Fact]
        public void Test_KeyValueLite()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            var keyvalue = new KeyValueLite(System.IO.Path.Combine(directory.FullName));
            var ty = keyvalue.FindString("sdfd");
            keyvalue.Insert(new KeyValuePair<string, long>("sdfd", 132423423));
            Assert.True(keyvalue.FindNumeric("sdfd") != null);


        }

        [Fact]
        public void Test_MaxId()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<TestClass>();
                conn.DeleteAll<TestClass>();
                conn.InsertAll(Factory.SelectByDate(100));
                var result = conn.GetMaxId<TestClass>(a => a.Id);
             
                Assert.True(result == 99);
            }
        }


        private class TestClass : IEquatable<TestClass>
        {

            public TestClass(int v1, long v2)
            {
                this.Id = v1;
                this.Date = new DateTime(v2);
            }
            public TestClass()
            {
            }

            public int Id { get; set; }

            public DateTime Date { get; set; }

            public bool Equals(TestClass other)
            {
                return this.Id == other.Id && this.Date == other.Date;
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj as TestClass);
            }

            public override int GetHashCode()
            {
                return Id;
            }

            public static TestClass Parse(object?[] data)
            {
                return new TestClass((int)data[0], (long)data[1]);
            }
        }

        private class Factory
        {
            public static IEnumerable<TestClass> SelectByDate(int number) => Enumerable.Range(0, number)
                .Select((_, i) => new TestClass { Id = i, Date = default(DateTime).AddDays(i * 100) });

            public static IEnumerable<TestClass> SelectByHours(int number)
            {
                var fac = RandomizerFactory.GetRandomizer(new RandomDataGenerator.FieldOptions.FieldOptionsDateTime());
                Dictionary<int, List<DateTime>> dictionary = Enumerable.Range(0, 24).ToDictionary(r => r, r => new List<DateTime>());
                int i = 0;
                while (dictionary.Values.Any(a => a.Count < number))
                {
                    var date = fac.Generate();

                    dictionary[date.Value.Hour].Add(date.Value);

                    yield return new TestClass { Id = ++i, Date = date.Value };
                }
            }
        }
    }
}