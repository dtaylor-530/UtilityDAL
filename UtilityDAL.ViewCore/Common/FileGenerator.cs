using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using UtilityDAL.Abstract.Generic;
using UtilityDAL.Model;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    public static class FileGenerator
    {
        internal static IObservable<KeyCollection> GenerateDataFilesDefault<T>(IFileDatabase<T> service, IObservableService<IEnumerable<T>> gs, string extension)
        {
            return gs.Observable.Take(5).Select((_, i) =>
            {
                string name = i.ToString();
                var items = service.From(name);
                if (items == null)
                {
                    var prices = _.ToList();
                    service.To(prices, name);
                    items = service.From(name);
                }
                return (new KeyCollection { Key = name, Collection = items.ToList() });
            });
        }

        internal static IObservable<KeyCollection> GenerateDataFilesDefault<T>(IFileDatabase<T> service, string extension)
        {
            //var tt = new UtilityDAL.Teatime(path);
            return System.Reactive.Linq.Observable.Create<KeyCollection>(observer =>
            {
                foreach (var id in service.SelectIds())
                {
                    var items = service.From(id);
                    if (items != null)
                        observer.OnNext(new KeyCollection { Key = id, Collection = items.ToList() });
                }
                return Disposable.Empty;
            });
        }
    }
}