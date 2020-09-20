using System;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    using Model;
    using System.Reactive.Linq;

    public class CsvDummyDataService<T> : IObservableService<KeyCollection>
    {
        public IObservable<KeyCollection> Observable { get; }

        public CsvDummyDataService(string path = null)
        {
            Observable = csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), "csv");
        }

        public CsvDummyDataService(IObservable<string> paths)
        {
            Observable = System.Reactive.Linq.Observable.Create<IObservable<KeyCollection>>(observer =>
            paths.Subscribe(path =>
            {
                observer.OnNext(csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV<T>(path), "csv"));
            })).Switch();
        }
    }

    public class CsvDataService : IObservableService<KeyCollection>
    {
        public IObservable<KeyCollection> Observable { get; }

        public CsvDataService(string path = null, int? i = null)
        {
            Observable = csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), "csv", i);
        }

        public CsvDataService(TimeSpan ts, string path = null)
        {
            Observable = csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), ts, "csv");
        }

        public CsvDataService(IObservable<string> paths, int? i = null)
        {
            Observable = System.Reactive.Linq.Observable.Create<IObservable<KeyCollection>>(observer =>
            paths.Subscribe(path =>
            {
                observer.OnNext(csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), "csv", i));
            })).Switch();
        }
    }
}