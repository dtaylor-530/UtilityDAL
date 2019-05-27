using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityDAL;
using UtilityDAL.Contract;
using UtilityInterface;
using UtilityWpf;
using UtilityWpf.View;
using UtilityWpf.ViewModel;

namespace UtilityDAL.View
{


    public class InputOutputControlRepo<R>:InputOutputControl<IConvertible,R>
    {


        public InputOutputControlRepo(IFunction<IConvertible, R> service,Func<IObservable<IConvertible>, IObservable<IConvertible>> func = null):base(service,func)
        {
            //ldbr = new LiteDbRepo<DbStoreKeyValuePair, T>(_ => _.Key, "../../Data/io.lite");
            ldbr = new XmlSaveLoadService<DbStoreKeyValuePair>("Key", "../../Data"); 
            //var mapper = BsonMapper.Global;

            //mapper.Entity<DbStoreKeyValuePair>()
            //    .Id(x => x.Key, true); // set your document ID

        }

        private XmlSaveLoadService<DbStoreKeyValuePair> ldbr;

        public static readonly DependencyProperty StoreProperty = DependencyProperty.Register("Store", typeof(IDbService), typeof(InputOutputControlRepo<R>));

        protected override void Init(IFunction<IConvertible, R> service)
        {
            InputChanges.Subscribe(_ =>
            {
                //R plvms = default(R);
                var x = (ldbr.Find(_.ToString()));

                this.Dispatcher.InvokeAsync(() =>
                {
                    if (x != null)
                    {
                        this.SetValue(StoreProperty, x.Db);
                        this.SetValue(OutputProperty, x.Db.FindAll());
                    }
                    else
                    {
                        var db = new LiteDbRepo("Object", "../../Data/"+_.ToString()+ "tree.lite");
                        this.SetValue(StoreProperty, db);
                        ldbr.Save(new[] { new DbStoreKeyValuePair { Key = _, Db = db } });
                        this.SetValue(OutputProperty, service.Function(_));
                    }
                }, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
            });
        }

        class DbStoreKeyValuePair
        {
            public IConvertible Key { get; set; }
            public IDbService Db { get; set; }
        }
    }
}
