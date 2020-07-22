using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UtilityDAL.Abstract;
using UtilityInterface.NonGeneric;

namespace UtilityDAL.Service
{
    public static class LitedbPeriodic
    {
        public static IList<T> ByUpToDate<T>(Func<DateTime, DateTime> periodmatch, string dbName = @"MyData.db") where T : IPeriodic
        {
            using (var db = new LiteDatabase(dbName))
            {
                var customers = db.GetCollection<T>();

                customers.EnsureIndex(x => x.DateTimes);

                DateTime dtn = periodmatch(DateTime.Now);

                return customers.Find(_ => periodmatch(_.DateTimes.Last()) == dtn).Cast<T>().ToList();
            }
        }

        public static Task Update<T>(IList<T> nested, string dbName = @"MyData.db") where T : IBSONRow, IPeriodic => Task.Run(() =>
        {
            using (var db = new LiteDatabase(dbName))
            {
                foreach (var ix in nested)
                    Update(db, ix);
            }
        });

        public static Task Update<T>(this LiteDatabase db, T nested) where T : IBSONRow, IPeriodic => Task.Run(() =>
        {
            var collection = db.GetCollection<T>();

            if (collection.Count() == 0)

                collection.Insert(nested);
            else
            {
                T c = default(T);
                if (nested.Id != 0)
                    c = collection.FindById(nested.Id);
                //else
                //{
                //    collection.EnsureIndex(xx => xx.Country);
                //    collection.EnsureIndex(xx => xx.League);
                //     c = collection.FindOne(xx => xx.Country == nested.Country && xx.League == nested.League);
                //}

                var dates = c.DateTimes.ToList();
                foreach (var d in nested.DateTimes)
                    dates.Add(d);

#warning Need to fix
                //c.DateTimes = dates;
                collection.Update(c.Id, c);
            }
        });
    }
}