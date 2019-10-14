using System;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    using Model;
    using System.Reactive.Linq;

    public class CsvDummyDataService<T> : IService<DataFile>
    {
        public IObservable<DataFile> Resource { get; }

        public CsvDummyDataService(string path = null)
        {
            Resource = csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), "csv");
        }

        public CsvDummyDataService(IObservable<string> paths)
        {
            Resource = Observable.Create<IObservable<DataFile>>(observer =>
            paths.Subscribe(path =>
            {
                observer.OnNext(csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV<T>(path), "csv"));
            })).Switch();
        }
    }

    public class CsvDataService : IService<DataFile>
    {
        public IObservable<DataFile> Resource { get; }

        public CsvDataService(string path = null, int? i = null)
        {
            Resource = csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), "csv", i);
        }

        public CsvDataService(TimeSpan ts, string path = null)
        {
            Resource = csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), ts, "csv");
        }

        public CsvDataService(IObservable<string> paths, int? i = null)
        {
            Resource = Observable.Create<IObservable<DataFile>>(observer =>
            paths.Subscribe(path =>
            {
                observer.OnNext(csvHelper.GenerateDataFilesDefault(new UtilityDAL.CSV.CSV(path), "csv", i));
            })).Switch();
        }
    }
}