using DynamicData;
using DynamicData.Kernel;
using System;
using System.Reactive.Concurrency;
using UtilityDAL.Contract;
using UtilityDAL.Contract.Generic;
using UtilityInterface.Generic;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.View
{
    public class Monitor<T> : IObservableService<IChangeSet<T, int>> where T : IBSONRow, IEquatable<T>
    {
        //public IObservable<T> Adds { get; }
        //public IObservable<T> Removes { get; }
        public IObservable<IChangeSet<T, int>> Service { get; }

        public Monitor(IFileDatabase<T> repo, IScheduler scheduler)
        {
            Service = Init(repo, scheduler);

            Service.Subscribe(_ =>
            {
            });
        }

        public static IObservable<IChangeSet<T, int>> Init(IFileDatabase<T> repo, IScheduler scheduler)
        {
            var dc = new UtilityDAL.Model.DefaultComparer<T>();

            return ObservableChangeSet.Create<T, int>(cache =>
            {
                scheduler
                .ScheduleRecurringAction(TimeSpan.FromSeconds(1),
                () =>
                {
                    var trades = repo.From(null);
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

    public class Monitor2<T> : IObservableService<IChangeSet<T, long>> where T : IChildRow, IEquatable<T>
    {
        public IObservable<IChangeSet<T, long>> Service { get; }

        public Monitor2(IFileDatabase<T> repo, IScheduler scheduler)
        {
            Service = Init(repo, scheduler);
        }

        public static IObservable<IChangeSet<T, long>> Init(IFileDatabase<T> repo, IScheduler scheduler)
        {
            var dc = new UtilityDAL.Model.DefaultComparer<T>();

            return ObservableChangeSet.Create<T, long>(cache =>
            {
                scheduler
                .ScheduleRecurringAction(TimeSpan.FromSeconds(1),
                () =>
                {
                    var trades = repo.From(null);
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