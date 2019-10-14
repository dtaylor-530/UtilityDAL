using Reactive.Bindings;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL.Contract.NonGeneric;

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

        private class FilexViewModel
        {
            private static readonly System.IO.DirectoryInfo directory = System.IO.Directory.CreateDirectory(directoryPath);

            //    private ReadOnlyObservableCollection<UtilityDAL.Entity.KeyValueDate> database;
            private const string directoryPath = "/FilextData";

            //public IFileDbService DbService { get; } = new UtilityDAL.Teatime.FileService<Filext>(@"../../Data");
            public IFileDbService DbService3 { get; } = new UtilityDAL.CSV.CSV(@"../../Data");

            public ObservableCollection<Model.Filex> Items { get; } = Observable.Interval(TimeSpan.FromSeconds(3))
                .StartWith(0)
                .Select(_ => UtilityDAL.Factory.Filex.Create(System.IO.Path.Combine(directory.FullName, UtilityHelper.RandomHelper.NextWord())))
                .ToReactiveCollection();

            //        public ObservableCollection<Filext> Items2 { get; } = Observable.Interval(TimeSpan.FromSeconds(3))
            //.StartWith(0)
            //.Select(_ => UtilityDAL.FilextFactory.Create(System.IO.Path.Combine(directory.FullName, UtilityHelper.RandomHelper.NextWord())))
            //.ToReactiveCollection();
            //    }

            private void FileCRUDControl_SeriesRetrieved(object sender, RoutedEventArgs e)
            {
            }
        }

        private void FileCRUDControl_SeriesRetrieved(object sender, RoutedEventArgs e)
        {
        }
    }
}