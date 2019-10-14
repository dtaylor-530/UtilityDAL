using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

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
            this.FileViewer.PropertyGroupDescription = new PropertyGroupDescription(nameof(UtilityWpf.ViewModel.PathViewModel.Directory)) { Converter = new NameConverter() };
        }

        private class NameConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return System.IO.Path.GetFileName((string)value);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                throw new NotImplementedException();
            }
        }
    }
}