//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reactive.Disposables;
//using System.Text;
//using Biggy.Core;
//using Biggy.Data.Json;
//using UtilityDAL.Contract;
//using UtilityHelper;
//using UtilityInterface.Generic.Database;


//namespace UtilityDAL
//{
//    public class BiggyRepo<T,R> : UtilityModel.Disposable, IDbService<T, R> where T : new()
//    {
//        private readonly string _key;
//        private readonly Func<T, R> _getkey;
//        private readonly JsonStore<T> _store;
//        private readonly BiggyList<T> _list;

//        public BiggyRepo(string key)
//        {
//            _key = key;

//            _store = new Biggy.Data.Json.JsonStore<T>();
//            _list = new Biggy.Core.BiggyList<T>(_store);

//        }

//        public BiggyRepo(Func<T,R> getkey)
//        {
//            _getkey = getkey;
//            _store = new Biggy.Data.Json.JsonStore<T>();
      
//            _list = new Biggy.Core.BiggyList<T>(_store);


//        }
//        public BiggyRepo()
//        {
//            //_key = key;
//            _store = new Biggy.Data.Json.JsonStore<T>();
//            _list = new Biggy.Core.BiggyList<T>(_store);

//        }
//        //public object GetCollection(out IDisposable disposable)
//        //{
//        //    var 
//        //    disposable = new MyDisposable();
//        //    return store;
//        //}

//        public IEnumerable<T> FindAll()
//        {
//            return _list;/*new Biggy.Core.BiggyList<T>(_store).ToList()*/;
//        }


//        public T Find(T item)
//        {
//            if (_key != null)
//            {
//                var xx = item.GetPropertyValue<string>(_key);
//                return _list.Single(_s => UtilityHelper.PropertyHelper.GetPropertyValue<string>(_s, _key).Equals(xx));
//            }
//            else if (_getkey != null)
//            {
//                return _list.Single(_s => _getkey(_s).Equals(_getkey(item)));
//            }
//            else
//                throw new Exception("neither key or key expression is not null");
//        }

//        public bool Insert(T item)
//        {
//            _list.Add(item/*(T)Converter.Convert(item, null, typeof(T), null)*/);
//            return true;
//        }

//        public bool Update(T item)
//        {
//            _list.Update(item);
//            return true;
//        }

//        public bool Delete(T item)
//        {
//            _list.Remove(item);
//            return true;
//        }

//        //private void Command(Func<Biggy.Core.BiggyList<T>, T, IConvertible> func, object item, Biggy.Core.BiggyList<T> list)
//        //{
//        //    //this.Dispatcher.InvokeAsync(() =>
//        //    //{
//        //    try
//        //    {
//        //        func(list, (T)item);
//        //    }
//        //    catch
//        //    {
//        //        Console.WriteLine("Error changing items in database");
//        //    }
//        //    //LiteDB.BsonDocument(UtilityHelper.PropertyHelper.FromObject(item).ToDictionary(_a => _a.Key, _a => new LiteDB.BsonValue(_a))
//        //    //ItemsSource = customers.FindAll();
//        //    //}, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
//        //}

//        public int InsertBulk(IList<T> items)
//        {
//            _list.Add(items);
//            return items.Count();
//        }

//        public T FindById(R item)
//        {
//            return _list.Single(_s => UtilityHelper.PropertyHelper.GetPropertyValue<R>(_s, _key).Equals(item));
//        }

//        public T FindById(string item)
//        {
//            throw new NotImplementedException();
//        }

//        public IEnumerable<T> SelectAll()
//        {
//            throw new NotImplementedException();
//        }

//        public T Select(T item)
//        {
//            throw new NotImplementedException();
//        }

//        public T SelectById(R id)
//        {
//            throw new NotImplementedException();
//        }

//        public int InsertBulk(IEnumerable<T> item)
//        {
//            throw new NotImplementedException();
//        }

//        public int UpdateBulk(IEnumerable<T> item)
//        {
//            throw new NotImplementedException();
//        }

//        public int DeleteBulk(IEnumerable<T> item)
//        {
//            throw new NotImplementedException();
//        }

//        public bool DeleteById(R id)
//        {
//            throw new NotImplementedException();
//        }
//    }

//    public class BiggyRepo<T> : BiggyRepo<T, IConvertible> where T : new()
//    {


//        public BiggyRepo(string key) : base(key)
//        {


//        }
//        public BiggyRepo(Func<T, IConvertible> getkey):base(getkey)
//        {

//        }

//        public BiggyRepo() : base()
//        {

//        }
//    }
//}