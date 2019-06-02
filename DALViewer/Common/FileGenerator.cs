using UtilityDAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL;
using UtilityInterface;
using UtilityDAL.Contract;
using UtilityDAL.Contract.Generic;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{

    public static class FileGenerator
    {

        internal static IObservable<DataFile> GenerateDataFilesDefault<T>(IFileDbService<T> service, IService<IEnumerable<T>> gs, string extension)
        {


            return gs.Resource.Take(5).Select((_,i) =>
            {
                string name = i.ToString();
                var items = service.FromDb(name);
                if (items == null)
                {
                    var prices = _.ToList();
                    service.ToDb(prices, name);
                    items = service.FromDb(name);
                }
                return (new DataFile { Key = name, Items = items.ToList() });
            });

        }


        internal static IObservable<DataFile> GenerateDataFilesDefault<T>(IFileDbService<T> service, string extension)
        {
            //var tt = new UtilityDAL.Teatime(path);
            return System.Reactive.Linq.Observable.Create<DataFile>(observer =>
            {
                foreach (var id in service.SelectIds())
                {
                    var items = service.FromDb(id);
                    if (items != null)
                        observer.OnNext(new DataFile { Key = id, Items = items.ToList() });

                }
                return Disposable.Empty;
            });
        }






    }



}
