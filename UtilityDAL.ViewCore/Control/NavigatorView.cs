using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace UtilityDAL.View
{
    public class NavigatorView : Control, IItemsSource
    {
        static NavigatorView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(NavigatorView), new FrameworkPropertyMetadata(typeof(NavigatorView)));
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(NavigatorView), new PropertyMetadata(null));
    }
}