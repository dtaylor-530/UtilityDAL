//using FDataGPNet.Common;
//using FootballData.Model;
//using System;
//using System.Collections.Generic;
//using System.Data.OleDb;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;

//namespace FDataGPNet.DAL
//{
//    public static class Access
//    {
//        public static System.Data.OleDb.OleDbConnection OpenConnection(string directory = null)
//        {
//            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection();

//            conn.ConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
//                                    @"Data Source=" + directory;
//            try
//            {
//                conn.Open();
//            }
//            catch (Exception)
//            {
//                throw;
//            }
//            return conn;

//        }

//        public static IEnumerable<string> GetLeagueCodes(OleDbConnection conn)
//        {
//            var cmd = new OleDbCommand();
//            cmd.Connection = conn;//?? OpenConnection();
//            cmd.CommandText = $"SELECT competitionCode FROM Competitions";

//            var items = new List<string>();
//            try
//            {
//                using (OleDbDataReader objReader = cmd.ExecuteReader())
//                {
//                    if (objReader.HasRows)
//                    {
//                        while (objReader.Read())
//                        {
//                            string item = null;
//                            object resultObject = objReader.GetValue(objReader.GetOrdinal("competitionCode"));
//                            if (resultObject != DBNull.Value)
//                            {
//                                item = resultObject.ToString();
//                            }
//                            else { }
//                            //string item = objReader.GetString(objReader.GetOrdinal("competitionCode"));
//                            items.Add(item);
//                        }
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                if (ex.GetType() == typeof(InvalidCastException))
//                    Console.WriteLine(ex);
//            }

//            return items;

//        }

//        public static int GetId(string competitionCode, OleDbConnection conn)
//        {
//            var cmd = new OleDbCommand();
//            cmd.Connection = conn;//?? OpenConnection();
//            cmd.CommandText = $"SELECT betfairId FROM Competitions Where competitionCode=@competitionCode";

//            cmd.Parameters.AddWithValue("@competitionCode", competitionCode);

//            var id = cmd.ExecuteScalar();

//            return (int)id;
//        }

//        ////public static int GetCompetitionByField(string field, object value, OleDbConnection conn=null)
//        ////{
//        ////    var cmd = new OleDbCommand();
//        ////    cmd.Connection = conn?? OpenConnection();
//        ////    cmd.CommandText = $"SELECT betfairId FROM Competitions Where competitionCode=@competitionCode";

//        ////    cmd.Parameters.AddWithValue("@competitionCode", competitionCode);

//        ////    var id = cmd.ExecuteScalar();

//        ////    return (int)id;
//        ////}

//        public static void Upsert(this List<FootballDataMatch> matches, int div, int? season = null, System.Data.OleDb.OleDbConnection conn = null)
//        {
//            string sql = "SELECT homeTeam From Events WHERE eventDate = @eventDate AND homeTeam = @homeTeam"; //System.IO.File.ReadAllText("SelectWhereQuery.txt");
//            string sqlI = System.IO.File.ReadAllText("InsertQuery.txt");
//            sqlI = sqlI.RemoveLineEndings().ReduceWhitespace();

//            bool alternate = false;
//            //string propertyname = null;
//            PropertyInfo proph = null, propa = null, propd = null;

//            if (matches.First().BbAvH == 0)
//            {
//                matches.First().FindFirstAlternate(out proph, out propd, out propa);
//                alternate = true;

//            }
//            DateTime minDate = DateTime.MinValue;
//            if (season == null)
//            {
//                if (matches.Select(x => x.Div).Distinct().Count() < 2)
//                    minDate = matches.Min(x => x.Date);

//            }

//            //cmd1.Parameters.Add(new OleDbParameter("@competitionId", OleDbType.Integer));
//            foreach (var match in matches)
//            {
//                if (season == null)
//                {
//                    //if (match.Season == 0)
//                    //    throw new Exception("Season property missing from match");
//                    //else
//                    match.Season = HelperClass.GetYear(match.Date, minDate);

//                }
//                else
//                    match.Season = (int)season;

