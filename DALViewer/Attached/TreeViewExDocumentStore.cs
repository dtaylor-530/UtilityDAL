using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityDAL.Contract;
using UtilityWpf.View;
using UtilityWpf.ViewModel;

namespace UtilityDAL.View.Attached
{
    public class TreeViewExDocumentStore : MultiSelectTreeView
    {
        public static string GetStore(DependencyObject obj)
        {
            return (string)obj.GetValue(StoreProperty);
        }

        public static void SetStore(DependencyObject obj, string value)
        {
            obj.SetValue(StoreProperty, value);
        }

        public static readonly DependencyProperty StoreProperty = DependencyProperty.RegisterAttached("Store", typeof(IDbService<SHDOObject, object>), typeof(TreeViewExDocumentStore), new PropertyMetadata(null, null,AttachedDocumentStore.StoreCoerce));

    }
}
