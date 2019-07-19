using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL;
using UtilityDAL.Contract;
using UtilityHelper;
using UtilityInterface.Generic.Database;
using UtilityInterface.NonGeneric.Database;
using UtilityWpf;
using UtilityWpf.View;
using UtilityWpf.ViewModel;

namespace UtilityDAL.View
{
    public static class AttachedDocumentStore //: MultiSelectTreeView//where T : new()
    {

        public static void SHDStoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _docstore = (IDbService<SHDOObject, object>)e.NewValue;
            ReflectChanges(d as ListBoxEx, _docstore);

        }


        internal static object StoreCoerce(DependencyObject d, object baseValue)
        {
            var _docstore = (IDbService<SHDOObject, object>)baseValue;
            ReflectChanges(d as ListBoxEx, _docstore);
            return baseValue;
        }

        internal static object StoreCoerce2(DependencyObject d, object baseValue)
        {
            var _docstore = (IDbService)baseValue;
            ReflectChanges(d as ItemsControl, _docstore);
            return baseValue;
        }



        public static void ReflectChanges(ListBoxEx lbx, IDbService<SHDOObject, object> _docstore)
        {
            var lst = lbx.ItemsSource?.Cast<SHDOObject>()?.ToList();
            IDisposable disposable = null;
            if (lst != null)
            {
                lst.AddRange(_docstore.SelectAll());
                lbx.ItemsSource = lst;
                disposable.Dispose();
            }

            lbx.Changes
                 .Subscribe(_ =>
                 {
                     var item = new SHDOObject(_.Key.Object);
                     //var k = _.Key.Object.GetPropertyValue<string>(lbx.Key);
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
                     disposable?.Dispose();
                 });
        }

        public static void ReflectChanges(ItemsControl itemscontrol, IDbService _docstore)
        {

            var key = (string)itemscontrol.GetValue(Ex.KeyProperty);
            var lst = itemscontrol.ItemsSource?.Cast<object>()?.ToList();
            IDisposable disposable = null;
            if (lst != null)
            {
                lst.AddRange(_docstore.SelectAll().Cast<object>());
                itemscontrol.ItemsSource = lst;
                disposable.Dispose();
            }
            
            (itemscontrol.ItemsSource as System.Collections.ObjectModel.ObservableCollection<object>)?
              .GetCollectionChanges()
               .Subscribe(_ =>
               {
                   var reason = _.Action;
                   var newitems = _.NewItems;// new SHDOObject(_.Key.Object);
                   var olditems = _.OldItems;

                   foreach (var item in newitems.Cast<object>().Concat(olditems.Cast<object>()))
                   {
                       var k = item.GetPropertyValue<string>(key);

                       if (reason == NotifyCollectionChangedAction.Add)
                       {
                           _docstore.Upsert(item, k);
                           //this.Dispatcher.InvokeAsync(() => ItemsSource = interactivecollection.Items, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
                       }
                       else if (reason == NotifyCollectionChangedAction.Remove)
                       {

                           _docstore.Delete(item);
                       }
                       //else if (reason == ChangeReason.Update)
                       //{
                       //    var clx = _docstore.GetCollection(out disposable);
                       //    var items = _docstore.GetItems(clx);
                       //    _docstore.Upsert(item, lbx.Key);
                       //}
                   }
                   disposable?.Dispose();
               });
        }

        public static void StoreChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {


        }

        //public ISubject<IDbService<SHDOObject,object>> StoreSubject = new Subject<IDbService<SHDOObject,object>>();

    }
}
