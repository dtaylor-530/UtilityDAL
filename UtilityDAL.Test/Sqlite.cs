using RandomDataGenerator.Randomizers;
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
                conn.InsertAll(Factory.SelectByTicks(100));
                var result = conn.WhereEqual<xx>(DayOfWeek.Monday);
                foreach (var r in result)
                    Assert.True(r.Ticks.DayOfWeek == DayOfWeek.Monday);
            }
        }

        [Fact]
        public void Test2()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
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
                var ticks = Factory.SelectByTicks(100).ToArray();

                conn.InsertAll(ticks);

                var xx = ticks.Where(td => td.Ticks > default(DateTime).AddDays(1 * 100) && td.Ticks < default(DateTime).AddDays(4 * 100));

                var result = conn.WhereBetween<xx>(default(DateTime).AddDays(1 * 100), default(DateTime).AddDays(4 * 100));

                Assert.True(result.Count() == 3);
            }
        }


        [Fact]
        public void Test_Hour()
        {
            var directory = System.IO.Directory.CreateDirectory("../../../Data");
            using (var conn = new SQLiteConnection(System.IO.Path.Combine(directory.FullName, "Test2.sqlite")))
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
                var ticks = Factory.SelectByHours(2).ToArray();

                conn.InsertAll(ticks);

                var xx = ticks.GroupBy(a => a.Ticks.Hour);

                var result = conn.WhereHourCompare<xx>(11, "=");

                Assert.True(result.Count() ==xx.Single(a=>a.Key==11).Count());
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
                conn.InsertAll(Factory.SelectByTicks(100));
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
                conn.InsertAll(Factory.SelectByTicks(100));
                conn.InsertAll(Factory.SelectByTicks(100));
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
            public static IEnumerable<xx> SelectByTicks(int number) => Enumerable.Range(0, number).Select((_, i) => new xx { Ticks = default(DateTime).AddDays(i * 100) });

            public static IEnumerable<xx> SelectByHours(int number)
            {
                var fac = RandomizerFactory.GetRandomizer(new RandomDataGenerator.FieldOptions.FieldOptionsDateTime());
                Dictionary<int, List<DateTime>> dictionary = Enumerable.Range(0, 24).ToDictionary(r => r, r => new List<DateTime>());
                int i = 0;
                while (dictionary.Values.Any(a => a.Count < number))
                {
                    var date = fac.Generate();
                    

                    dictionary[date.Value.Hour].Add(date.Value);

                    yield return new xx { Id = ++i, Ticks = date.Value };
                }
            }
        }
    }
}