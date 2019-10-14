using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UtilityDAL.Model;
using UtilityInterface.Generic.Database;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.Sqlite
{
    public class NestedStore
    {
        private Dictionary<Type, object> container = new Dictionary<Type, object>();
        private SQLiteConnection _conn;

        public NestedStore(SQLiteConnection _conn)
        {
            this._conn = _conn;
        }

        public bool TransferToDB<T, R>(IEnumerable<T> items, bool check, params string[] children) where T : IEquatable<T>, IId, new() where R : IChildRow
        {
            _conn.CreateTable<T>();
            var service = TryGetAdd<T>();

            var accessors = children.Select(child =>
            {
                var childProperty = typeof(T).GetProperty(child);
                return new { set = childProperty.SetMethod, get = childProperty.GetMethod };
            });
            T[] itemsArray = items.ToArray();

            foreach (var ia in itemsArray)
                ia.Id = 0;

            var list = TryGetAdd<T>();

            var ty = list.GetList();

            var groupedItems = items.Concat(ty).GroupBy(_ => _).ToList();

            List<T> insertItems = new List<T>();

            //var groupedItemsArray = groupedItems.Select(_=>_.Key).ToArray();

            foreach (var x in groupedItems)
            {
                var xtt = x.SingleOrDefault(_ => _.Id != 0);
                xtt = xtt.Equals(default(T)) ? x.Key : xtt;
                foreach (var accessor in accessors)
                {
                    var g = x.SelectMany(_ => accessor.get.Invoke(_, null) as IEnumerable<R>).ToList();
                    foreach (var z in g)
                    {
                        z.ParentId = xtt.Id;
                    }
                    accessor.set.Invoke(xtt, new object[] { g });
                }

                insertItems.Add(xtt?.Equals(default(T)) ?? true ? x.Key : xtt);
            }

            var inn = insertItems.Except(ty);
            var xx = UtilityDAL.SqliteEx.ToDB(inn, ref ty, _conn, check);

            Debug.Assert(ty.All(_ => _.Id != 0));

            return xx;
        }

        public bool TransferToDB2<T, R>(IEnumerable<T> items, bool check, params string[] children) where T : DbRow, IEquatable<T>, new() where R : IChildRow<DbRow>
        {
            _conn.CreateTable<T>();
            var accessors = children.Select(child =>
            {
                var childProperty = typeof(T).GetProperty(child);
                return new { set = childProperty.SetMethod, get = childProperty.GetMethod };
            });
            var list = TryGetAdd<T>();

            var ty = list.GetList();

            T[] itemsArray = items.ToArray();

            foreach (var ia in itemsArray)
                ia.Id = 0;

            var aty = itemsArray.Concat(ty).Select(_ => _.GetHashCode()).ToArray();

            var groupedItems = itemsArray.Concat(ty).ToList().GroupBy(_ => _).ToList();

            List<T> insertItems = new List<T>();

            //var groupedItemsArray = groupedItems.Select(_=>_.Key).ToArray();

            foreach (var x in groupedItems)
            {
                var xtt = x.SingleOrDefault(_ => _.Id != 0) ?? x.Key;

                foreach (var accessor in accessors)
                {
                    var g = x.SelectMany(_ => (accessor.get.Invoke(_, null) ?? new R[] { }) as IEnumerable<R>).ToList();
                    foreach (var z in g)
                    {
                        z.Parent = xtt;
                    }
                    accessor.set.Invoke(xtt, new object[] { g });
                }

                insertItems.Add(xtt);
            }

            var inn = insertItems.Except(ty).ToArray();

            list.SetList(ty);
            //groupedItems = inn.GroupBy(_ => _).ToList();
            foreach (var x in groupedItems)
            {
                var tttt = x.Key;
                var tdy = x.Single(_ => _.Id != 0);
                foreach (var item in x.ToList())
                {
                    if (item.Id != tdy.Id)
                        item.Id = tdy.Id;
                }
            }

            var xx = UtilityDAL.SqliteEx.ToDB(inn, ref ty, _conn, check);

            Debug.Assert(ty.All(_ => _.Id != 0));

            return xx;
        }

        public bool TransferToDB2<T>(IEnumerable<T> items, bool check) where T : DbRow, IEquatable<T>, new()
        {
            _conn.CreateTable<T>();

            var list = TryGetAdd<T>();

            var ty = list.GetList();

            T[] itemsArray = items.ToArray();

            foreach (var ia in itemsArray)
                ia.Id = 0;

            var groupedItems = itemsArray.Concat(ty).GroupBy(_ => _).ToList();

            List<T> insertItems = new List<T>();

            //var groupedItemsArray = groupedItems.Select(_=>_.Key).ToArray();

            foreach (var x in groupedItems)
            {
                var xtt = x.SingleOrDefault(_ => _.Id != 0) ?? x.Key;

                insertItems.Add(xtt);
            }

            var inn = insertItems.Except(ty).ToArray();

            var xx = UtilityDAL.SqliteEx.ToDB(inn, ref ty, _conn, check);

            Debug.Assert(ty.All(_ => _.Id != 0));

            return xx;
        }

        private TableList<T> TryGetAdd<T>() where T : IEquatable<T>, new()
        {
            container[typeof(T)] = container.TryGetValue(typeof(T), out object value) ? (TableList<T>)value : new TableList<T>(_conn);

            return (TableList<T>)container[typeof(T)];
        }

        public bool TransferToDB<T>(IEnumerable<T> items, bool check) where T : IEquatable<T>, IId, new()
        {
            _conn.CreateTable<T>();
            var xx = TryGetAdd<T>().GetList();
            return UtilityDAL.SqliteEx.ToDB(items.GroupBy(_ => _).Select(_ => _.Key).ToList(), ref xx, _conn, check);
        }

        //public bool TransferToDB2<T, R>(IEnumerable<T> items, Func<T, IEnumerable<R>> children, Action<IEnumerable<R>, T> setchildren) where T : DbRow, IEquatable<T>, new() where R : UtilityInterface.Database.IChildRow<DbRow>
        //{
        //    _conn.CreateTable<T>();
        //    var newItems = items.GroupBy(_ => _).ToList();
        //    var xx = UtilityDAL.SqliteEx.ToDB(newItems.Select(_ => _.Key).ToList(), TryGetAdd<T>().GetList(), _conn);
        //    foreach (var x in newItems)
        //    {
        //        var tttt = x.Key;

        //        setchildren(x.SelectMany(l => children(l)).ToList(), tttt);//.GroupBy(a => a, ckc)).SelectMany(v => v.ToList()).ToList();

        //        foreach (var z in children(tttt))
        //        {
        //            z.Parent = tttt;
        //        }

        //    }

        //    return xx;

        //}        //public bool TransferToDB<T, R>(IEnumerable<T> items, Func<T, IEnumerable<R>> children, Action<IEnumerable<R>, T> setchildren) where T : IEquatable<T>, UtilityInterface.Database.IId, new() where R : UtilityInterface.Database.IChildRow
        //{
        //    _conn.CreateTable<T>();
        //    var service = TryGetAdd<T>();

        //    var newItems = items.GroupBy(_ => _).ToList();
        //    var xx = UtilityDAL.SqliteEx.ToDB(newItems.Select(_=>_.Key).ToList(), service.GetList(), _conn);
        //    foreach (var x in newItems)
        //    {
        //        var tttt = x.Key;

        //        setchildren(x.SelectMany(l => children(l)).ToList(), tttt);//.GroupBy(a => a, ckc)).SelectMany(v => v.ToList()).ToList();

        //        foreach (var z in children(tttt))
        //        {
        //            z.ParentId = tttt.Id;
        //        }
        //    }

        //    return xx;
        //}
    }
}