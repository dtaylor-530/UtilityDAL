//using PropertyTools.DataAnnotations;
using System.Windows;
using System.Windows.Controls;

namespace UtilityDAL.View
{
    /// <summary>
    /// Interaction logic for SelectionUserControl.xaml
    /// </summary>
    public partial class CsvSelectionUserControl : UserControl
    {
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register("Directory", typeof(string), typeof(CsvSelectionUserControl));

        //[DirectoryPath]
        //[AutoUpdateText]
        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        public CsvSelectionUserControl()
        {
            InitializeComponent();
            //this.Loaded += SelectionUserControl_Loaded;
            sp.DataContext = this;
        }

        private void SelectionUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //folder.DataContext = new FolderPickerViewModel((string)GetValue(DirectoryProperty));
            //var x = new FolderPickerViewModel((string)GetValue(DirectoryProperty));
            //folder.DataContext = x;

            //x.Output.Subscribe(_ =>
            //{
            //});

            ////var c = new DirectoriesViewModel(, new UtilityWpf.DispatcherService(Application.Current.Dispatcher), "csv", _ => UtilityDAL.CsvHelper.Parse(_));
            ////IObservableService<Model.DataFile> instance = new CsvDataService(x.Output, 4) as IObservableService<Model.DataFile>;
            ////dockpanel.DataContext = new ViewModel.ProviderViewModel(instance.Resource.ToReactiveCollection());

            //maincontent.DataContext = c;
        }
    }
}