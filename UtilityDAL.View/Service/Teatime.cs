using System;
using System.Reactive.Linq;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    using Model;

    public class TeaDataDummyService<T> : IService<DataFile> where T : struct
    {
        public IObservable<DataFile> Resource { get; }

        public TeaDataDummyService(string path = null)
        {
            Resource = FileGenerator.GenerateDataFilesDefault(new Teatime.Repository<T>(path), "tea");
        }

        public TeaDataDummyService(IObservable<string> paths)
        {
            paths.Subscribe(_ =>
            {
            });
            Resource = Observable.Create<IObservable<DataFile>>(observer =>
              paths.Subscribe(path =>
              {
                  observer.OnNext(FileGenerator.GenerateDataFilesDefault(new Teatime.Repository<T>(path), "tea"));
              })).Switch();
        }
    }
}