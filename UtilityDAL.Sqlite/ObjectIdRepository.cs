using phirSOFT.LazyDictionary;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using UtilityDAL.Sqlite.Utility;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Sqlite
{

    //public class DbEntityRepository : DbEntityRepository<IDbEntity>
    //{


    //    //internal IEnumerable<long> FindIds<T>(T[] firstNotInSecond, Type type) where T : IDbEntity, new()
    //    //{
    //    //    throw new NotImplementedException();
    //    //}
    //}


    public class DbEntityRepository<T> : ObjectIdRepository<T> where T : IDbEntity, new()
    {
        protected readonly SQLiteConnection connection;


        //public DbEntityRepository(Repository2<T> connection)
        //{
        //    this.connection = connection;
        //}

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
                return (obj as IGuid).Guid.ToByteArray().Take(3).Aggregate(0, (a, b) => a * b);
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

    public class ObjectIdRepository<T>
    {
        protected readonly LazyDictionary<Type, MyRef<long>> lazyDict3;
        protected readonly LazyDictionary<T, long> lazyDict2;
        //   readonly LazyDictionary<Type, long> lazyDict4;
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



        //class Ref
        //{
        //    public long Value { get; set; }
        //}


    }
        public class MyRef<TR>
        {
            public TR Ref { get; set; }
        }

}
