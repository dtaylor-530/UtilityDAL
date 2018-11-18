using System;
using System.Collections.Generic;
using System.Text;
using UtilityInterface;
using System.Linq;

namespace UtilityDAL
{


    public class DocumentSaveLoadService<T> : IPermanent<IList<T>>
    {

        protected UtilityDAL.IDbService<T,IConvertible> Service; /*{ get; }*/

        public bool Save(object documents)
        {

            Service.InsertBulk(documents as IList<T>);

            return true;
        }

        public IList<T> Load()
        {
            IDisposable disposable = null;
            try
            {
                var x = Service.FindAll().ToList();
                return x;
            }
            catch
            {
                Console.WriteLine("error retrieving document collection");
                return null;
            }
            finally
            {
                disposable?.Dispose();
            }

        }

    }


    public class DocumentSaveLoadService<T,R> : IPermanent<IList<T>>
    {

        protected UtilityDAL.IDbService<T,R> Service; /*{ get; }*/

        public bool Save(object documents)
        {
            
            Service.InsertBulk(documents as IList<T>);

            return true;
        }

        public IList<T> Load()
        {
            IDisposable disposable = null;
            try
            {
                var x = Service.FindAll().ToList();
                return x;
            }
            catch
            {
                Console.WriteLine("error retrieving document collection");
                return null;
            }
            finally
            {
                disposable?.Dispose();
            }

        }


    }



    //public sealed class LiteDbSaveLoadService<T> : IPermanent<IList<T>>
    // {

    //     static LiteDbSaveLoadService()
    //     {
    //     }
    //     /*       public*/
    //     LiteDbRepo<T> Service; /*{ get; }*/

    //     public bool Save(object documents)
    //     {
    //         var x = (Service.GetCollection(out IDisposable disposable) as LiteDB.LiteCollection<T>);
    //         x.InsertBulk(documents as IList<T>);
    //         disposable.Dispose();
    //         return true;
    //     }

    //     public IList<T> Load()
    //     {
    //        var x=(Service.GetCollection(out IDisposable disposable) as LiteDB.LiteCollection<T>).FindAll().ToList();
    //         disposable.Dispose();
    //         return x;
    //     }

    //     LiteDbSaveLoadService()
    //     {
    //         Service = new LiteDbRepo<T>("Url", "../../Data");
    //     }

    //     private static Func<T, string> GetKey()
    //     {
    //         Func<T, string> getkey = _ => UtilityHelper.PropertyHelper.GetPropValue<string>(_, "Url");
    //         return getkey;

    //     }
    //     public static LiteDbSaveLoadService<T> Instance => new LiteDbSaveLoadService<T>();
    // }
}
