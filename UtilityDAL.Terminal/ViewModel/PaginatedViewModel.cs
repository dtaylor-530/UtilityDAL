using Reactive.Bindings;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace UtilityDAL.DemoApp
{
    public class PaginatedViewModel
    {
        public ReactiveProperty<string> File { get; } = new ReactiveProperty<string>(System.IO.Directory.GetFiles("../../Data", "*.csv", System.IO.SearchOption.AllDirectories).First());

        public ReactiveProperty<IEnumerable<dynamic>> Items { get; }

        public PaginatedViewModel(UtilityWpf.IDispatcherService ds)
        {
            Items = new ReactiveProperty<IEnumerable<dynamic>>(File.Select(file => new UtilityDAL.CSV.CSV().From(file).Cast<dynamic>()));
        }
    }
}