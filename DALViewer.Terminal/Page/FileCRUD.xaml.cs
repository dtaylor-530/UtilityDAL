using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
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
using UtilityDAL.Model;
using UtilityDAL.Contract;

namespace UtilityDAL.DemoApp
{
    /// <summary>
    /// Interaction logic for KeyValueDate.xaml
    /// </summary>
    public partial class Filex : Page
    {
        public Filex()
        {
            InitializeComponent();
            //this.DataContext = new ViewModel.Filex();
            this.DataContext = new FilexViewModel();
        }

        class FilexViewModel
        {  
                private static readonly System.IO.DirectoryInfo directory = System.IO.Directory.CreateDirectory(directoryPath);
             //    private ReadOnlyObservableCollection<UtilityDAL.Entity.KeyValueDate> database;
                const string directoryPath = "/FilextData";


            public IFileDbService DbService { get; } = new UtilityDAL.TeatimeFileService<Filext>(@"../../Data");
            public IFileDbService DbService3 { get; } = new UtilityDAL.CSV3(@"../../Data");

            public ObservableCollection<Model.Filex> Items { get; }=Observable.Interval(TimeSpan.FromSeconds(3))
                .StartWith(0)
                .Select(_ => UtilityDAL.Factory.Filex.Create(System.IO.Path.Combine(directory.FullName, UtilityHelper.RandomHelper.NextWord())))
                .ToReactiveCollection();

            public ObservableCollection<Filext> Items2 { get; } = Observable.Interval(TimeSpan.FromSeconds(3))
    .StartWith(0)
    .Select(_ => UtilityDAL.Factory.FilextFactory.Create(System.IO.Path.Combine(directory.FullName, UtilityHelper.RandomHelper.NextWord())))
    .ToReactiveCollection();
        }

        private void FileCRUDControl_SeriesRetrieved(object sender, RoutedEventArgs e)
        {

        }
    }



}
