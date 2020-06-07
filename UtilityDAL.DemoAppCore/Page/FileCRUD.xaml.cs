using DynamicData;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL.Contract.NonGeneric;
using UtilityDAL.Model;
using Zio.FileSystems;

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
            private ReadOnlyObservableCollection<Zio.FileEntry> items;
            private const string directoryPath = "/FilextData";

            public FilexViewModel()
            {
                var xx = new MemoryFileSystem();
                xx.CreateDirectory("/Mem");

                Observable.Interval(TimeSpan.FromSeconds(3))
        .StartWith(0)
        .ObserveOnDispatcher()
        .Select(_ =>new Zio.FileEntry(xx,System.IO.Path.Combine("/Mem", UtilityHelper.RandomHelper.NextWord())))
   .ToObservableChangeSet()
        .Bind(out items)
        .Subscribe();
            }

            public IFileDbService DbService3 { get; } = new UtilityDAL.CSV.CSV(Constants.DefaultDbDirectory);

            public ReadOnlyObservableCollection<Zio.FileEntry> Items => items;

            private void FileCRUDControl_SeriesRetrieved(object sender, RoutedEventArgs e)
            {
            }
        }

        private void FileCRUDControl_SeriesRetrieved(object sender, RoutedEventArgs e)
        {
        }
    }
}