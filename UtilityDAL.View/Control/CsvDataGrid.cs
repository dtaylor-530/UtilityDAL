using GenericParsing;
using System;
using System.Collections;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;

namespace UtilityDAL.View
{
    public class CsvDataGrid : DataGrid
    {
        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(CsvDataGrid), new PropertyMetadata(null, PathChanged));

        private static void PathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as CsvDataGrid).PathChangeSubject.OnNext((string)e.NewValue);
        }

        private ISubject<string> PathChangeSubject = new Subject<string>();

        static CsvDataGrid()
        {
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(CsvDataGrid), new FrameworkPropertyMetadata(typeof(CsvDataGrid)));
        }

        //public IEnumerable ItemsSink
        //{
        //    get { return (IEnumerable)GetValue(ItemsSinkProperty); }
        //    set { SetValue(ItemsSinkProperty, value); }
        //}

        // Using a DependencyProperty as the backing store for ItemsSink.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty ItemsSinkProperty =
        //    DependencyProperty.Register("ItemsSink", typeof(IEnumerable), typeof(CsvDataGrid), new PropertyMetadata(null));

        public CsvDataGrid()
        {
            //Uri resourceLocater = new Uri("/UtilityWpf.View;component/Themes/CsvDataGrid.xaml", System.UriKind.Relative);
            //ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            //Style = resourceDictionary["CsvDataGridStyle"] as Style;

            PathChangeSubject.DistinctUntilChanged()
            .Subscribe(_ =>
            {
                this.Dispatcher.InvokeAsync(() => this.SetValue(ItemsSourceProperty, CsvHelper.FromCsv(_)), System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken));
            });
        }

        private static class CsvHelper
        {
            public static ICollection FromCsv(String name, string path = "")
            {
                var text = System.IO.Path.Combine(path, name.Replace(".csv", "") + ".csv");
                // Using an XML Config file.
                using (GenericParserAdapter parser = new GenericParserAdapter(text))
                {
                    parser.ColumnDelimiter = ',';
                    parser.FirstRowHasHeader = true;
                    //parser.SkipStartingPathRows = 10;
                    parser.MaxBufferSize = 4096;
                    //parser.MaxRows = 500;
                    parser.TextQualifier = '\"';

                    //  parser.Load("MyPath.xml");
                    var yt = parser.GetDataTable().DefaultView;
                    return yt;
                }
            }
        }
    }
}