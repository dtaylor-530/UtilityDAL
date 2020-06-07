using LiteDB;
using System;
using System.IO;

namespace UtilityDAL
{
    public static class LiteDbHelper
    {
        public static ILiteCollection<T> GetCollection<T>(string directory, string name, out IDisposable disposable, string collectionName = null)
        {
            disposable = new LiteDatabase(Path.Combine(directory, name+ "."+ Constants.LiteDbExtension));
            return ((LiteDatabase)disposable).GetCollection<T>(collectionName);
        }

        //public static ILiteCollection<object> GetCollection(string directory, string name, out IDisposable disposable)
        //{
        //    disposable = new LiteDatabase(directory);
        //    return ((LiteDatabase)disposable).GetCollection<object>();
        //}
    }
}