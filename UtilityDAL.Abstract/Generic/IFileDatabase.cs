using System.Collections.Generic;

namespace UtilityDAL.Abstract.Generic
{
    // For mulit file based storage like CSV TeaTime

    public interface IFileDatabase<T>
    {
        IList<T> From(string name);

        bool To(IList<T> lst, string name);

        bool Clear(string name);

        List<string> SelectIds();
    }
}