//                System.Diagnostics.Debug.Assert(DateTimeHelpers.IsYear(match.Season.ToString()));

//                string sqlB = $"SELECT betfairName from Teams Where FDName=@FDName";
//                OleDbCommand cmd3 = new OleDbCommand(sqlB, conn);
//                cmd3.Parameters.AddWithValue("@FDName", match.HomeTeam);
//                var cx = cmd3.ExecuteScalar();

//                System.Diagnostics.Debug.Assert(cx != null);

//                string sqlB2 = $"SELECT betfairName from Teams Where FDName=@FDName";
//                OleDbCommand cmd4 = new OleDbCommand(sqlB, conn);
//                cmd4.Parameters.AddWithValue("@FDName", match.AwayTeam);
//                var cx2 = cmd4.ExecuteScalar();

//                System.Diagnostics.Debug.Assert(cx2 != null);

//                OleDbCommand cmd1 = new OleDbCommand(sql, conn);
//                //    cmd1.Parameters.AddWithValue("@competitionId",div);
//                //    cmd1.Parameters.AddWithValue("@season", season);
//                cmd1.Parameters.AddWithValue("@eventDate", match.Date);
//                cmd1.Parameters.AddWithValue("@homeTeam", cx);

//                if (match.Equals(matches.Last()))
//                    Console.WriteLine("2");

//                var cx3 = cmd1.ExecuteScalar();

//                int clash = 0;

//                if (cx3 == null)
//                {
//                    // handled as needed, but this snippet will throw an exception to force a rollback

//                    OleDbCommand cmd2 = new OleDbCommand(sqlI, conn);

//                    cmd2.Parameters.AddWithValue("@competitionId", div);
//                    cmd2.Parameters.AddWithValue("@season", match.Season);
//                    cmd2.Parameters.AddWithValue("@eventDate", match.Date);
//                    cmd2.Parameters.AddWithValue("@homeTeam", cx);
//                    cmd2.Parameters.AddWithValue("@awayTeam", cx2);
//                    cmd2.Parameters.AddWithValue("@HFTGoals", match.FTHG);
//                    cmd2.Parameters.AddWithValue("@AFTGoals", match.FTAG);
//                    cmd2.Parameters.AddWithValue("@FTResult", match.FTR);
//                    cmd2.Parameters.AddWithValue("@status", "FT");
//                    //cmd2.Parameters.AddWithValue("@location", match.);
//                    cmd2.Parameters.AddWithValue("@stage", 1);
//                    //cmd2.Parameters.AddWithValue("@FixtureWeek", match.);
//                    cmd2.Parameters.AddWithValue("@HOddsMax", match.BbMxH);
//                    cmd2.Parameters.AddWithValue("@AOddsMax", match.BbMxA);
//                    cmd2.Parameters.AddWithValue("@DOddsMax", match.BbMxD);

//                    if (alternate)
//                    {
//                        cmd2.Parameters.AddWithValue("@HOddsAvg", proph.GetValue(match));
//                        cmd2.Parameters.AddWithValue("@AOddsAvg", propa.GetValue(match));
//                        cmd2.Parameters.AddWithValue("@DOddsAvg", propd.GetValue(match));
//                    }
//                    else
//                    {
//                        cmd2.Parameters.AddWithValue("@HOddsAvg", match.BbAvH);
//                        cmd2.Parameters.AddWithValue("@AOddsAvg", match.BbAvA);
//                        cmd2.Parameters.AddWithValue("@DOddsAvg", match.BbAvD);
//                    }

