using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL
{
    public class DefaultComparer<T> : IEqualityComparer<T> where T : IEquatable<T>
    {
        public bool Equals(T x, T y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(T obj)
        {
            return obj.GetHashCode();
        }
    }

}
