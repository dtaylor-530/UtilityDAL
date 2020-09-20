using System;
using System.Collections.Generic;

namespace UtilityDAL.Abstract.Generic
{

    public interface IFileDbAsyncService<T>
    {
        IObservable<T> From(string name);

        bool To(List<T> lst, string name);
    }
}