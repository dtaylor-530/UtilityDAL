//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reactive.Linq;
//using System.Reactive.Subjects;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using UtilityDAL;
//using UtilityHelper.NonGeneric;
//using UtilityWpf.View;

//namespace UtilityDAL.View
//{
//    //public abstract class DocumentStoreControl<T> : DocumentStoreControl
//    //{
//    //    public DocumentStoreControl(IDocumentStore<T> docstore, Func<object, IConvertible> getKey):base(docstore,getKey)
//    //    {
//    //    }

//    //}
//    public abstract class DocumentStoreControl : UtilityWpf.View.CollectionEditor
//    {
//        //public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register("Directory", typeof(string), typeof(DocumentStoreControl<T>));

//        ////public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(DocumentStoreControl<T>));
//        //static void DatabaseCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DocumentStoreControl<T>).DatabaseCommandsSubject.OnNext((UtilityWpf.View.DatabaseCommand)e.NewValue);

//        protected IDocumentStore _docstore;
//        Func<object, IConvertible> _getKey = null;

//        public DocumentStoreControl(IDocumentStore docstore,Func<object,IConvertible> getKey=null)
//        {
//            _docstore = docstore;
//            _getKey = getKey;

//            var clxnitems = _docstore.GetItems(_docstore.GetCollection(out IDisposable disposable)).Cast<Object>();
//            if (clxnitems.Count() > 0)
//            {
//                /*   this.Dispatcher.InvokeAsync(() =>*/
//                ItemsSourceSubject.OnNext(clxnitems);/*, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));*/

//                //ItemsSource = items;
//            }
//            disposable.Dispose();
//            //NewItemsSubject.Subscribe(_ =>
//            //{ });
//            Observable.FromEventPattern<EventHandler, EventArgs>(_ => this.Initialized += _, _ => this.Initialized -= _)
//                .CombineLatest(ItemsSourceSubject, (a, b) => b)
//                   .CombineLatest(KeySubject, (item, key) => new { item, key })
//                .Subscribe(_ => React(_.item, _.key));

//            InputSubject.Subscribe(_ =>
//            {
//                var customers = _docstore.GetCollection(out disposable);
//                switch (_)
//                {
//                    case (DatabaseCommand.Delete):
//                        _docstore.Delete(customers, SelectedItem, Key);
//                        break;
//                    case (DatabaseCommand.Update):
//                        _docstore.Update(customers, SelectedItem);
//                        break;
//                }
//                disposable.Dispose();
//            });
//        }

//        protected void React(IEnumerable _b, string key)
//        {
//            foreach (var _c in _b)
//            {
//                _docstore.Upsert(_b, key);
//            }

//            var clxnitems = _docstore.GetItems(_docstore.GetCollection(out IDisposable disposable));
//            if (clxnitems != null)
//            {
//                /*this.Dispatcher.InvokeAsync(() =>*/
//                ItemsSourceSubject.OnNext(clxnitems);/*, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));*/
//            }

//            disposable.Dispose();
//        }

//        public override object GetKey(object trade) => _getKey(trade);

//        //private object GetCollection(out IDisposable disposable)
//        //{
//        //    return _docstore.GetCollection(out disposable);
//        //}

//        //private IEnumerable GetItems(object repo)
//        //{
//        //    return _docstore.GetItems(repo);
//        //}

//        //private object FindById(object items, T item, string key)
//        //{
//        //    return _docstore.FindById(items, item, key);
//        //}

//        //private bool Insert(object items, T item)
//        //{
//        //    return _docstore.Insert(items, item);
//        //}

//        //private bool Update(object items, T item)
//        //{
//        //    return _docstore.Update(items, item);
//        //}

//        //private bool Delete(object items, T item, string key)
//        //{
//        //    return _docstore.Delete(items, item, key);
//        //}

//    }

//    public abstract class DocumentStoreControl<T> : UtilityWpf.View.CollectionEditor
//    {
//        //public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register("Directory", typeof(string), typeof(DocumentStoreControl<T>));

//        ////public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(DocumentStoreControl<T>));
//        //static void DatabaseCommandChange(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as DocumentStoreControl<T>).DatabaseCommandsSubject.OnNext((UtilityWpf.View.DatabaseCommand)e.NewValue);

//        protected IDocumentStore<T> _docstore;
//        Func<T, object> _getKey = null;

//        public DocumentStoreControl(IDocumentStore<T> docstore, Func<T, object> getKey)
//        {
//            _docstore = docstore;
//            _getKey = getKey;

//            var clxnitems = _docstore.GetItems(_docstore.GetCollection(out IDisposable disposable)).Cast<Object>();
//            if (clxnitems.Count() > 0)
//            {
//                /*   this.Dispatcher.InvokeAsync(() =>*/
//                ItemsSourceSubject.OnNext(clxnitems);/*, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));*/

//                //ItemsSource = items;
//            }
//            disposable.Dispose();
//            //NewItemsSubject.Subscribe(_ =>
//            //{ });
//            //Observable.FromEventPattern<EventHandler, EventArgs>(_ => this.Initialized += _, _ => this.Initialized -= _)
//            //    .CombineLatest(NewItemsSubject, (a, b) => b)
//            //       .CombineLatest(KeySubject, (item, key) => new { item, key })
//            //    .Subscribe(_ => React(_.item, _.key));

//            InputSubject.Subscribe(_ =>
//            {
//                var customers = _docstore.GetCollection(out disposable);
//                switch (_)
//                {
//                    case (DatabaseCommand.Delete):
//                        _docstore.Delete(customers, (T)SelectedItem, Key);
//                        break;
//                    case (DatabaseCommand.Update):
//                        _docstore.Update(customers, (T)SelectedItem);
//                        break;
//                }
//                disposable.Dispose();
//            });
//        }

//        //protected override void React(object _b, string key)
//        //{
//        //    _docstore.Upsert((T)_b, key);

//        //    var clxnitems = _docstore.GetItems(_docstore.GetCollection(out IDisposable disposable));
//        //    if (clxnitems != null)
//        //    {
//        //        /*this.Dispatcher.InvokeAsync(() =>*/
//        //        ItemsSourceSubject.OnNext(clxnitems);/*, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));*/
//        //    }

//        //    disposable.Dispose();
//        //}

//        public override object GetKey(object trade)
//        {
//            return _getKey((T)trade);
//        }

//    }

//}