using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Model
{
    public class DefaultComparer<T> : IEqualityComparer<T> where T:IEquatable<T>
    {
        public bool Equals(T x, T y) => x.Equals(y);


        public int GetHashCode(T obj) => obj.GetHashCode();
   
    }
}
