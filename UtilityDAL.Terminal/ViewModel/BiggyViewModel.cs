using UtilityDAL.ViewModel;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityWpf.ViewModel;
using UtilityDAL;
using Reactive.Bindings;

namespace UtilityDAL.DemoApp
{


    //public class BiggyViewModel : LiteDbViewModel<DummyDbObject>
    //{
    //    public LiteDbDummyViewModel(Interface.IRepoChangeService<DummyDbObject> repo, UtilityWpf.IDispatcherService scheduler) : base(repo.Resource, EnableAll<DummyDbObject>(), scheduler)
    //    {
    //        repo.CallBack(base.Output);

    //    }
    //    private static IObservable<Func<T, bool>> EnableAll<T>() => Observable.Repeat<Func<T, bool>>(_ => true, 1);
    //}



    //public class BiggyViewModel
    //{
    //    public BiggyRepo<SHDOObject, object> DocumentStore { get; }

    //    private static System.Random r = new Random();

    //    public ReactiveProperty<Pricecsv> NewItem { get; } = new ReactiveProperty<Pricecsv>(
    //        Observable.Interval(TimeSpan.FromSeconds(3))
    //        .StartWith(1)
    //        .Take(20)
    //        .Select((_, i) =>
    //        new Pricecsv { Date = new DateTime((i + 1), (i) % 11 + 1, (i) % 30 + 1), Bid = r.NextDouble(), Offer = r.NextDouble() }));



    //    public BiggyViewModel()
    //    {
    //        DocumentStore = new BiggyRepo<SHDOObject, object>();
    //    }

    //}




}
