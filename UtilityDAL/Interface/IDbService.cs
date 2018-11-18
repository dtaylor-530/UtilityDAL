using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL
{
    public interface IDbService<T,R>:IDisposable
    {
        //object GetCollection(out IDisposable disposable);

        IEnumerable<T> FindAll();

        T Find(T item);

         T FindById(R item);

        bool Insert( T item);

        int InsertBulk(IList<T> item);

        bool Update( T item);

        bool Delete( T item);


    }

    public interface IDbService:IDisposable
    {
        //object GetCollection(out IDisposable disposable);

        IEnumerable FindAll();

        object Find( object item);

        object FindById(object item);

        bool Insert( object item);

        int InsertBulk(IList<object> item);

        bool Update(/*object items,*/ object item);

        bool Delete(/*object items,*/ object item);


    }

}
