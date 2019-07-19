using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL;
using DynamicData;
using DynamicData.Kernel;
using UtilityWpf.ViewModel;


namespace UtilityDAL.ViewModel
{


    public class LiteDb
    {


    }



    //public class LiteDbViewModel<T> : IOutputViewModel<KeyValuePair<UtilityEnum.Database.Operation, T>>/* where T : UtilityDAL.Litedb.IBSONRow*/
    //{


    //    public ReactiveCommand<string> DbOperationCommand { get; }

    //    public InteractiveCollectionViewModel<T, int> CollectionVM { get; }

    //    public IObservable<KeyValuePair<UtilityEnum.Database.Operation, T>> Output { get; }

    //    public LiteDbViewModel(IObservable<IChangeSet<T, int>> repo, IObservable<Func<T,bool>>enabled,UtilityWpf.IDispatcherService ds)
    //    {

    //        Func<T, bool> ft = a => true;

    //        CollectionVM = new InteractiveCollectionViewModel<T, int>(repo, Observable.Repeat(ft, 1, ds.Background),enabled, ds.UI);

    //        DbOperationCommand = new ReactiveCommand<string>(CollectionVM.GetDoubleClicked().Select(_ => _ != null));

    //        DbOperationCommand.Subscribe(_ =>
    //        {
    //            var end =(UtilityEnum.Database.Operation)Enum.Parse(typeof(UtilityEnum.Database.Operation), _);


    //        });

    //        CollectionVM.GetDoubleClicked().Subscribe(_ =>
    //        {
    //           // DbOperationCommand.WithLatestFrom

    //        });

    //        var x = Observable.Create<UtilityEnum.Database.Operation>(observer => DbOperationCommand.Subscribe(_ =>
    //        observer.OnNext((UtilityEnum.Database.Operation)Enum.Parse(typeof(UtilityEnum.Database.Operation), _))))
    //        .Merge(CollectionVM.GetDoubleClicked().Select(_ => UtilityEnum.Database.Operation.Insert));


    //        x.Subscribe(_ =>
    //        { });
    //        //var ty= CollectionVM.Output.WithLatestFrom(DbOperationCommand, (a, b) => new { a, b }).Select(_ =>
    //        //{
    //        //    return "";

    //        //});

    //        Output = CollectionVM.GetDoubleClicked() .WithLatestFrom( x,(a,b)=>new { a, b }).Select(_=>
    //        {

    //            //var end = _.a == null ? Database.Operation.None : _.b;

    //            return new KeyValuePair<UtilityEnum.Database.Operation, T>(_.b, _.a);

    //        });
    //        Output.Subscribe(_ =>
    //        { });

    //        //ty.Subscribe(_ =>
    //        //{ });


    //    }




    }


    //public class LiteDbWrapper<T> where T : UtilityDAL.IBSONRow
    //{


    //    public LiteDbWrapper(IObservable<KeyValuePair<UtilityEnum.Database.Operation, T>> service, UtilityDAL.IDocumentStore<T> repo)
    //    {

    //        service.Subscribe(_ =>
    //        {
    //            var col = repo.GetCollection(out IDisposable disposable);
    //            var items = repo.GetItems(col).Cast<T>().ToList();

    //            switch (_.Key)
    //            {
    //                case (UtilityEnum.Database.Operation.Add):
    //                    repo.Insert(items,_.Value);
    //                    break;
    //                case (UtilityEnum.Database.Operation.Remove):
    //                    //repo.Delete(items,_.Value,_,repo.);
    //                    break;
    //                default:
    //                    break;

    //            }

    //        });

    //    }

    //}





  
//}
