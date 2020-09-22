using MoreLinq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityDAL.Sqlite.Utility;
using UtilityHelper;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Sqlite
{
    public abstract class Repository2<T> where T : IDbEntity, new()
    {
        //public const string FileName = "BFBotApp";

        //public static readonly string AppDirectory = Directory.CreateDirectory(Path.Combine(@"C:\Users\rytal\AppData\Local\", FileName)).FullName;

        //public static readonly string DataDirectory = Directory.CreateDirectory(Path.Combine(AppDirectory, Data)).FullName;

        public const string SqliteExtension = "sqlite";

        protected const string Data = "Data";

        protected readonly Dictionary<Type, Lazy<List<long>>> idDictionary;
        protected readonly Lazy<DbEntityRepository<T>> objectIdRepository;
        protected readonly Lazy<SQLiteConnection> connection;

        public Repository2(string name)
        {
            this.Name = name;
            objectIdRepository = LazyEx.Create(()=> new DbEntityRepository<T>(this.connection.Value));
            connection = new Lazy<SQLiteConnection>(() => new SQLiteConnection(ConnectionPath));
        }

        public string Name { get; }

        public abstract string ConnectionPath { get; }

       // public virtual string ConnectionPath => Path.Combine(DataDirectory, Name + "." + SqliteExtension);

        public virtual int Insert(IEnumerable<T> items)
        {
            var conn = DatabaseConnection;
            int insert = conn.InsertAll(items);
            return insert;
        }

        public virtual int InsertOnlyNew(IEnumerable<T> items) 
        {
            var arr = items;

            var tableName = SqliteEx.GetSqliteName(typeof(T));

            var guids = DatabaseConnection.Query<MyRef<Guid>>($"select Guid as {nameof(MyRef<Guid>.Ref)} from {tableName}").ToList();

            //var orderDetails = DatabaseConnection.Query<dynamic>($"select * from {tableName}");

            var firstNotInSecond = LinqExtension.SelectFromFirstNotInSecond(items, guids.Select(a => a.Ref), a => (a as IGuid).Guid, a => a).DistinctBy(a => (a as IGuid).Guid).ToArray();

            var count_ = firstNotInSecond.GroupBy(a => (a as IGuid).Guid).Where(a => a.Count() > 1).ToArray();

            if (count_.Length > 0)
            {

            }

            int insert = 0;

            if (firstNotInSecond.Any())
            {
                DateTime dateTime = DateTime.Now;
                foreach (var (entity, id) in firstNotInSecond.Zip(objectIdRepository.Value.FindIds(firstNotInSecond, firstNotInSecond.First().GetType()), (a, b) => (a, b)))
                {
                    (entity as ISetId).Id = id;
                    entity.AddedTime = dateTime;
                }

                var count = firstNotInSecond.GroupBy(a => (a as IId).Id).Where(a => a.Count() > 1).ToArray();
                if (count.Length > 0)
                {

                }

                insert = DatabaseConnection.InsertAll(firstNotInSecond);
            }
            return insert;
        }

        public virtual SQLiteConnection DatabaseConnection => connection.Value;

        public long SpareId()
        {
            var list = idDictionary[typeof(T)].Value;
            var last = list.Last() + 1;
            list.Add(last);
            return last;
        }
    }
}
