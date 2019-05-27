using UtilityDAL.ViewModel;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityDAL.Model;
using UtilityDAL.Contract;

namespace UtilityDAL.DemoApp

{
    public class RepoChangeService : IRepoChangeService<DummyDbObject>
    {
        private IDbService<DummyDbObject,int> _repo;

        public IObservable<IChangeSet<DummyDbObject, int>> Resource { get; set; }


        public RepoChangeService(UtilityWpf.IDispatcherService service)
        {

            _repo = new UtilityDAL.LiteDbRepo<DummyDbObject,int>(_=>_.Id,"dummylitedb.db");

            var items = _repo.FindAll();
            int i = 0;
           
            try { i = items.Max(_ => _.Id); } catch { }

            Observable.Interval(TimeSpan.FromSeconds(3)).Take(2).Subscribe(_ =>
            {
                i++;
                _repo.Insert(new DummyDbObject { Id = i, Name = i.ToString() });
            });


            //var mon = new Monitor<DummyDbObject>(_repo, service.Background);

            //Resource = mon.Resource;
        }


        public void CallBack(IObservable<KeyValuePair<UtilityEnum.Database.Operation, DummyDbObject>> ops)
        {
            //var xx = new LiteDbWrapper<DummyDbObject>(ops, _repo);

        }


    }



}
