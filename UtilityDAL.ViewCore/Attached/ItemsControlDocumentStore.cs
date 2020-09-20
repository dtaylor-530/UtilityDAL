using System.Windows;
using System.Windows.Controls;
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

        public static readonly DependencyProperty StoreProperty = DependencyProperty.RegisterAttached("Store", typeof(IDatabaseService), typeof(ItemsControlDocumentStore), new PropertyMetadata(null, null, AttachedDocumentStore.StoreCoerce2));
    }
}