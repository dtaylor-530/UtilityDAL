using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityDAL.Contract;
using UtilityDAL.Contract.Generic;
using UtilityDAL.Contract.NonGeneric;

namespace UtilityDAL.View
{
    public class FileCRUDControl : Control
    {

        static FileCRUDControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileCRUDControl), new FrameworkPropertyMetadata(typeof(FileCRUDControl)));
        }

        private const string ConnectionString = "../../Data/1";

        TextBlock Execution;
        Button AddButton;
        Button ClearButton;
        Button RefreshButton;
        DataGrid DataGrid;

        public override void OnApplyTemplate()
        {
            Execution = this.GetTemplateChild("ExecutionTextBlock") as TextBlock;
            AddButton = this.GetTemplateChild("Add") as Button;
            ClearButton = this.GetTemplateChild("Clear") as Button;
            RefreshButton = this.GetTemplateChild("Refresh") as Button;
            DataGrid = this.GetTemplateChild("Data") as DataGrid;
            ClearButton.Click += (a, b) => Clear();
            AddButton.Click += (a, b) => Add();
            RefreshButton.Click += (a, b) => this.Dispatcher.InvokeAsync(() =>
            {
                //var data = Refresh();
                Get();
                if (Data != null)
                {
                    RaiseSeriesRetrievedEvent(Data);
                }
            }, System.Windows.Threading.DispatcherPriority.Background);
        }



        public FileCRUDControl()
        {
          //  System.IO.Directory.CreateDirectory("../../Data");
        }



        public string DatabaseName
        {
            get { return (string)GetValue(DatabaseNameProperty); }
            set { SetValue(DatabaseNameProperty, value); }
        }

        // Using a DependencyProperty as the backing store for DatabaseName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DatabaseNameProperty =
            DependencyProperty.Register("DatabaseName", typeof(string), typeof(FileCRUDControl), new PropertyMetadata(string.Empty));



        public object Service
        {
            get { return (object)GetValue(ServiceProperty); }
            set { SetValue(ServiceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Service.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ServiceProperty = DependencyProperty.Register("Service", typeof(object), typeof(FileCRUDControl), new PropertyMetadata(null));




        public IEnumerable Data
        {
            get { return (IEnumerable)GetValue(DataProperty); }
            set { SetValue(DataProperty, value); }
        }


        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(IEnumerable), typeof(FileCRUDControl), new PropertyMetadata(null));

        private async void Clear()
        {

            try
            {
                string databaseName = DatabaseName == string.Empty ? "one" : DatabaseName;
                if (Service is IFileDbService dbservice)
                {
                    await this.Dispatcher.InvokeAsync(async ()=> Execution.Text = await TimedAction(() => dbservice.Clear(databaseName)));
                }
                else
                {
                    object service = Service;
                    await this.Dispatcher.InvokeAsync(async () => Execution.Text = await TimedAction(GetClearAction(service, databaseName)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private async void Add()
        {

            try
            {
                var data = Data.Cast<object>().ToList();
                string databaseName = DatabaseName == string.Empty ? "one" : DatabaseName;
                if (Service is IFileDbService dbservice)
                {
                    await this.Dispatcher.InvokeAsync(async () => Execution.Text = await TimedAction(() => dbservice.To(data,databaseName)));
                }
                else
                {
                    object service = Service;
                    await this.Dispatcher.InvokeAsync(async () => Execution.Text = await TimedAction(GetToDbAction(service, databaseName, data)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private async void Get()
        {

            try
            {
                var data = Data.Cast<object>().ToList();
                object service = Service;

                if (service is IFileDbService dbservice)
                {
                    string databaseName = DatabaseName == string.Empty ? "one" : DatabaseName;
                    await this.Dispatcher.InvokeAsync(async () => Execution.Text = await TimedAction(async () =>await this.Dispatcher.InvokeAsync(()=>Data= dbservice.From(databaseName))));
                }
                else
                {
                    await this.Dispatcher.InvokeAsync(async () => Execution.Text = await TimedAction(async () => await this.Dispatcher.InvokeAsync(() => Data = GetFromDbAction(service, DatabaseName)?.Invoke() as IEnumerable)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static Action GetToDbAction(object service,string databaseName,List<object> data)
        {
            Type type = service.GetType();
            var @interface = type.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDatabase<>));
            if (@interface != null)
            {
                var t2 = @interface.GetGenericArguments()?.First();
                var method = type.GetMethod(nameof(IFileDatabase<object>.To));
                object pdata = ConvertList(data, t2);
                Action a = () => method.Invoke(service, new object[] { pdata, GetDatabaseName(t2, databaseName) });
                return a;
            }
            return null;
        }

        private static Func<object> GetFromDbAction(object service, string databaseName)
        {
            Type type = service.GetType();
            var @interface = type.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDatabase<>));
            if (@interface != null)
            {
                var t2 = @interface.GetGenericArguments()?.First();
                var method = type.GetMethod(nameof(IFileDatabase<object>.To));
                Func<object> a = () => method.Invoke(service, new object[] { GetDatabaseName(t2, databaseName) });
            
                return a;
            }
            return null;
        }


        private static Action GetClearAction(object service, string databaseName)
        {
            Type type = service.GetType();
            var @interface = type.GetInterfaces().FirstOrDefault(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDatabase<>));
            if (@interface != null)
            {
                var t2 = @interface.GetGenericArguments()?.First();
                var method = type.GetMethod(nameof(IFileDatabase<object>.Clear));
                Action a = () => method.Invoke(service, new object[] { GetDatabaseName(t2, databaseName) });
                return a;
            }
            return null;
        }

        private static async Task<string> TimedAction(Action a)
        {
            return await await Task.Run(() =>
            {
                System.Diagnostics.Stopwatch sw = System.Diagnostics.Stopwatch.StartNew();
                a();
                sw.Stop();
                return sw;
            }).ContinueWith(async sw => (await sw).ElapsedMilliseconds.ToString());
        }


        public static object ConvertList(IList<object> items, Type type, bool performConversion = false)
        {

            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast)).MakeGenericMethod(type);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToList)).MakeGenericMethod(type);

            IEnumerable<object> itemsToCast;

            if (performConversion)
            {
                itemsToCast = items.Select(item => Convert.ChangeType(item, type));
            }
            else
            {
                itemsToCast = items;
            }

            var castedItems = castMethod.Invoke(null, new[] { itemsToCast });

            return toListMethod.Invoke(null, new[] { castedItems });
        }


        //private IEnumerable Refresh()
        //{
        //    try
        //    {
        //        dynamic service = null;
        //        Type type = Service.GetType();

        //        if (type.GetInterfaces().Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)))
        //        {
        //            var method = type.GetMethod("FromDb");
        //            string databaseName = GetDatabaseName(type,DatabaseName);
        //            var invoke = method.Invoke(Service, new object[] { databaseName });
        //            return (IEnumerable)invoke;
        //        }
        //        else if ((Service is IFileDbService))
        //        {
        //            return (Service as IFileDbService).From(DatabaseName == string.Empty ? "one" : DatabaseName);
        //        }

        //    }
        //    catch (Exception e)
        //    {
        //    }
        //    finally
        //    {
        //    }
        //    return null;
        //}


        private static string GetDatabaseName(Type type, string databaseName)
        {
            return databaseName == string.Empty ? type.Name :  databaseName;

        }

        public static readonly RoutedEvent SeriesRetrievedEvent = EventManager.RegisterRoutedEvent("SeriesRetrieved", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FileCRUDControl));


        public event RoutedEventHandler SeriesRetrieved
        {
            add { AddHandler(SeriesRetrievedEvent, value); }
            remove { RemoveHandler(SeriesRetrievedEvent, value); }
        }

        void RaiseSeriesRetrievedEvent(IEnumerable series)
        {
            SeriesRoutedEventArgs newEventArgs = new SeriesRoutedEventArgs(FileCRUDControl.SeriesRetrievedEvent) { Series = series };
            RaiseEvent(newEventArgs);
        }


        public class SeriesRoutedEventArgs : RoutedEventArgs
        {
            public IEnumerable Series { get; set; }

            public SeriesRoutedEventArgs(RoutedEvent @event) : base(@event)
            {

            }

        }
    }
}




//private void Add()
//{
//    try
//    {
//        var sw = System.Diagnostics.Stopwatch.StartNew();
//        var data = Data.Cast<object>().ToList();

//        object service = Service;
//        Type type = Service.GetType();
//        if (Service is IFileDbService dbservice)
//        {
//            string databaseName = DatabaseName == string.Empty ? "one" : DatabaseName;
//            Task.Run(() =>
//            (dbservice).To(data, databaseName))
//      .ContinueWith(x => sw.Stop())
//      .ContinueWith(x => Execution.Text = sw.ElapsedMilliseconds.ToString(), TaskScheduler.FromCurrentSynchronizationContext());
//        }
//        else if (type.GetInterfaces().Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)))
//        {
//            var t2 = type.GetInterfaces().First(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)).GetGenericArguments().First();
//            string databaseName = GetDatabaseName(type);
//            var method = type.GetMethod("ToDb");
//            object pdata = ConvertList(data, t2);
//            Task.Run(() =>
//            method.Invoke(service, new object[] { pdata, databaseName }))

//     .ContinueWith(x => sw.Stop())
//     .ContinueWith(x => Execution.Text = sw.ElapsedMilliseconds.ToString(), TaskScheduler.FromCurrentSynchronizationContext());

//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//    }
//}



//private Action GetAction(Action action, string methodName)
//{

//    try
//    {

//        var data = Data.Cast<object>().ToList();

//        object service = Service;
//        Type type = Service.GetType();
//        if (Service is IFileDbService dbservice)
//        {
//            string databaseName = DatabaseName == string.Empty ? "one" : DatabaseName;
//            Task.Run(() =>
//            (dbservice).Clear(databaseName))
//      .ContinueWith(x => sw.Stop())
//      .ContinueWith(x => Execution.Text = sw.ElapsedMilliseconds.ToString(), TaskScheduler.FromCurrentSynchronizationContext());
//        }
//        else if (type.GetInterfaces().Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)))
//        {
//            var t2 = type.GetInterfaces().First(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)).GetGenericArguments().First();
//            string databaseName = GetDatabaseName(type);
//            var method = type.GetMethod("ToDb");
//            object pdata = ConvertList(data, t2);
//            Action a = () => method.Invoke(service, new object[] { pdata, databaseName });
//        }
//    }
//    catch (Exception ex)
//    {
//        Console.WriteLine(ex.Message);
//    }
//}
