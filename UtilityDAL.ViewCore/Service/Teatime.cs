using System;
using System.Reactive.Linq;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    using Model;

    public class TeaDataDummyService<T> : IObservableService<KeyCollection> where T : struct
    {
        public IObservable<KeyCollection> Observable { get; }

        public TeaDataDummyService(string path = null)
        {
            Observable = FileGenerator.GenerateDataFilesDefault(new Teatime.Repository<T>(path), "tea");
        }

        public TeaDataDummyService(IObservable<string> paths)
        {
            paths.Subscribe(_ =>
            {
            });
            Observable = System.Reactive.Linq.Observable.Create<IObservable<KeyCollection>>(observer =>
              paths.Subscribe(path =>
              {
                  observer.OnNext(FileGenerator.GenerateDataFilesDefault(new Teatime.Repository<T>(path), "tea"));
              })).Switch();
        }
    }
}