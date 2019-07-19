using LiteDB;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityDAL.Model;
using UtilityHelper;
using UtilityDAL.Contract;
using UtilityInterface.NonGeneric.Database;
using UtilityInterface.Generic.Database;

namespace UtilityDAL
{

    public class LiteDbRepo<T> : LiteDbRepo<T, IConvertible> where T : new()
    {


        public LiteDbRepo(Func<T, IConvertible> getkey, string directory):base(getkey,directory)
        {

        }

    }


    public  class LiteDbRepo<T,R>: IDbService<T,R>
    {
        private readonly Func<T, R> _getkey = null;
        private readonly string _key = null;
        //private readonly string _directory;
        private readonly LiteCollection<T> _collection;
        private readonly IDisposable _disposable;


        public LiteDbRepo(Func<T,R> getkey, string directory)
        {
            _getkey = getkey;
            //_directory = directory;
            _collection = LiteDbHelper.GetCollection<T>(directory, out IDisposable _disposable);

        }

        public LiteDbRepo(string key, string directory)
        {
            _key = key;
            //_directory = directory;
            _collection = LiteDbHelper.GetCollection<T>(directory, out IDisposable _disposable);

        }

        public LiteDbRepo( string directory)
        {
            _collection = LiteDbHelper.GetCollection<T>(directory, out IDisposable _disposable);
        }

        public LiteDbRepo()
        {
      
        }

        //public IConvertible GetKey(object trade)
        //{
        //    return (UtilityHelper.PropertyHelper.GetPropertyValue<IConvertible>(trade, _key, typeof(T)));
        //}

        public IEnumerable<T> FindAll()
        {
            return _collection.IncludeAll().FindAll();
        }

        public T Find(T item)
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
            //    return _collection.FindById(new LiteDB.BsonValue(_getkey(item)));
        }

        public bool Insert( T item)
        {
            _collection.Insert(item);
            return true;
        }

        public bool Update( T item)
        {
            _collection.Update(item);
            return true;
        }


        public bool Delete( T item)
        {
            if (_key == null)
            {
                var key = _getkey(item);
                _collection.Delete(_ => _getkey(_).Equals(key));
            }
            else if (_key != null)
                _collection.Delete(Query.EQ(_key, new BsonValue((object)item.GetPropertyValue<R>(_key))));
            else
                return false;
            return true;
        }

        public int InsertBulk(IList<T> items)
        {
            return _collection.InsertBulk(items);
        }

        public void Dispose()
        {
            _disposable?.Dispose();
        }

        public T FindById(R item)
        {
            return _collection.FindById(item.ToString());
           // return _collection.FindById(new BsonValue(item.ToString()));
        }

        public IEnumerable<T> SelectAll()
        {
            throw new NotImplementedException();
        }

        public T Select(T item)
        {
            throw new NotImplementedException();
        }

        public T SelectById(R id)
        {
            throw new NotImplementedException();
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



    public class LiteDbRepo : IDbService
    {
        string _key = null;
        //private string _directory;
        private LiteCollection<object> _collection;
        IDisposable _disposable;


        public LiteDbRepo(string key, string directory)
        {
            _key = key;
            //_directory = directory;
            _collection = LiteDbHelper.GetCollection(directory,out _disposable);

        }
        public LiteDbRepo()
        {
        }

        public IConvertible GetKey(object trade)
        {
            return (UtilityHelper.PropertyHelper.GetPropertyValue<IConvertible>(trade, _key.ToString()));
        }

        public IEnumerable FindAll()
        {
            return _collection.FindAll();
        }

        public object Find(object item)
        {
            return (_collection as LiteCollection<object>).FindById(new LiteDB.BsonValue(UtilityHelper.PropertyHelper.GetPropertyValue<IConvertible>(item, _key, typeof(object))));
        }

        public bool Insert(object item)
        {
            (_collection).Insert(item);
            return true;
        }

        public bool Update( object item)
        {
            (_collection).Update(item);
            return true;
        }


        public bool Delete(object item)
        {
            (_collection).Delete(_ => _.GetPropertyValue<IConvertible>(_key, typeof(object)).Equals(item.GetPropertyValue<IConvertible>(_key, typeof(object))));
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

        public object FindById(object item)
        {
            return _collection.FindById(new BsonValue(item));
        }

        public IEnumerable SelectAll()
        {
            throw new NotImplementedException();
        }

        public object Select(object item)
        {
            throw new NotImplementedException();
        }

        public object SelectById(object item)
        {
            throw new NotImplementedException();
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
