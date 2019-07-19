
using System.Windows;
using UtilityWpf.View;
using UtilityWpf.ViewModel;
using UtilityDAL.Contract;
using UtilityInterface.Generic.Database;

namespace UtilityDAL.View
{

    public class ListBoxExDocumentStore: ListBoxEx
    {
        public static string GetStore(DependencyObject obj)
        {
            return (string)obj.GetValue(StoreProperty);
        }

        public static void SetStore(DependencyObject obj, string value)
        {
            obj.SetValue(StoreProperty, value);
        }

        public static readonly DependencyProperty StoreProperty = DependencyProperty.RegisterAttached("Store", typeof(IDbService<SHDOObject, object>), typeof(ListBoxExDocumentStore), new PropertyMetadata(null,null, AttachedDocumentStore.StoreCoerce));

    }


}
