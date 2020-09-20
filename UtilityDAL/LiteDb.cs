using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using UtilityHelper;
using UtilityInterface.Generic.Database;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL
{
    public class LiteDbRepo<T> : LiteDbRepo<T, IConvertible> where T : new()
    {
        public LiteDbRepo(Func<T, IConvertible> getkey, string directory) : base(getkey, directory)
        {
        }
    }


    public class LiteDbDateRepo<T, R> : LiteDbRepo<T, R>
    {
        // Create a new file every month since the limit on collections is 300 (so creating one every year wouldn't work)
        public LiteDbDateRepo(Func<T, R> keyPredicate, string directory) : base(keyPredicate, 
            directory,
            DateTime.Now.Year.ToString() + DateTime.Now.MonthName(), 
            DateTime.Now.ToString(Constants.DateFormat) )
        {

        }

    }

    public class LiteDbRepo<T, R> : IDatabaseService<T, R>, IDisposable
    {
        private readonly Func<T, R> _getkey = null;
        private readonly string _key = null;

        //private readonly string _directory;
        private readonly ILiteCollection<T> _collection;

        private readonly IDisposable _disposable;

        public LiteDbRepo(Func<T, R> getkey, string directory, string name = null, string collectionName = null)
        {
            _getkey = getkey;
            //_directory = directory;
            _collection = LiteDbHelper.GetCollection<T>(directory, name ?? typeof(T).Name,  out _disposable, collectionName);
        }

        public LiteDbRepo(string key, string directory)
        {
            _key = key;
            //_directory = directory;
            _collection = LiteDbHelper.GetCollection<T>(directory, typeof(T).Name, out _disposable);
        }

        public LiteDbRepo(string directory)
        {
            _collection = LiteDbHelper.GetCollection<T>(directory, typeof(T).Name, out _disposable);
        }

        public LiteDbRepo()
        {
        }


        public void Dispose()
        {
            _disposable?.Dispose();
        }


        public bool Insert(T item)
        {
            _collection.Insert(item);
            return true;
        }

        public bool Update(T item)
        {
            _collection.Update(item);
            return true;
        }

        public bool Delete(T item)
        {
            if (_key == null)
            {
                var key = _getkey(item);
                _collection.DeleteMany(_ => _getkey(_).Equals(key));
            }
            else if (_key != null)
                _collection.Delete(new BsonValue(item.GetPropertyValue<R>(_key)));
            else
                return false;
            return true;
        }

        public int InsertBulk(IList<T> items)
        {
            return _collection.Insert(items);
        }

        public IEnumerable<T> SelectAll()
        {
            return _collection.FindAll();
        }

        public T Select(T item)
        {
            if (_key == null)
            {
                if (_getkey != null)
                {
                    var key = _getkey(item);
                    return _collection.FindOne(_ => _getkey(_).Equals(key));
                }
            }
            else if (_key != null)
                return _collection.FindOne(Query.EQ(_key, new BsonValue((object)item.GetPropertyValue<R>(_key))));

            return default(T);
        }

        public T SelectById(R id)
        {
            return _collection.FindById(new BsonValue(id));
        }

        public int InsertBulk(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }

        public int UpdateBulk(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }

        public int DeleteBulk(IEnumerable<T> item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(R id)
        {
            throw new NotImplementedException();
        }
    }



    public class LiteDbRepo : IDatabaseService, IDisposable
    {
        private readonly string _key = null;

        //private string _directory;
        private ILiteCollection<object> _collection;

        private IDisposable _disposable;

# warning do not use
        public LiteDbRepo(string key, string directory)
        {
            _key = key;
            //_directory = directory;
          //  _collection = LiteDbHelper.GetCollection(directory, out _disposable);
        }

        public LiteDbRepo()
        {
        }

        public IConvertible GetKey(object trade)
        {
            return (UtilityHelper.PropertyHelper.GetPropertyValue<IConvertible>(trade, _key.ToString()));
        }



        public bool Insert(object item)
        {
            (_collection).Insert(item);
            return true;
        }

        public bool Update(object item)
        {
            (_collection).Update(item);
            return true;
        }

        public bool Delete(object item)
        {
            (_collection).DeleteMany(_ => _.GetPropertyValue<IConvertible>(_key, typeof(object)).Equals(item.GetPropertyValue<IConvertible>(_key, typeof(object))));
            return true;
        }

        public int InsertBulk(IList<object> items)
        {
            return _collection.InsertBulk(items);
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public IEnumerable SelectAll()
        {
            return _collection.FindAll();
        }

        public object Select(object item)
        {
            return (_collection as LiteCollection<object>).FindById(new LiteDB.BsonValue(UtilityHelper.PropertyHelper.GetPropertyValue<IConvertible>(item, _key, typeof(object))));

        }

        public object SelectById(object item)
        {
            return _collection.FindById(new BsonValue(item));
        }

        public int InsertBulk(IEnumerable item)
        {
            throw new NotImplementedException();
        }

        public int UpdateBulk(IEnumerable item)
        {
            throw new NotImplementedException();
        }

        public int DeleteBulk(IEnumerable item)
        {
            throw new NotImplementedException();
        }

        public bool DeleteById(object item)
        {
            throw new NotImplementedException();
        }
    }
}