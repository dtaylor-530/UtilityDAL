using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using UtilityDAL.Contract.Generic;
using UtilityDAL.Model;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    public static class FileGenerator
    {
        internal static IObservable<DataFile> GenerateDataFilesDefault<T>(IFileDatabase<T> service, IService<IEnumerable<T>> gs, string extension)
        {
            return gs.Resource.Take(5).Select((_, i) =>
            {
                string name = i.ToString();
                var items = service.From(name);
                if (items == null)
                {
                    var prices = _.ToList();
                    service.To(prices, name);
                    items = service.From(name);
                }
                return (new DataFile { Key = name, Items = items.ToList() });
            });
        }

        internal static IObservable<DataFile> GenerateDataFilesDefault<T>(IFileDatabase<T> service, string extension)
        {
            //var tt = new UtilityDAL.Teatime(path);
            return System.Reactive.Linq.Observable.Create<DataFile>(observer =>
            {
                foreach (var id in service.SelectIds())
                {
                    var items = service.From(id);
                    if (items != null)
                        observer.OnNext(new DataFile { Key = id, Items = items.ToList() });
                }
                return Disposable.Empty;
            });
        }
    }
}