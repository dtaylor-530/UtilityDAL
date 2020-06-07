﻿using System;
using System.Reactive.Linq;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{
    using Model;

    public class TeaDataDummyService<T> : IObservableService<DataFile> where T : struct
    {
        public IObservable<DataFile> Service { get; }

        public TeaDataDummyService(string path = null)
        {
            Service = FileGenerator.GenerateDataFilesDefault(new Teatime.Repository<T>(path), "tea");
        }

        public TeaDataDummyService(IObservable<string> paths)
        {
            paths.Subscribe(_ =>
            {
            });
            Service = Observable.Create<IObservable<DataFile>>(observer =>
              paths.Subscribe(path =>
              {
                  observer.OnNext(FileGenerator.GenerateDataFilesDefault(new Teatime.Repository<T>(path), "tea"));
              })).Switch();
        }
    }
}