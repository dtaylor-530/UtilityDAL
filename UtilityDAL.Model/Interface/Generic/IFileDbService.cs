using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL.Contract.Generic
{

    // For mulit file based storage like CSV TeaTime

    public interface IFileDbService<T>
    {
        IList<T> FromDb(string name);
        bool ToDb(IList<T> lst, string name);
        List<string> SelectIds();
    }

  
    public interface IFileDbAsyncService<T>
    {
        IObservable<T> FromDb(string name);
        bool ToDb(List<T> lst, string name);


    }
}
