
using SQLite;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Threading.Tasks;
using System.Text;
using System.Threading.Tasks;

using UtilityInterface.Database;

namespace UtilityDAL
{ 
    public static class SqliteEx
    {

        //static readonly string dbName;
        //static readonly string providerName = "SQLite";

        //static Sqlite()
        //{
        //    dbName = Helper.GetConnectionString(providerName, true);

        //}


        public static bool ToDb<T>(this SQLiteConnection db, IEnumerable<T> Trades) where T : IChildRow, new()
        {
            try
            {
                //var db = MakeConnection();
                int success = 0;
                foreach (T Trade in Trades)
                {
                    try
                    {
                        //int rowsAffected = await db.UpdateAsync(Trade);
                        //if (rowsAffected == 0)
                        //{
                        // The item does not exists in the database so lets insert it
                        var rowsAffected = db.Insert(Trade);

                        //}
                        success += Convert.ToInt32(rowsAffected > 0);
                    }
                    catch
                    {

                    }


                }
                if (success == Trades.Count()) return true; else return false;
            }
            catch
            {
                return false;
            }

        }
        public static async Task<bool> ToDbAsync<T>(this SQLiteAsyncConnection db, T trade) where T : IChildRow, new()
        {
            try
            {

                //T tradeSQL = Map3.ToTradeSQL(trade);

                //var db = MakeAsyncConnection();

                try
                {
                    //int rowsAffected = await db.UpdateAsync(tradeSQL);
                    //if (rowsAffected == 0)
                    //{
                    // The item does not exists in the database so lets insert it
                    await db.Table<T>().CountAsync().ContinueWith(async _ =>
                    {
                        trade.Id = _.Result;
                        var rowsAffected = await db.InsertAsync(trade);
                    });



                    //}

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }

        }




        //public static SQLite.SQLiteConnection MakeConnection()
        //{

        //    return new SQLite.SQLiteConnection(dbName);


        //}

        //public static SQLite.SQLiteAsyncConnection MakeAsyncConnection()
        //{
        //    return new SQLite.SQLiteAsyncConnection(dbName);


        //}
        //public static bool MatchTradeByContractId(long id)
        //{
        //    var db = MakeConnection();

        //    var x = db.Table<Trade>().Any(_ => _.ContractId == id);

        //    db.Dispose();

        //    return x;

        //}


        public static async Task<IObservable<T>> FromDbAsync<T>(this SQLiteAsyncConnection db, long? id = null) where T : IChildRow, new()
        {
            //var db = MakeAsyncConnection();

            Func<T, bool> f = (a) => { if (id != null) if (a.ParentId == id) return true; return false; };

           return await DbEx.FromDbAsync(
             db.Table<T>().Where(__ => f(__)).ToListAsync());


        }


        public static List<T> FromDb<T>(this SQLiteConnection db, long? id = null) where T : IChildRow, new()
        {
  

                return id == null ?
                    db.Table<T>().ToList() :
                    db.Table<T>().ToList().Where(_ => _.ParentId == id).ToList();
          
        }


        private static string _dbName;



        public static void Create<T>(string path)
        {
            using (SQLiteConnection db = new SQLiteConnection(path))
            {
                var types = UtilityDAL.Reflection.GetTypesByAssembly<T>();

                foreach (var type in types)
                    db.CreateTable(type, CreateFlags.AutoIncPK);
            }
        }




        public static void Create(string path, params Type[] types)
        {
            using (SQLiteConnection db = new SQLiteConnection(path))
            {
                foreach (var type in types)
                {
                    var types2 = UtilityDAL.Reflection.GetTypesByAssembly(type);

                    foreach (var type2 in types2)
                        db.CreateTable(type2, CreateFlags.AutoIncPK);
                }
            }
        }

        //public static void CreateTables(string path, params Type[] types)
        //{
        //    using (SQLiteConnection db = new SQLiteConnection(path))
        //    {
        //        foreach (var type in types)
        //        {
        //            db.CreateTable(type, CreateFlags.AutoIncPK);
        //        }
        //    }
        //}



        static SqliteEx()
        {
            string dbName = "";
            _dbName = dbName;

        }

        //public static long? FindId<T>(this T hash) where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        //{

        //    var db = MakeConnection();
        //    var x = db.ExecuteScalar<T>("Select * From " + typeof(T).Name);

        //    if (x != null)
        //    {
        //        var seasons = from s in db.Table<T>()
        //                      where s.Equals(hash)
        //                      select s;

        //        return seasons.SingleOrDefault()?.Id;
        //    }
        //    return null;

        //}
        public static long? FindId<T>(this T hash, List<T> ts, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IId, new()
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



        public static long? FindId<T>(this T hash, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IId, new()
        {
            //try
            //{
            //    var xx=db.Table<T>().ToList().SingleOrDefault(_ => _.Equals(hash))?.Id;
            //    return xx;
            //}
            //catch (Exception e)
            //{

            //}

            //var x = db.ExecuteScalar<T>("Select * From " + typeof(T).Name);

            //if (x != null)
            //{
            //var seasons = from s in db.Table<T>().ToList()
            //              where s.Equals(hash)
            //              select s;


            var seasons = from s in db.Table<T>().ToList()
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

        public static long? FindId<T, R>(this T hash, List<T> t, SQLiteConnection db) where T : IEquatable<T>, IChildRow<DbRow>, new() where R : IId
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

        //public static long? FindId<T, R>(this T hash, List<T> t, SQLiteConnection db) where T : DbChildRow,IEquatable<T>, new() where R : IId
        //{

        //    var seasons = from s in t
        //                  where ((IChildRow<R>)s).Equals(hash)
        //                  select s;
        //    if (seasons.Count() == 1)
        //        return seasons.First().Id;
        //    else if (seasons.Count() == 0)
        //        return null;
        //    else
        //        throw new Exception("duplicate values");
        //    //}
        //    //return null;

        //}

        public static long? FindId<T, R>(this T hash, SQLiteConnection db) where T : IEquatable<T>, IChildRow<R>, new() where R : IId
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

        //public static long? FindId<T>(this T hash, SQLiteConnection db) where T : DbChildRow, IEquatable<T>,  new() 
        //{

        //    var t = db.Table<T>();
        //    var seasons = from s in t
        //                  where ((DbChildRow)s).Equals(hash)
        //                  select s;
        //    if (seasons.Count() == 1)
        //        return seasons.First().Id;
        //    else if (seasons.Count() == 0)
        //        return null;
        //    else
        //        throw new Exception("duplicate values");


        //}

        internal static long GetMaxId<T>(SQLiteConnection db) where T : UtilityInterface.Database.IChildRow, new()
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


        public static SQLite.SQLiteConnection MakeConnection()
        {
            return new SQLite.SQLiteConnection(_dbName);


        }

        public static DateTime GetLastDate<T>(string path = null) where T : IDbTimeValue, new()
        {
            return new DateTime(MakeConnection().Table<T>().Max(x => x.Time));

        }


        //public static bool TransferToDB<T>(IEnumerable<T> matches)
        //{

        //    SQLiteConnection db = MakeConnection();
        //    //Task.Run(() =>
        //    //{

        //    int updated = 0;
        //    //int id = 0;
        //    int insertedMatches = 0;
        //    int insertedOdds = 0;
        //    foreach (Match match in matches)
        //    {
        //        TransferToDB(match, db, ref insertedMatches, ref updated, ref insertedOdds);
        //    }
        //    db.Dispose();
        //    MessageBox.Show($"Export to Db Complete! Matches Inserted ={insertedMatches}, Odds Inserted ={insertedOdds}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);


        //    return true;


        //}

        public static bool RemoveFromDB<T>(IList<T> matchestoremove, string path, string name = "default.db") where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        {
            SQLiteConnection db = MakeConnection();


            var y = db.GetTableInfo(typeof(T).Name);


            if (y.Count == 0)
            {
                //var sqldb = new WebScrape.Model.SQLiteDb(System.IO.Path.Combine(path, name));
                //sqldb.Create();

            }

            foreach (T match in matchestoremove)
            {
                RemoveFromDB(match, db);
            }

            db.Dispose();
            //MessageBox.Show($"Export to Db Complete! Matches Inserted ={insertedMatches}, Odds Inserted ={insertedOdds}", "Information", MessageBoxButton.OK, MessageBoxImage.Information);


            return true;
        }



        private static bool RemoveFromDB<T>(T match, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        {


            if (match == null) return false;

            var x = from s in db.Table<T>()
                    where s.Equals(match)
                    select s;

            //var dt = DateTime.FromBinary(match.StartTime);
            //var startofDay = dt.Date.Ticks;
            //var endofDay = dt.AddTicks(-1).AddDays(1).Ticks;


            //var y = x.Where(_ => _.StartTime > startofDay & _.StartTime < endofDay);


            //if (y.Count() == 0)
            //    return false;
            //else if (y.Count() == 1)
            //{
            //    var m = y.First();
            //    var g = db.Table<Contract>().Where(_ => _.MatchId == m.Id);
            //    var ids = g.Select(_ => (object)_.Id);
            //    db.DeleteAllIds<Contract>(ids);
            //    db.Delete(m, false);



            //}
            //else if (y.Count() > 1)
            //    throw new Exception($"Duplicate record {match.ToString()} in database");

            return true;

        }


        public static IList<T> FromDB<T>() where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        {
            return MakeConnection().Table<T>().ToList();
        }

        //public static bool ToDB<T>(T match) where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        //{

        //    if (match == null) return false;
        //    var db = MakeConnection();
        //    if (FindId<T>(match) == null)
        //    {
        //        db.Insert(match);
        //        db.InsertWithChildren(match, true);
        //    }
        //    else
        //    {

        //        db.UpdateWithChildren(match);
        //    }


        //if (ImplementsInterface(typeof(T), typeof(IParent<>)).Length>0)
        //{

        //    foreach (var child in ((IParent<T>)match).Children.C)
        //    {



        //    }

        //}


        //}
        //Console.WriteLine("data insert: " + match.HomeTeam + " vs " + match.AwayTeam + " @ " + match.StartTime);


        //int rowsAffected = 0;
        //var df = (from p in db.Table<Match>() where (p.HomeTeam == match.HomeTeam) && (p.StartTime == match.StartTime) select p);

        //if (df.Count() < 1)
        //{
        //    // The item does not exists in the database so lets insert it
        //    rowsAffected = db.Insert(match);
        //    insertedMatches++;
        //    id = (int)match.Id;
        //}
        //else if (df.Count() > 1)
        //{
        //    throw new Exception("replica of item in table");
        //}
        //else
        //{
        //    id = (int)df.First().Id;
        //}
        //if (match.Odds != null)
        //{
        //    foreach (var o in match.Odds)
        //    {
        //        o.MatchId = id;
        //        db.Insert(o);
        //        int aff = Convert.ToInt16(rowsAffected == 1);
        //        updated += aff;
        //        insertedOdds++;
        //    }
        //}

        //if (match.Teams != null)
        //{

        //    foreach (var team in match.Teams)
        //    {
        //        team.MatchId = id;
        //        db.Insert(team);
        //        int aff = Convert.ToInt16(rowsAffected == 1);
        //        updated += aff;
        //        insertedTeams++;

        //        if (team.Players!=null)
        //            foreach(var player in team.Players)
        //                if(player.Stats!=null)
        //                foreach(var stat in player.Stats)
        //                    {
        //                        o.MatchId = id;
        //                        db.Insert(o);
        //                        int aff = Convert.ToInt16(rowsAffected == 1);
        //                        updated += aff;
        //                        insertedOdds++;


        //                    }

        //    }
        //}



        //    return true;
        //}



        public static bool ToDB<T>(ref T match, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        {

            if (match == null) return false;
            long? id = FindId<T>(match, db);
            if (id == null)
            {
                // try
                // {
                db.Insert(match);
                //    
                //}
                //catch (Exception e)
                //{
                //    //db.InsertWithChildren(match, true);
                //    //db.UpdateWithChildren(match);
                //    throw e;
                //}
            }
            else
            {
                match.Id = (long)id;
                try
                {
                    db.Update(match);
                }
                catch (Exception e)
                {

                }
            }
            return true;
        }


        public static bool ToDB<T>(ref T match, List<T> lst, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IId, new()
        {

            if (match == null) return false;
            long? id = FindId<T>(match, lst, db);
            if (id == null)
            {
                db.Insert(match);
            }
            else
            {
                match.Id = (long)id;
                try
                {
                    db.Update(match);
                }
                catch (Exception e)
                {

                }
            }
            return true;
        }



        public static bool ToDB<T, R>(ref T match, SQLiteConnection db) where T : IEquatable<T>, IChildRow<R>, new() where R : IId
        {

            if (match == null) return false;
            long? id = FindId<T, R>(match, db);
            if (id == null)
            {
                //  try
                // {
                db.Insert(match);
                // 
                //   }
                //  catch
                //  {
                //db.InsertWithChildren(match, true);
                //db.Update(match);
                //  }
            }
            else
            {
                match.Id = (long)id;
                db.Update(match);
            }
            return true;
        }


        public static bool ToDB<T, R>(ref T match, List<T> xx, SQLiteConnection db) where T : IEquatable<T>, IChildRow<DbRow>, new() where R : IId
        {

            if (match == null) return false;
            long? id = FindId<T, R>(match, xx, db);
            if (id == null)
            {
                db.Insert(match);
            }
            else
            {
                match.Id = (long)id;
                db.Update(match);
            }
            return true;
        }

        //public static bool ToDB<T, R>(ref T match, List<T> xx, SQLiteConnection db) where T : DbChildRow,IEquatable<T>,new() where R : IId
        //{

        //    if (match == null) return false;
        //    long? id = FindId<T, R>(match, xx, db);
        //    if (id == null)
        //    {
        //        db.Insert(match);
        //    }
        //    else
        //    {
        //        match.Id = (long)id;
        //        db.Update(match);
        //    }
        //    return true;
        //}
        //public static bool ToDB<T>(IList<T> matches, SQLiteConnection db) where T : IEquatable<T>, UtilityInterface.Database.IChildRow, new()
        //{
        //    foreach (var match in matches)
        //        ToDB(ref match, db);
        //    return true;
        //}
        //public static IEnumerable<Match> GetAllFixtures(string dataSource = "ESPNdb.db")
        //{

        //    SQLiteConnection db = new SQLiteConnection(dataSource);
        //    string sdate = DateTime.Now.ToString("yyyy-MM-dd");
        //    var matches = db.Query<Match>("select * from Match where Date(StartTime) > ? ", sdate);


        //    return matches;

        //}


        //public static IEnumerable<Match> GetAllResults(string dataSource = "ESPNdb.db")
        //{

        //    SQLiteConnection db = new SQLiteConnection(dataSource);
        //    string sdate = DateTime.Now.ToString("yyyy-MM-dd");
        //    var matches = db.Query<Match>("select * from Match where Date(StartTime) < ? ", sdate);



        //    return matches;

        //}


        //public static IEnumerable<Contract> GetOddsMovement(string dataSource = "ESPNdb.db")
        //{

        //    SQLiteConnection db = new SQLiteConnection(dataSource);
        //    string sdate = DateTime.Now.ToString("yyyy-MM-dd");
        //    var odds = db.Query<Contract>("SELECT * FROM Odd");
        //    // returns odd with highest number of matchids assocuated with it
        //    string statement = "SELECT * FROM Odd GROUP BY MatchId HAVING COUNT(MatchId) = (SELECT MAX(oddcnt) FROM (SELECT MatchId, COUNT(MatchId) AS oddcnt FROM Odd  GROUP BY MatchId) t1)";

        //    return odds;

        //}


        //public static string InsertCompetitions(IEnumerable<string> competitions)
        //{
        //    SQLiteConnection db = new SQLiteConnection("ESPNdb.db");

        //    var comps = competitions.Select(c => new Competition { Name = c });


        //    if (comps.Count() == db.Table<Competition>().Count())
        //        return "Competitions match number in database";

        //    foreach (var comp in comps)
        //    {
        //        var up = db.Update(comp);
        //        if (up < 1)
        //            db.Insert(comp);


        //    }

        //    //INSERT INTO #IMEIS (imei) VALUES ('val1'), ('val2')


        //    return null;

        //}
        public static Type[] ImplementsInterface(Type myType, Type intface)
        {

            // this conditional is necessary if myType can be an interface,
            // because an interface doesn't implement itself: for example,
            // typeof (IList<int>).GetInterfaces () does not contain IList<int>!
            if (myType.IsInterface && myType.IsGenericType &&
                myType.GetGenericTypeDefinition() == intface)
                return myType.GetGenericArguments();

            foreach (var i in myType.GetInterfaces())
                if (i.IsGenericType && i.GetGenericTypeDefinition() == intface)
                    return i.GetGenericArguments();

            return null;
        }
    }

    public interface IDbTimeValue
    {
        Int64 Time { get; set; }
        Int64 Value { get; set; }
    }


    //public class SqLiteData
    //{

    //    public static System.Data.SQLite.SQLiteConnection MakeConnection()
    //    {
    //        string dataSource = OddsPortalScraper.DAL.Properties.Settings.Default.ConsoleAppDbPath + "\\"
    //            + OddsPortalScraper.DAL.Properties.Settings.Default.DbName;

    //        var conn1 = new System.Data.SQLite.SQLiteConnection($"Data Source={dataSource};");

    //        return conn1;
    //    }
    //}

}


