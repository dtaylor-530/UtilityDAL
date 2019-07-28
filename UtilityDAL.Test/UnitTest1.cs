using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using UtilityDAL.Sqlite;

namespace UtilityDAL.Test
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

            using (var conn = new SQLite.SQLiteConnection("Test.sqlite"))
            {
                conn.CreateTable<xx>();
                conn.DeleteAll<xx>();
                conn.InsertAll(Factory.GetTicks(100));
                var result = conn.WhereEqual<xx>(DayOfWeek.Monday);
                foreach (var r in result)
                    Assert.True(new DateTime(r.Ticks).DayOfWeek == DayOfWeek.Monday);
            }
        }


        [Fact]
        public void Test2()
        {

            using (var conn = new SQLite.SQLiteConnection("Test.sqlite"))
            {
                conn.CreateTable<xx>();
                conn.DeleteAll<xx>();
                conn.InsertAll(Factory.GetTicks(100));
                var result = conn.WhereBetween<xx>(default(DateTime).AddDays(1 * 100), default(DateTime).AddDays(4 * 100));

                Assert.True(result.Count() == 4);
            }
        }

        class xx
        {
            public long Ticks { get; set; }
        }

        class Factory
        {
            public static IEnumerable<xx> GetTicks(int number) => Enumerable.Range(0, number).Select((_, i) => new xx { Ticks = default(DateTime).AddDays(i * 100).Ticks });

        }
    }
}
