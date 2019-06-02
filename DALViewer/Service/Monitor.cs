using DynamicData;
using DynamicData.Kernel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL;
using UtilityDAL.Contract;
using UtilityDAL.Contract.Generic;
using UtilityInterface;
using UtilityInterface.Generic;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.View
{

    public class Monitor<T> :IService<IChangeSet<T, int>> where T : IBSONRow, IEquatable<T>
    {


        //public IObservable<T> Adds { get; }
        //public IObservable<T> Removes { get; }
        public IObservable<IChangeSet<T, int>> Resource { get; }

        public Monitor(IFileDbService<T> repo, IScheduler scheduler)
        {

            Resource = Init(repo, scheduler);

            Resource.Subscribe(_ =>
            {

            });
        }



        public static IObservable<IChangeSet<T, int>> Init(IFileDbService<T> repo, IScheduler scheduler)
        {
            var dc = new UtilityDAL.Model.DefaultComparer<T>();

            return ObservableChangeSet.Create<T, int>(cache =>
            {

                scheduler
                .ScheduleRecurringAction(TimeSpan.FromSeconds(1),
                () =>
                {
                    var trades = repo.FromDb(null);
                    cache.EditDiff(trades,dc);
                });

                //expire closed items from the cache to avoid unbounded data
                //var expirer = cache
                //     .ExpireAfter(t => t.Status == TradeStatus.Closed ? TimeSpan.FromMinutes(1) : (TimeSpan?)null, TimeSpan.FromMinutes(1), _schedulerProvider.Background)
                //     .Subscribe(x => _logger.Info("{0} filled trades have been removed from memory", x.Count()));

                return System.Reactive.Disposables.Disposable.Empty;/*CompositeDisposable(tradeGenerator, tradeCloser, expirer);*/
            }, trade => trade.Id);
        }
    }







    public class Monitor2<T> : IService<IChangeSet<T, long>> where T : IChildRow, IEquatable<T>
    {

        public IObservable<IChangeSet<T, long>> Resource { get; }

        public Monitor2(IFileDbService<T> repo, IScheduler scheduler)
        {
            Resource = Init(repo, scheduler);

        }



        public static IObservable<IChangeSet<T, long>> Init(IFileDbService<T> repo, IScheduler scheduler)
        {
            var dc = new UtilityDAL.Model.DefaultComparer<T>();

            return ObservableChangeSet.Create<T, long>(cache =>
            {

                scheduler
                .ScheduleRecurringAction(TimeSpan.FromSeconds(1),
                () =>
                {
                    var trades = repo.FromDb(null);
                    cache.EditDiff(trades, dc);
                });

                //expire closed items from the cache to avoid unbounded data
                //var expirer = cache
                //     .ExpireAfter(t => t.Status == TradeStatus.Closed ? TimeSpan.FromMinutes(1) : (TimeSpan?)null, TimeSpan.FromMinutes(1), _schedulerProvider.Background)
                //     .Subscribe(x => _logger.Info("{0} filled trades have been removed from memory", x.Count()));

                return System.Reactive.Disposables.Disposable.Empty;/*CompositeDisposable(tradeGenerator, tradeCloser, expirer);*/
            }, trade => trade.Id);
        }
    }


    
    //public interface IMonitor<T, R>
    //{

    //    IObservable<IChangeSet<T, R>> Changes { get; }

    //}
}
