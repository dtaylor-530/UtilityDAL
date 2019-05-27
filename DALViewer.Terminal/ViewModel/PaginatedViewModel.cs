
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityDAL;
using UtilityWpf.ViewModel;

namespace UtilityDAL.DemoApp
{

    public class PaginatedViewModel
        {
        public ReactiveProperty<string> File { get;} = new ReactiveProperty<string>(System.IO.Directory.GetFiles("../../Data", "*.csv", System.IO.SearchOption.AllDirectories).First());

        public ReactiveProperty<IEnumerable<dynamic>> Items { get;  }


        public PaginatedViewModel(UtilityWpf.IDispatcherService ds)
        {
            Items = new ReactiveProperty<IEnumerable<dynamic>>(File.Select(file => new UtilityDAL.CSV().FromDb(file).Cast<dynamic>()));

        }
    }




}
