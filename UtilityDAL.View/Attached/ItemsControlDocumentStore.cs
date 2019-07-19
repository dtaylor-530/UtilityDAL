using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL;
using UtilityHelper;
using UtilityWpf.View;
using UtilityWpf.ViewModel;
using UtilityDAL.Contract;
using UtilityInterface.NonGeneric.Database;

namespace UtilityDAL.View
{
    public class ItemsControlDocumentStore : ItemsControl//where T : new()
    {

        public static string GetStore(DependencyObject obj)
        {
            return (string)obj.GetValue(StoreProperty);
        }

        public static void SetStore(DependencyObject obj, string value)
        {
            obj.SetValue(StoreProperty, value);
        }


        public static readonly DependencyProperty StoreProperty = DependencyProperty.RegisterAttached("Store", typeof(IDbService), typeof(ItemsControlDocumentStore), new PropertyMetadata(null, null, AttachedDocumentStore.StoreCoerce2));





    }

    


}
