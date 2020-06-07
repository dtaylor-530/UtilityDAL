//using DynamicData;
//using Reactive.Bindings;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.IO;
//using System.Linq;
//using System.Reactive.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows.Input;
//using UtilityDAL.Entity;
//using UtilityInterface;

//namespace UtilityDAL.DemoApp.ViewModel
//{
//    public class Filex
//    {
//    private static readonly DirectoryInfo directory = System.IO.Directory.CreateDirectory(direactoryPath);
//    private ReadOnlyObservableCollection<UtilityDAL.Entity.KeyValueDate> database;
//    const string direactoryPath = "/FilexData";
//    const string dbName = "files.sqlite";

//    public ReactiveCollection<UtilityDAL.Model.Filex> Filexs { get; }

//    public ReadOnlyObservableCollection<UtilityDAL.Entity.KeyValueDate> Database => database;

//    public UtilityDAL.Model.Filex SelectedItem { get; set; }

//    public ICommand Refresh { get; } = new ReactiveCommand();

//    public ICommand Store { get; } = new ReactiveCommand();

//    public Filex()
//    {
//        UtilityDAL.IDbService<UtilityDAL.Entity.KeyValueDate,string> sqlite = new UtilityDAL.Sqlite<UtilityDAL.Entity.KeyValueDate,string>(_ => _.Key);

//        Filexs = Observable.Interval(TimeSpan.FromSeconds(3))
//            .StartWith(0)
//            .Select(_ => UtilityDAL.Factory.Filex.Create(Path.Combine(directory.FullName, UtilityHelper.RandomHelper.NextWord())))
//            .ToReactiveCollection();

//        (Store as ReactiveCommand).Subscribe(_ =>
//        {
//            SelectedItem.Date = DateTime.Now;
//            var y = UtilityDAL.Map.FilexToKeyValueDate.ToKeyValueDate(SelectedItem);
//            if (sqlite.FindById(y.Key) == null)
//                sqlite.Insert(y);
//        });

//        var dis = (Refresh as ReactiveCommand)
//         .StartWith(new object())
//         .SelectMany(_ => sqlite.FindAll())
//         .ToObservableChangeSet(_ => _.Key)
//         .Sort(new DateTimeComparer())
//         .DisposeMany()
//         .Bind(out database)
//         .Subscribe();

//        SelectedItem = Filexs.Count > 0 ? Filexs[0] : null;
//    }
//}

//class DateTimeComparer : Comparer<UtilityDAL.Entity.KeyValueDate>
//{
//    public override int Compare(UtilityDAL.Entity.KeyValueDate x, UtilityDAL.Entity.KeyValueDate y)
//    {
//        return System.Convert.ToInt32(x.Date > y.Date);
//    }

//}