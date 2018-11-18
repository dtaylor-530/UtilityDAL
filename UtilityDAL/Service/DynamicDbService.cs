﻿
using DynamicData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL;
using UtilityReactive;

namespace UtilityDAL
{

    public class DynamicDbService<T> : DynamicDbService<T, IConvertible>
    {
        public DynamicDbService(Func<T, IConvertible> getKey = null):base(getKey)
        {

        }
    }


        public class DynamicDbService<T,R> : IDbService<T,R>
    {
        private ISubject<T> Adds = new Subject<T>();
        private ISubject<T> Removes = new Subject<T>();
        private IObservable<IChangeSet<T,R>> _changeset;


        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        public IObservableCache<T, R> Cache { get; }


        public DynamicDbService(Func<T, R> getKey = null)
        {
            _changeset = UtilityReactive.ChangeSetFactory.Build(getKey ?? GetKey(), (IObservable<T>)Adds, (IObservable<T>)Removes);

            _changeset.Publish();
            Cache = _changeset.AsObservableCache(true);

        }


        public T Find(R id)
        {
            return Cache.KeyValues.SingleOrDefault(_ => _.Key.Equals( id)).Value;
        }

        public bool Insert(T item)
        {
            Adds.OnNext(item);
            return true;
        }

        public bool Update(T item)
        {
            Adds.OnNext(item);
            return true;
        }

        public bool Delete(T item)
        {
            Removes.OnNext(item);
            return true;
        }

        protected virtual Func<T,R> GetKey()
        {
            UtilityDAL.IdHelper.GetIdProperty<T>();
            Func<T, R> getkey = _ => UtilityHelper.PropertyHelper.GetPropValue<R>(_, "Id");
            return getkey;

        }





        public void Combine(IObservable<T> set)
        {
            set.Subscribe(_ =>
            {
                Adds.OnNext(_);
            });
        }

        public IEnumerable<T> FindAll()
        {
          return  Cache.KeyValues.Select(_ => _.Value);
        }

        public T Find(T item)
        {
           return  Cache.KeyValues.SingleOrDefault(_ => _.Value.Equals(item)).Value;
        }

        public T FindById(R item)
        {
            return Cache.KeyValues.SingleOrDefault(_ => _.Key.Equals(item)).Value;
        }

        public int InsertBulk(IList<T> items)
        {
            int i = 0;
            foreach (var x in items)
            {
                Adds.OnNext(x);
                i++;
            }
            return i;
        }

        public void Dispose()
        {
              Cache.Dispose();
        }



        //Func<T, string> getKey => GetKey();

    }







}
