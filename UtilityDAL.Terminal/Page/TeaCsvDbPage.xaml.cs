using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace UtilityDAL.DemoApp
{
    /// <summary>
    /// Interaction logic for DbUserControl.xaml
    /// </summary>
    public partial class DbPage : Page
    {
        public DbPage()
        {
            InitializeComponent();
            this.FileViewer.PropertyGroupDescription = new PropertyGroupDescription(nameof(UtilityWpf.ViewModel.PathViewModel.Directory)) { Converter = new Converter() };
        }

        private class Converter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                string val = (string)value;
                return val;
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}
