using System;
using System.Linq;

namespace UtilityDAL
{
    public static class IdHelper
    {
        private static readonly string[] idnames = { "id", "key", "name", "identification" };

        public static string GetIdProperty<T>() => GetIdProperty(typeof(T));

        public static string GetIdProperty(Type type)
        {
            var pnames = type.GetProperties().Select(_ => _.Name.ToLower());
            return (idnames.First(_ => pnames.Contains(_)));
        }
    }
}