using System;
using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace UtilityDAL.View
{
    public class PageView : Control, IItemsSource, UtilityWpf.Abstract.IObject
    {
        static PageView()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PageView), new FrameworkPropertyMetadata(typeof(PageView)));
        }

        public IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable), typeof(PageView), new PropertyMetadata(null));

        public object Object
        {
            get { return (object)GetValue(ObjectProperty); }
            set { SetValue(ObjectProperty, value); }
        }

        public static readonly DependencyProperty ObjectProperty = DependencyProperty.Register("Object", typeof(object), typeof(PageView), new PropertyMetadata(null, Changed));

        private static void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as PageView).ItemsSource = (e.NewValue as IEnumerable);
        }
    }
}