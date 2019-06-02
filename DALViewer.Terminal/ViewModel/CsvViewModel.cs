using UtilityDAL.Model;
using UtilityDAL;
using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

using UtilityWpf.ViewModel;

namespace UtilityDAL.DemoApp
{ 
    //public class CsvProviderDummyViewModel : ViewModel.ProviderViewModel 
    //{
    //    private const string folder = "";
    //    public static FolderPickerViewModel FolderPickerVM { get; } = new FolderPickerViewModel(folder);

    //    public CsvProviderDummyViewModel() : base(new CsvDummyDataService().Resource.ToReactiveCollection())
    //    {

    //    }
    //}

    //public class CsvDummyDataService : IService<DataFile>
    //{
    //    public IObservable<DataFile> Resource { get; }

    //    public CsvDummyDataService()
    //    {
    //        Resource = csvHelper.GenerateDataFilesDefault(new CSV<Pricecsv>(""), "csv",4);

    //    }

    //}
}
