using System.Windows;
using UtilityInterface.Generic.Database;
using UtilityWpf.View;
using UtilityWpf.ViewModel;

namespace UtilityDAL.View
{
    public class ListBoxExDocumentStore : ListBoxEx
    {
        public static string GetStore(DependencyObject obj)
        {
            return (string)obj.GetValue(StoreProperty);
        }

        public static void SetStore(DependencyObject obj, string value)
        {
            obj.SetValue(StoreProperty, value);
        }

        public static readonly DependencyProperty StoreProperty = DependencyProperty.RegisterAttached("Store", typeof(IDatabaseService<SHDOObject, object>), typeof(ListBoxExDocumentStore), new PropertyMetadata(null, null, AttachedDocumentStore.StoreCoerce));
    }
}