using System;
using System.Linq;
using UtilityInterface.Generic.Database;

namespace UtilityDAL
{
    public static class IDatabaseServiceHelper
    {
        public static bool Upsert<T, R>(this IDatabaseService<T, R> store, T _b)
        {
            //var collection = store.GetCollection(out IDisposable disposable);

            object result = null;
            try
            {
                result = store.Select(_b);
            }
            catch
            {
                var type = typeof(T);
                try
                {
                    result = store.Insert(_b);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "/n/r" + "unable to insert item " + _b.ToString());
                }
            }

            if (result == null)
                try
                {
                    result = store.Insert(_b);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "/n/r" + "unable to insert item " + _b.ToString());
                }
            else
                store.Update(_b);

            return true;
        }

        public static bool Upsert(this UtilityInterface.NonGeneric.Database.IDatabaseService store, object _b, string key)
        {
            //var collection = store.GetCollection(out IDisposable disposable);

            object result = null;
            try
            {
                result = store.Select(_b);
            }
            catch
            {
                var type = _b.GetType();
                if (type.GetProperties().SingleOrDefault(_a => _a.Name == key) != null)
                    try
                    {
                        result = store.Insert(_b);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message + "/n/r" + "unable to insert item " + _b.ToString());
                    }
                else
                    Console.WriteLine("Unable to insert item. " + type.Name + " does not have 'key' property " + key);
            }

            if (result == null)
                try
                {
                    result = store.Insert(_b);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "/n/r" + "unable to insert item " + _b.ToString());
                }
            else
                store.Update(_b);

            return true;
        }
    }
}