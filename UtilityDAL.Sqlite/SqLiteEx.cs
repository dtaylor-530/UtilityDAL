
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL.Model;
using UtilityDAL.Contract;
using UtilityInterface.NonGeneric.Database;
using UtilityInterface.Generic.Database;
using UtilityDAL.Common;

namespace UtilityDAL
{
    public static class SqliteEx
    {
        public static string GetName(this Type type) => ((SQLite.TableAttribute[])type.GetCustomAttributes(typeof(SQLite.TableAttribute), false)).FirstOrDefault()?.Name ?? type.Name;


        private static string _dbName;

        static SqliteEx()
        {
            string dbName = "";
            _dbName = dbName;

        }

        public static SQLite.SQLiteConnection MakeConnection() => new SQLite.SQLiteConnection(_dbName);


        public static void Create<T>(string path)
        {
            using (SQLiteConnection db = new SQLiteConnection(path))
            {
                var types = UtilityHelper.TypeHelper.GetTypesByAssembly<T>();

                foreach (var type in types)
                    db.CreateTable(type, CreateFlags.AutoIncPK);
            }
        }



        public static void Create(string path, params Type[] types)
        {
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(path));
            using (SQLiteConnection db = new SQLiteConnection(path))
            {
                foreach (var type in types)
                {
                    var types2 = UtilityHelper.TypeHelper.GetTypesByAssembly(type);

                    foreach (var type2 in types2)
                        db.CreateTable(type2, CreateFlags.AutoIncPK);
                }
            }
        }




        public static IList<T> FromDB<T>() where T : IEquatable<T>, IChildRow, new() => MakeConnection().Table<T>().ToList();


        //public static async Task<IObservable<T>> FromDbAsync<T>(this SQLiteAsyncConnection db, long? id = null) where T : IChildRow, new()
        //{
        //    Func<T, bool> f = (a) => { if (id != null) if (a.ParentId == id) return true; return false; };

        //    return await DbEx.FromDbAsync(
        //      db.Table<T>().Where(__ => f(__)).ToListAsync());
        //}

        public static List<T> FromDb<T>(this SQLiteConnection db, long? id = null) where T : IChildRow, new() =>
            id == null ?
                    db.Table<T>().ToList() :
                    db.Table<T>().ToList().Where(_ => _.ParentId == id).ToList();



        public static long? FindId<T>(this T hash, IEnumerable<T> ts) where T : IEquatable<T>, IId, new()
        {
            var seasons = from s in ts
                          where s.Equals(hash)
                          select s;

            if (seasons.Count() == 1)
                return seasons.First().Id;
            else if (seasons.Count() == 0)
                return null;
            else
                throw new Exception("duplicate values");

        }



        public static long? FindId<T>(this T hash, SQLiteConnection db) where T : IEquatable<T>, IId, new()
        {
            return FindId(hash, db.Table<T>().ToList());
        }

        public static async Task<long?> FindId<T>(this T hash, SQLiteAsyncConnection db) where T : IEquatable<T>,IId, new()
        {

            var seasons = from s in await db.Table<T>().ToListAsync()
                          where s.Equals(hash)
                          select s;

            if (seasons.Count() == 1)
                return seasons.First().Id;
            else if (seasons.Count() == 0)
                return null;
            else
                throw new Exception("duplicate values");
            //}
            //return null;
        }

        public static long? FindId<T, R>(this T hash, List<T> t) where T : IEquatable<T>, IChildRow<DbRow>, new() where R : IId
        {

            var seasons = from s in t
                          where ((IChildRow<R>)s).Equals(hash)
                          select s;
            if (seasons.Count() == 1)
                return seasons.First().Id;
            else if (seasons.Count() == 0)
                return null;
            else
                throw new Exception("duplicate values");
            //}
            //return null;

        }

        public static async Task<long?> FindId<T, R>(this T hash, SQLiteAsyncConnection db) where T : IEquatable<T>, IChildRow, new() where R : IId
        {
            return FindId(hash, await db.Table<T>().ToListAsync());
        }


        public static long? FindId<T, R>(this T hash, SQLiteConnection db) where T : IEquatable<T>, IChildRow, new() where R : IId
        {

            var t = db.Table<T>();
            var seasons = from s in t
                          where ((IChildRow<R>)s).Equals(hash)
                          select s;
            if (seasons.Count() == 1)
                return seasons.First().Id;
            else if (seasons.Count() == 0)
                return null;
            else
                throw new Exception("duplicate values");


        }

