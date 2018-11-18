using LiteDB;
using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL
{
    public static class LiteDbHelper
    {
        public static LiteCollection<T> GetCollection<T>(string directory, out IDisposable disposable)
        {
            disposable = new LiteDB.LiteDatabase(directory);
            return ((LiteDB.LiteDatabase)disposable).GetCollection<T>();
        }

        public static LiteCollection<object> GetCollection(string directory, out IDisposable disposable)
        {
            disposable = new LiteDB.LiteDatabase(directory);
            return ((LiteDB.LiteDatabase)disposable).GetCollection<object>();
        }
    }
}
