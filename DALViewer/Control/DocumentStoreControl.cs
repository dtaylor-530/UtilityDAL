using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityDAL;
using UtilityDAL.Contract;
using UtilityInterface;
using UtilityWpf.View;
using UtilityWpf.ViewModel;

namespace UtilityDAL.View
{

    public class DocumentStoreControl<T> : ListBoxEx//where T : new()
    {
        //private Func<object, object> _getKey;
        private IDbService<SHDOObject, IConvertible> _docstore;

        public static string GetIsStore(DependencyObject obj)
        {
            return (string)obj.GetValue(IsStoreProperty);
        }

        public static void SetIsStore(DependencyObject obj, string value)
        {
            obj.SetValue(IsStoreProperty, value);
        }

        private static readonly bool isStore = true;
        public static readonly DependencyProperty IsStoreProperty = DependencyProperty.Register("IsStore", typeof(bool), typeof(DocumentStoreControl<T>), new PropertyMetadata(isStore, IsStoreChanged));

        private static void IsStoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DocumentStoreControl<T>).IsStoreSubject.OnNext((bool)e.NewValue);
        }
        ISubject<bool> IsStoreSubject = new Subject<bool>();

        public DocumentStoreControl(IDbService<SHDOObject, IConvertible> docstore, Func<object, object> getKey = null) : base(getKey)
        {

            //_getKey = getKey ?? base.GetKey;
            _docstore = docstore;
            this.ItemsSource = _docstore.FindAll();

            Changes.Subscribe(_ =>
            {

            });
            //IsStoreSubject.Subscribe(_ =>
            //{

            //});

            base.Changes.WithLatestFrom(IsStoreSubject.StartWith(isStore).DistinctUntilChanged(), (a, b) => b ? a : default(KeyValuePair<IContainer<object>, ChangeReason>))
                //.Where(_ => !_.Equals(default(KeyValuePair<IContainer<object>, ChangeReason>)))
                .Subscribe(_ =>
                {
                    if (_.Equals(null))
                        return;
                    //var item = new SHDObject<T>((T)_.Key.Object);
        

                    if (_.Key is SHDObject<T>)
                    {
                        var key = (SHDObject<T>)_.Key;
                        var item = new SHDOObject(key.Object, key.IsExpanded, key.IsSelected, key.IsChecked, key.IsVisible, key.IsEnabled,false, key.Id);

                        if (_.Value == ChangeReason.Add)
                        {
                            _docstore.Upsert(item);
                            //this.Dispatcher.InvokeAsync(() => ItemsSource = interactivecollection.Items, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
                        }
                        else if (_.Value == ChangeReason.Remove)
                        {
                            //var clx = _docstore.GetCollection(out disposable);
                            //var items = _docstore.GetItems(clx);
                            _docstore.Delete(item);
                        }
                        else if (_.Value == ChangeReason.Update)
                        {
                            _docstore.Upsert(item);
                        }
                    }
                    else
                    {
                        //var item2 = (SHDObject<object>)_.Key;
                        //var item3 = new SHDObject<T>((T)item2.Object, item2.IsExpanded, item2.IsSelected, item2.IsChecked, item2.IsVisible, item2.IsEnabled);

                        //if (_.Value == ChangeReason.Add)
                        //{
                        //    _docstore.Upsert<SHDObject<T>, IConvertible>(item3);
                        //    //this.Dispatcher.InvokeAsync(() => ItemsSource = interactivecollection.Items, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
                        //}
                        //else if (_.Value == ChangeReason.Remove)
                        //{
                        //    //var clx = _docstore.GetCollection(out disposable);
                        //    //var items = _docstore.GetItems(clx);
                        //    _docstore.Delete(item3);
                        //}
                        //else if (_.Value == ChangeReason.Update)
                        //{
                        //    //_docstore.Upsert(item3);
                        //}
                    }

                });
        }


        //public override object GetKey(object trade) => _getKey((object)trade);



    }


    public class DocumentStoreControl : ListBoxEx//where T : new()
    {
        //private Func<object, object> _getKey;
        private IDbService<SHDOObject, IConvertible> _docstore;

        public static string GetIsStore(DependencyObject obj)
        {
            return (string)obj.GetValue(IsStoreProperty);
        }

        public static void SetIsStore(DependencyObject obj, string value)
        {
            obj.SetValue(IsStoreProperty, value);
        }


        public static readonly DependencyProperty IsStoreProperty = DependencyProperty.Register("IsStore", typeof(bool), typeof(DocumentStoreControl), new PropertyMetadata(true, IsStoreChanged));

        private static void IsStoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as DocumentStoreControl).IsStoreSubject.OnNext((bool)e.NewValue);
        }
        ISubject<bool> IsStoreSubject = new Subject<bool>();

        public DocumentStoreControl(IDbService<SHDOObject, IConvertible> docstore, Func<object, object> getKey) : base(getKey)
        {
            _docstore = docstore;
            //Init((bool)this.GetValue(IsStoreProperty));
            //var isStore = (bool)this.GetValue(IsStoreProperty);
            var all = _docstore.FindAll();
            this.ItemsSource = all;
            var isStore = (bool)this.GetValue(IsStoreProperty);
            base.Changes.WithLatestFrom(IsStoreSubject.StartWith(isStore), (a, b) => b ? a : default(KeyValuePair<IContainer<object>, ChangeReason>))
                .Where(_ => !_.Equals(default(KeyValuePair<IContainer<object>, ChangeReason>)))
                .Subscribe(_ =>
                {
                    var key = (SHDObject<object>)_.Key;
                    var item = new SHDOObject(key.Object, key.IsExpanded, key.IsSelected, key.IsChecked, key.IsVisible, key.IsEnabled,false, key.Id);
                    if (_.Value == ChangeReason.Add)
                    {
                        _docstore.Upsert(item);
                        //this.Dispatcher.InvokeAsync(() => ItemsSource = interactivecollection.Items, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
                    }
                    else if (_.Value == ChangeReason.Remove)
                    {
                        _docstore.Delete(item);
                    }
                    else if (_.Value == ChangeReason.Update)
                    {
                        _docstore.Upsert(item);
                    }

                });
        }

        public DocumentStoreControl(IDbService<SHDOObject, IConvertible> docstore) : base()
        {

            //_getKey = getKey ?? base.GetKey;
            _docstore = docstore;
        //    Init((bool)this.GetValue(IsStoreProperty));
        //}


        //private void Init(bool isStore)
        //{
            var all = _docstore.FindAll();
            this.ItemsSource = all;
            var isStore= (bool)this.GetValue(IsStoreProperty);
            base.Changes.WithLatestFrom(IsStoreSubject.StartWith(isStore), (a, b) => b ? a : default(KeyValuePair<IContainer<object>, ChangeReason>))
                .Where(_ => !_.Equals(default(KeyValuePair<IContainer<object>, ChangeReason>)))
                .Subscribe(_ =>
                {
                var key = (SHDObject<object>)_.Key;
                    var item = new SHDOObject(key.Object,key.IsExpanded,key.IsSelected,key.IsChecked,key.IsVisible,key.IsEnabled,false,key.Id);
                    if (_.Value == ChangeReason.Add)
                    {
                        _docstore.Upsert(item);
                        //this.Dispatcher.InvokeAsync(() => ItemsSource = interactivecollection.Items, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
                    }
                    else if (_.Value == ChangeReason.Remove)
                    {
                        _docstore.Delete(item);
                    }
                    else if (_.Value == ChangeReason.Update)
                    {
                        _docstore.Upsert(item);
                    }

                });
        }



    }

}
