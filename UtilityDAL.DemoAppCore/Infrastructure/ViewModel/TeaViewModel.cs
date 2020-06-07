//namespace UtilityDAL.DemoApp
//{
//public class TeaProviderDummyViewModel : ViewModel.ProviderViewModel
//{
//    public TeaProviderDummyViewModel() : base(new DummyTeaDataService().Resource.ToReactiveCollection())
//    {
//    }
//}

//public class DummyTeaDataService : IObservableService<DataFile>
//{
//    public IObservable<DataFile> Resource { get; }
//    public DummyTeaDataService()
//    {
//        var path = System.IO.Directory.GetCurrentDirectory();
//        Resource = FileGenerator.GenerateDataFilesDefault(new Teatime<Price>(""), "tea");

//    }

//}

//}