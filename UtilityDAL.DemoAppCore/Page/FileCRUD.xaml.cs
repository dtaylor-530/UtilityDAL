using DynamicData;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL.Contract.NonGeneric;
using UtilityDAL.Model;

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
            private ReadOnlyObservableCollection<Model.Filex> items;
            private const string directoryPath = "/FilextData";

            public FilexViewModel()
            {
                Observable.Interval(TimeSpan.FromSeconds(3))
        .StartWith(0)
        .ObserveOnDispatcher()
        .Select(_ => UtilityDAL.Factory.Filex.Create(System.IO.Path.Combine(directory.FullName, UtilityHelper.RandomHelper.NextWord())))
   .ToObservableChangeSet()
        .Bind(out items)
        .Subscribe();
            }

            public IFileDbService DbService3 { get; } = new UtilityDAL.CSV.CSV(@"../../../Data");

            public ReadOnlyObservableCollection<Model.Filex> Items => items;

            private void FileCRUDControl_SeriesRetrieved(object sender, RoutedEventArgs e)
            {
            }
        }

        private void FileCRUDControl_SeriesRetrieved(object sender, RoutedEventArgs e)
        {
        }
    }
}