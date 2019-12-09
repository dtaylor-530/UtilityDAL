using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace UtilityDAL.View
{
    /// <summary>
    /// Interaction logic for LogUserControl.xaml
    /// </summary>
    public partial class LogUserControl : UserControl
    {
        public LogUserControl()
        {
            InitializeComponent();
        }

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Items.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(IEnumerable), typeof(LogUserControl), new PropertyMetadata(null, fds));

        private static void fds(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LogUserControl).UserControl.Items = e.NewValue as IEnumerable;
        }
    }
}