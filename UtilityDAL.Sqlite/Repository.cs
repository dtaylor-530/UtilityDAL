using MoreLinq;
using phirSOFT.LazyDictionary;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityDAL.Sqlite.Utility;
using UtilityHelper;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Sqlite
{
    public abstract class Repository<T> where T : IDbEntity, new()
    {
        //public const string SqliteExtension = "sqlite";

       // protected const string Data = "Data";

        protected readonly Dictionary<Type, Lazy<List<long>>> idDictionary;
        protected readonly Lazy<DbEntityRepository> objectIdRepository;
        protected readonly Lazy<SQLiteConnection> connection;

        public Repository(string name)
        {
            this.Name = name;
            objectIdRepository = LazyEx.Create(()=> new DbEntityRepository(this.connection.Value));
            connection = new Lazy<SQLiteConnection>(() => new SQLiteConnection(ConnectionPath));
        }

        public string Name { get; }

        public abstract string ConnectionPath { get; }

        public SQLiteConnection Connection => connection.Value;

        public virtual int Insert(IEnumerable<T> items) 
        {
            if (items.Any() == false)
                return 0;

            var arr = items.ToArray();
            var type = items.First().GetType();

            connection.Value.CreateTable(type);

            var tableName = SqliteEx.GetSqliteName(items.First().GetType());

            var guids = DatabaseConnection.Query<MyRef<Guid>>($"select Guid as {nameof(MyRef<Guid>.Ref)} from {tableName}").ToList();

            var firstNotInSecond = LinqExtension.SelectFromFirstNotInSecond(arr, guids.Select(a => a.Ref), a => (a as IGuid).Guid, a => a).DistinctBy(a => (a as IGuid).Guid).ToArray();

            var count_ = firstNotInSecond.GroupBy(a => (a as IGuid).Guid).Where(a => a.Count() > 1).ToArray();

            if (count_.Length > 0)
            {
                throw new Exception("items in set share same Id");
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
                    throw new Exception("items in set share same Id");
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


        public class DbEntityRepository : ObjectIdRepository
        {
            protected readonly SQLiteConnection connection;

            public DbEntityRepository(SQLiteConnection connection) : base(new EqualityComparer())
            {
                this.connection = connection;
            }

            class EqualityComparer : IEqualityComparer<T>
            {
                public bool Equals(T x, T y)
                {
                    if ((x as IGuid).Guid == (y as IGuid).Guid)
                    {
                        return true;
                    }
                    return false;
                }

                public int GetHashCode(T obj)
                {
                    return (obj as IGuid).Guid.GetHashCode();
                }
            }

            protected override long GetLastId(Type type)
            {
                var maxId3 = connection.Query<Ref>($"select Max({nameof(IId.Id)}) as {nameof(Ref.Value)} from {type.GetSqliteName()}");
                return maxId3.SingleOrDefault()?.Value ?? 0;
            }

            class Ref
            {
                public long Value { get; set; }
            }
        }

        public class ObjectIdRepository
        {
            protected readonly LazyDictionary<Type, MyRef<long>> lazyDict3;
            protected readonly LazyDictionary<T, long> lazyDict2;
            protected readonly IEqualityComparer<T> comparer;

            public ObjectIdRepository(IEqualityComparer<T> comparer)
            {

                lazyDict3 = new LazyDictionary<Type, MyRef<long>>(t =>
                {
                    return new MyRef<long> { Ref = GetLastId(t) };
                });

                lazyDict2 = new LazyDictionary<T, long>(dbe =>
                {
                    return ++lazyDict3[dbe.GetType()].Ref;
                }, comparer);
                this.comparer = comparer;
            }


            public long FindId(T obj)
            {
                lock (lazyDict2)
                    return lazyDict2[obj];
            }

            public virtual IEnumerable<long> FindIds(IEnumerable<T> objs, Type type)
            {
                var lazyDict4 = new LazyDictionary<T, long>(dbe =>
                {
                    return ++lazyDict3[type].Ref;
                }, comparer);

                //var type = Helper.GetParentTypes(typeof(T)).FirstOrDefault() ;
                var enumerator = objs.GetEnumerator();

                lock (lazyDict4)
                    lock (lazyDict3)
                        while (enumerator.MoveNext())
                            yield return lazyDict4[enumerator.Current];
            }

            protected virtual long GetLastId(Type type)
            {
                return 0;
            }

        }

        public class MyRef<TR>
        {
            public TR Ref { get; set; }
        }
    }
}