using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using UtilityDAL.Abstract.Generic;
using UtilityDAL.Abstract.NonGeneric;
using UtilityDAL.Model;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    public static class csvHelper
    {
        public static IObservable<KeyCollection> GenerateDataFilesDefault<T>(IFileDatabase<T> service, IObservableService<IEnumerable<T>> gs, string extension)
        {
            return gs.Observable.Select((_, i) =>
            {
                string name = i.ToString();
                List<T> items = null;
                try
                {
                    items = service.From(name).ToList();
                }
                catch
                {
                    Console.WriteLine("Generating csv files in bin directory");
                    var prices = _.ToList();
                    service.To(prices, name);
                    items = service.From(name).ToList();
                }

                return (new KeyCollection { Key = name, Collection = items.ToList() });
            });
        }

        public static IObservable<KeyCollection> GenerateDataFilesDefault<T>(IFileDatabase<T> service, string extension, int? count = null)
        {
            //var tt = new UtilityDAL.Teatime(path);
            return System.Reactive.Linq.Observable.Create<KeyCollection>(observer =>
            {
                var ids = count == null ? service.SelectIds() : service.SelectIds().Take((int)count);
                foreach (var id in ids)
                {
                    try
                    {
                        var items = service.From(id);
                        observer.OnNext(new KeyCollection { Key = id, Collection = items?.ToList() });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error parsing file " + id + "\n\r" + ex.Message);
                        observer.OnNext(new KeyCollection { Key = id, Collection = null });
                    }
                }

                return Disposable.Empty;
            });
        }

        public static IObservable<KeyCollection> GenerateDataFilesDefault(IFileDatabase service, string extension, int? count = null)
        {
            //var tt = new UtilityDAL.Teatime(path);
            return System.Reactive.Linq.Observable.Create<KeyCollection>(observer =>
            {
                var ids = count == null ? service.SelectIds() : service.SelectIds().Take((int)count);
                foreach (var id in ids)
                {
                    try
                    {
                        var items = service.From(id);
                        observer.OnNext(new KeyCollection { Key = id, Collection = items });
                    }
                    catch
                    {
                        observer.OnNext(new KeyCollection { Key = id, Collection = null });
                    }
                }
                return Disposable.Empty;
            });
        }

        public static IObservable<KeyCollection> GenerateDataFilesDefault(IFileDatabase service, TimeSpan ts, string extension)
        {
            //var tt = new UtilityDAL.Teatime(path);

            return System.Reactive.Linq.Observable.Create<KeyCollection>(observer =>
            {
                return Observable.Interval(ts).Zip(service.SelectIds(), (a, b) => b).Subscribe(
                id =>
                {
                    try
                    {
                        var items = service.From(id);
                        observer.OnNext(new KeyCollection { Key = id, Collection = items });
                    }
                    catch
                    {
                        observer.OnNext(new KeyCollection { Key = id, Collection = null });
                    }
                });
            });
        }

        private static Random r = new Random();
    }
}