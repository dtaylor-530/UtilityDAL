using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL.Contract.Generic
{

    // For mulit file based storage like CSV TeaTime

    public interface IFileDatabase<T>
    {
        IList<T> From(string name);
        bool To(IList<T> lst, string name);

        bool Clear(string name);

        List<string> SelectIds();
    }

  
    public interface IFileDbAsyncService<T>
    {
        IObservable<T> From(string name);
        bool To(List<T> lst, string name);


    }
}
