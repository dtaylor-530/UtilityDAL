using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityDAL.Sqlite;
using Xunit;

namespace UtilityDAL.Test
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            System.IO.Directory.CreateDirectory("../../../Data");
        }

        [Fact]
        public void Test1()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<xx>();
                conn.DeleteAll<xx>();
                conn.InsertAll(Factory.GetTicks(100));
                var result = conn.WhereEqual<xx>(DayOfWeek.Monday);
                foreach (var r in result)
                    Assert.True(r.Ticks.DayOfWeek == DayOfWeek.Monday);
            }
        }

        [Fact]
        public void Test2()
        {
            //var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection("Test2.sqlite"))
            {
                 conn.CreateTable<xx>();
                var t = conn.Table<xx>();
                try
                {
                    conn.DeleteAll<xx>();
                }
                catch (Exception ex)
                {
                }
                conn.InsertAll(Factory.GetTicks(100));
                var result = conn.WhereBetween<xx>(default(DateTime).AddDays(1 * 100), default(DateTime).AddDays(4 * 100));

                Assert.True(result.Count() == 4);
            }
        }

        [Fact]
        public void Test3()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<xx>();
                conn.DeleteAll<xx>();
                conn.InsertAll(Factory.GetTicks(100));
                var result = conn.Take<xx>(100);

                Assert.True(result.Count() == 100);
            }
        }

        [Fact]
        public void TestRemoveDuplicates()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
            {
                conn.CreateTable<xx>();
                conn.DeleteAll<xx>();
                conn.InsertAll(Factory.GetTicks(100));
                conn.InsertAll(Factory.GetTicks(100));
                var result = conn.Take<xx>(200);
                Assert.True(result.Count() == 200);
                conn.RemoveDuplicates<xx>();
                Assert.True(conn.Table<xx>().Count() == 100);
            }
        }

        private class xx : IEquatable<xx>
        {
            public int Id { get; set; }

            public DateTime Ticks { get; set; }

            public bool Equals(xx other)
            {
                return this.Id == other.Id && this.Ticks == other.Ticks;
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj as xx);
            }

            public override int GetHashCode()
            {
                return Id;
            }
        }

        private class Factory
        {
            public static IEnumerable<xx> GetTicks(int number) => Enumerable.Range(0, number).Select((_, i) => new xx { Ticks = default(DateTime).AddDays(i * 100) });
        }
    }
}