//                    cmd2.Parameters.AddWithValue("@HHTGoals", match.HTHG);
//                    cmd2.Parameters.AddWithValue("@AHTGoals", match.HTAG);
//                    cmd2.Parameters.AddWithValue("@HTResult", match.HTR);
//                    cmd2.Parameters.AddWithValue("@HShots", match.HS);
//                    cmd2.Parameters.AddWithValue("@AShots", match.AS);
//                    cmd2.Parameters.AddWithValue("@HXShots", match.HST);
//                    cmd2.Parameters.AddWithValue("@AXShots", match.AST);
//                    cmd2.Parameters.AddWithValue("@HFouls", match.HF);
//                    cmd2.Parameters.AddWithValue("@AFouls", match.AF);
//                    cmd2.Parameters.AddWithValue("@HCorners", match.AC);
//                    cmd2.Parameters.AddWithValue("@ACorners", match.AC);
//                    cmd2.Parameters.AddWithValue("@HYellows", match.HY);
//                    cmd2.Parameters.AddWithValue("@AYellows", match.AY);
//                    cmd2.Parameters.AddWithValue("@HReds", match.HR);
//                    cmd2.Parameters.AddWithValue("@AReds", match.AR);

//                    if (cmd2.ExecuteNonQuery() != 1)
//                        throw new InvalidProgramException();
//                }
//                else
//                    clash++;

//            }

//        }

//    }

//    class HelperClass
//    {
//        public static int GetYear(DateTime date, DateTime minDate)
//        {
//            //DateTime minDate = new DateTime(1970, 7, 1);
//            //if (date.Subtract(minDate) % 365>)
//            int year = 0;
//            if (date.Subtract(new DateTime(date.Year, 1, 1)) > minDate.AddDays(-1).Subtract((new DateTime(minDate.Year, 1, 1))))
//                year = date.Year;
//            else
//                year = date.Year - 1;

//            return year;
//            //    var x = date.AddDays(diffDays);

//        }
//    }

//}
////public static void UpdateMatches(List<Match> matches, SqlConnection conn = null)
////{
////    conn = conn ?? OpenConnection();

////    using (SqlTransaction transaction = conn.BeginTransaction())
////    {
////        // original - this caused an error to be thrown.
////        //SqlCommand cmd = new SqlCommand("dbo.UpsertRatings", conn, transaction);
////        //cmd.CommandType = CommandType.StoredProcedure;

////        //string sql = System.IO.File.ReadAllText(@"C:\Users\rolyat4\Documents\Visual Studio 2015\Projects\Wpf Football\MasterApp\FootballDataApi\SportingLife\bin\Debug\sqlUpsertQuery.txt");
////        string sql = System.IO.File.ReadAllText("sqlUpsertQuery.txt");
////        SqlCommand cmd = new SqlCommand(sql, conn, transaction);

////        cmd.Parameters.Add(new SqlParameter("@competitionId", SqlDbType.Int));
////        cmd.Parameters.Add(new SqlParameter("@date", SqlDbType.DateTime));
////        cmd.Parameters.Add(new SqlParameter("@status", SqlDbType.VarChar));
////        cmd.Parameters.Add(new SqlParameter("@homeTeam", SqlDbType.VarChar));
////        cmd.Parameters.Add(new SqlParameter("@awayTeam", SqlDbType.VarChar));
////        cmd.Parameters.Add(new SqlParameter("@homeTeamScore", SqlDbType.Int));
////        cmd.Parameters.Add(new SqlParameter("@awayTeamScore", SqlDbType.Int));

////        try
////        {
////            foreach (Match match in matches)
////            {
////                cmd.Parameters[0].Value = match.CompetitionId;
////                cmd.Parameters[1].Value = match.EventDate;
////                cmd.Parameters[2].Value = match.Status;
////                cmd.Parameters[3].Value = match.HomeTeam;
////                cmd.Parameters[4].Value = match.AwayTeam;
////                cmd.Parameters[5].Value = match.HomeTeamScore;
////                cmd.Parameters[6].Value = match.AwayTeamScore;

////                if (cmd.ExecuteNonQuery() != 1)
////                {
////                    // handled as needed, but this snippet will throw an exception to force a rollback
////                    throw new InvalidProgramException();
////                }

////            }

////            transaction.Commit();

////        }
////        catch (Exception)
////        {
////            transaction.Rollback();
////            throw;
////        }

////        conn.Close();
////    }

////public string[] FindPossibleMatches(string team, string competitionId,System.Data.OleDb.OleDbConnection conn = null)
////{