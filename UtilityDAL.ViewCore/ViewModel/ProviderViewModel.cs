using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using UtilityDAL.Model;
using UtilityInterface.Generic;

namespace UtilityDAL.ViewModel
{
    //public class TeaProviderViewModel<T> : ProviderViewModel where T : struct
    //{
    //    private const string folder = "";
    //    public static FolderPickerViewModel FolderPickerVM { get; } = new FolderPickerViewModel(folder);
    //    public TeaProviderViewModel():base(new TeaDataDummyService<T>(FolderPickerVM.Output).Resource.ToReactiveCollection()) { }

    //}

    //public class TeaProviderDescendingSizeViewModel<T> : ProviderViewModel<int> where T : struct
    //{
    //    private const string folder = "";
    //    public static FolderPickerViewModel FolderPickerVM { get; } = new FolderPickerViewModel(folder);
    //    public TeaProviderDescendingSizeViewModel() : base(new TeaDataDummyService<T>(FolderPickerVM.Output), df => df.Items.Count) { }

    //}

    //public class CsvProviderViewModel<T> : ProviderViewModel where T : struct
    //{
    //    private const string folder = "";
    //    public static FolderPickerViewModel FolderPickerVM { get; } = new FolderPickerViewModel(folder);
    //    public CsvProviderViewModel() : base(new CsvDummyDataService<T>(FolderPickerVM.Output).Resource.ToReactiveCollection())
    //    {
    //    }

    //}

    //public class CsvProviderDescendingSizeViewModel<T>: ProviderViewModel<int> where T : struct
    //{
    //    private const string folder = "";
    //    public static FolderPickerViewModel FolderPickerVM { get; } = new FolderPickerViewModel(folder);
    //    public CsvProviderDescendingSizeViewModel() : base(new CsvDummyDataService<T>(FolderPickerVM.Output), df => df.Items.Count)
    //    { }

    //}

    //public class ProviderViewModel
    //{
    //    public ICollection<DataFile> Items { get; protected set; }

    //    //public  ServiceViewModel<DataFile> ServiceVM { get; }
    //    public DataFile Output => output.Value;

    //    public ICollection<DataFile> SelectedItems { get; }
    //    public ReactiveUI.ReactiveCommand<Unit,DataFile> SelectCommand { get; } 

    //    public ProviderViewModel(ICollection<DataFile> service)
    //    {
    //        SelectCommand = ReactiveUI.ReactiveCommand.CreateFromObservable(() => System.);  
    //        //ServiceVM = new ServiceViewModel<DataFile>(service);
    //        Items = service;
    //        output = Output.Merge(SelectCommand).ToProperty(this, a=>a.Output);
    //        SelectedItems = Output.ToReactiveCollection();
    //    }
    //}

    //public class ProviderViewModel<T> : ProviderViewModel where T : IComparable
    //{
    //    public ProviderViewModel(IService<DataFile> service, Func<DataFile, T> sort, bool byDescending = true)
    //        : base(/*new ServiceViewModel<DataFile, T>(service, sort, byDescending).Items*/null)
    //    {
    //    }
    //}

    //public class CsvProviderViewModel<T> : INPCBase, IOutputViewModel<DataFile>
    //{
    //    public FolderPickerViewModel InputVM { get; }
    //    public FileSelectionViewModel OutputVM { get; private set; }
    //    public IObservable<DataFile> Output { get; }

    //    public CsvProviderViewModel(IDispatcherService ds)
    //    {
    //        var x = new CsvDataService<T>(null);

    //        OutputVM = new FileSelectionViewModel(x, ds);
    //        Output = OutputVM.Output;

    //    }

    //    public CsvProviderViewModel(string s, IDispatcherService ds)
    //    {
    //        InputVM = new FolderPickerViewModel(s);

    //        //this.PropertyChanged += CsvProviderViewModel_PropertyChanged;

    //        var xx = InputVM.Output.Subscribe(_ =>
    //        {
    //            var x = new CsvDataService<T>(s);

    //            OutputVM = new FileSelectionViewModel(x, ds);
    //            OutputVM.Output.Subscribe(__ =>
    //            {
    //                ((System.Reactive.Subjects.ISubject<DataFile>)Output).OnNext(__);
    //            });

    //            NotifyChanged(nameof(OutputVM));
    //        });

    //        Output = new System.Reactive.Subjects.Subject<DataFile>();

    //        //this.WhenPropertyChanged(_=>_.OutputVM).Subscribe(_=>;
    //    }

    //    //private void CsvProviderViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    //{
    //    //    if (e.PropertyName == nameof(OutputVM))
    //    //    {
    //    //    }
    //    //}
    //}

    //public class CsvProviderViewModel : INPCBase, IOutputViewModel<DataFile>
    //{
    //    public FolderPickerViewModel InputVM { get; }
    //    public FileSelectionViewModel OutputVM { get; private set; }
    //    public IObservable<DataFile> Output { get; }

    //    public CsvProviderViewModel(IDispatcherService ds)
    //    {
    //        var x = new CsvDataService(null);

    //        OutputVM = new FileSelectionViewModel(x, ds);

    //    }

    //    public CsvProviderViewModel(string s, IDispatcherService ds)
    //    {
    //        InputVM = new FolderPickerViewModel(s);

    //        var xx = Output.Subscribe(_ =>
    //        {
    //            var x = new CsvDataService(s);
    //            OutputVM = new FileSelectionViewModel(x, ds);
    //            NotifyChanged(nameof(OutputVM));
    //            OutputVM.Output.Subscribe(__ =>
    //            {
    //                ((System.Reactive.Subjects.ISubject<DataFile>)Output).OnNext(__);
    //            });
    //        });

    //        //this.PropertyChanged += CsvProviderViewModel_PropertyChanged;

    //        Output = new System.Reactive.Subjects.Subject<DataFile>();
    //    }

    //    //private void CsvProviderViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    //    //{
    //    //    if (e.PropertyName == nameof(OutputVM))
    //    //    {
    //    //    }
    //    //}

    //    public CsvProviderViewModel(string s, TimeSpan t, IDispatcherService ds)
    //    {
    //        var x = new CsvDataService(t, s);

    //        OutputVM = new FileSelectionViewModel(x, ds);

    //        Output = OutputVM.Output;
    //    }

    //}
}