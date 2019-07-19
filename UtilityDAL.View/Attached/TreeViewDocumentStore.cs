using DynamicData;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL.Contract;
using UtilityHelper;
using UtilityInterface.Generic.Database;
using UtilityWpf.View;
using UtilityWpf.ViewModel;

namespace UtilityDAL.View
{
    public class TreeViewDocumentStore : TreeView
    {
        //private Func<object, object> _getKey;
        //private IDocumentStore<SHDOObject> _docstore;

        public static string GetStore(DependencyObject obj)
        {
            return (string)obj.GetValue(StoreProperty);
        }

        public static void SetStore(DependencyObject obj, string value)
        {
            obj.SetValue(StoreProperty, value);
        }


        public static readonly DependencyProperty StoreProperty = DependencyProperty.RegisterAttached("Store", typeof(IDbService<SHDOObject,object>), typeof(TreeViewDocumentStore), new PropertyMetadata(null, null, AttachedDocumentStore.StoreCoerce2));

      

    
    }

}
