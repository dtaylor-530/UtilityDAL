using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace UtilityDAL.DemoApp
{
    public class PaginatedViewModel:ReactiveUI.ReactiveObject
    {
        public string File { get; } = System.IO.Directory.GetFiles("../../../Data", "*.csv", System.IO.SearchOption.AllDirectories).First();

        public IEnumerable<dynamic> Items { get; }

        public PaginatedViewModel(UtilityWpf.IDispatcherService ds)
        {
            Items = new UtilityDAL.CSV.CSV().From(File).Cast<dynamic>();
        }
    }
}