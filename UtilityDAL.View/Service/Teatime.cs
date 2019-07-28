using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using UtilityInterface.Generic;

namespace UtilityDAL.View
{

    using Model;
    using System.Reactive.Disposables;
    using UtilityDAL;
    using UtilityInterface;
    using UtilityWpf.ViewModel;

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
           Resource=Observable.Create<IObservable<DataFile>>(observer =>
           paths.Subscribe(path =>
           {
               observer.OnNext(FileGenerator.GenerateDataFilesDefault(new Teatime.Repository<T>(path), "tea"));
           })).Switch();
        }

    }

}