        internal static long GetMaxId<T>(SQLiteConnection db) where T : IChildRow, new()
        {
            long maxid = 0;
            try
            {
                maxid = db.Table<T>().Max(_ => _.Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return maxid;
        }




        public static DateTime GetLastDate<T>(string path = null) where T : ITimeValue, new() => new DateTime(MakeConnection().Table<T>().Max(x => x.Time));








        public static long? FindId<T>(this T hash, List<T> ts, SQLiteAsyncConnection db) where T : IEquatable<T>, IId, new()
        {
            var seasons = from s in ts
                          where s.Equals(hash)
                          select s;

            if (seasons.Count() == 1)
                return seasons.First().Id;
            else if (seasons.Count() == 0)
                return null;
            else
                throw new Exception("duplicate values");

        }







        public static bool ToDB<T>(T match, List<T> lst, SQLiteConnection db) where T : IEquatable<T>, IId, new()

                  => (match == null) ? false : ToDB(FindId(match, lst), match, db, lst);



        //public static bool ToDB<T>(IEnumerable<T> matches, List<T> lst, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IId, new()

        //    =>  ToDB(matches, db, lst);


        public static async Task<bool> ToDB<T>(T match, List<T> lst, SQLiteAsyncConnection db) where T : IEquatable<T>, IId, new()

               => (match == null) ? false : await ToDB(FindId(match, lst), match, db);

        public static bool ToDB<T, R>(T match, SQLiteConnection db) where T : IEquatable<T>, IChildRow, new() where R : IId

            => (match == null) ? false : ToDB(FindId<T, R>(match, db), match, db);


        public static async Task<bool> ToDB<T, R>(T match, SQLiteAsyncConnection db) where T : IEquatable<T>, IChildRow, new() where R : IId
            => (match == null) ? false : await ToDB(await FindId<T, R>(match, db), match, db);


        public static bool ToDB<T, R>(T match, List<T> xx, SQLiteConnection db) where T : IEquatable<T>, IChildRow<DbRow>, new() where R : IId

            => (match == null) ? false : ToDB(FindId<T, R>(match, xx), match, db, xx);

        public static async Task<bool> ToDB<T, R>(T match, List<T> xx, SQLiteAsyncConnection db) where T : IEquatable<T>, IChildRow<DbRow>, new() where R : IId

            => (match == null) ? false : await ToDB(FindId<T, R>(match, xx), match, db);


        public static bool ToDB<T>(long? id, T match, SQLiteConnection db, List<T> xx) where T : IEquatable<T>, IId
        {
            if (id == null)
            {
                db.Insert(match);
                xx.Add(match);
            }
            else
            {
                match.Id = (long)id;
                db.Update(match);
            }

            return true;
        }


        public static bool ToDB<T>(IEnumerable<T> matches,ref List<T> xx, SQLiteConnection db, bool check = true) where T : IEquatable<T>, IId, new()
        {
            bool success = true;
            IEnumerable<T> xxxx = null;
            UpdateIds<T>(ref xxxx, matches, xx);

            if (check)
            {
                success = db.UpdateAll(xxxx, true) > 0;
            }

            //var insert = matches.Except(xx);
    
            success = success && db.InsertAll(matches, true) > 0;

    
            xx.AddRange(matches);
          

            return success;

        }


        public static void UpdateIds<T>(ref IEnumerable<T> xxxx,IEnumerable<T> a, IEnumerable<T> b) where T:IId
        {
             xxxx = b.Union(a).ToList();
            using (var dg = xxxx.GetEnumerator())
            using (var dv = a.Union(b).GetEnumerator())
            {
                while (dg.MoveNext() && dv.MoveNext())
                    dv.Current.Id = dg.Current.Id;
            }

        }
        public static async Task<bool> ToDB<T>(long? id, T match, SQLiteAsyncConnection db) where T : IEquatable<T>, IId
        {
            if (id == null)
                await db.InsertAsync(match);
            else
            {
                match.Id = (long)id;
                await db.UpdateAsync(match);
            }
            return true;
        }


        public static bool ToDB<T>(long? id, T match, SQLiteConnection db) where T : IEquatable<T>, IId
        {
            if (id == null)
                db.Insert(match);
            else
            {
                match.Id = (long)id;
                db.Update(match);
            }
            return true;
        }




        //public static bool RemoveFromDB<T>(IList<T> matchestoremove, string path, string name = "default.db") where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        //{
        //    SQLiteConnection db = MakeConnection();


        //    var y = db.GetTableInfo(typeof(T).Name);

        //    if (y.Count == 0)
        //    {
        //    }

        //    foreach (T match in matchestoremove)
        //    {
        //        RemoveFromDB(match, db);
        //    }

        //    db.Dispose();

        //    return true;
        //}



        //private static bool RemoveFromDB<T>(T match, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        //{
        //    if (match == null) return false;

        //    var x = from s in db.Table<T>()
        //            where s.Equals(match)
        //            select s;

        //    return true;
        //}




        //public static bool ToDb<T>(this SQLiteConnection db, IEnumerable<T> Trades) where T : IChildRow, new()
        //{
        //    try
        //    {
        //        //var db = MakeConnection();
        //        int success = 0;
        //        foreach (T Trade in Trades)
        //        {
        //            try
        //            {
        //                // The item does not exists in the database so lets insert it
        //                var rowsAffected = db.Insert(Trade);
        //                success += Convert.ToInt32(rowsAffected > 0);
        //            }
        //            catch
        //            {

        //            }
        //        }
        //        if (success == Trades.Count()) return true; else return false;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public static async Task<bool> ToDbAsync<T>(this SQLiteAsyncConnection db, T trade) where T : IChildRow, new()
        //{
        //    try
        //    {
        //        try
        //        {
        //            // The item does not exists in the database so lets insert it
        //            await db.Table<T>().CountAsync().ContinueWith(async _ =>
        //            {
        //                trade.Id = _.Result;
        //                var rowsAffected = await db.InsertAsync(trade);
        //            });
        //        }
        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message);
        //            return false;
        //        }

        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //        return false;
        //    }

        //}


    }

}


