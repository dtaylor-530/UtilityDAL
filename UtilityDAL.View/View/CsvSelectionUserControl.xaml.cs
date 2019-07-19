using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
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
using UtilityWpf.ViewModel;
using UtilityDAL;
using System.Reactive.Linq;
using UtilityWpf;
using PropertyTools.DataAnnotations;

namespace UtilityDAL.View
{
    /// <summary>
    /// Interaction logic for SelectionUserControl.xaml
    /// </summary>
    public partial class CsvSelectionUserControl : UserControl
    {

        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register("Directory", typeof(string), typeof(CsvSelectionUserControl));

        [DirectoryPath]
        [AutoUpdateText]
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
            ////IService<Model.DataFile> instance = new CsvDataService(x.Output, 4) as IService<Model.DataFile>;
            ////dockpanel.DataContext = new ViewModel.ProviderViewModel(instance.Resource.ToReactiveCollection());

            //maincontent.DataContext = c;
        }



    }



}