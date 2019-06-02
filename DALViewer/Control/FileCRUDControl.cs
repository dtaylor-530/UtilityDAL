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
        Button InsertButton;
        Button RefreshButton;
        DataGrid DataGrid;

        public override void OnApplyTemplate()
        {
            Execution = this.GetTemplateChild("ExecutionTextBlock") as TextBlock;
            InsertButton = this.GetTemplateChild("Add") as Button;
            RefreshButton = this.GetTemplateChild("Refresh") as Button;
            DataGrid = this.GetTemplateChild("Data") as DataGrid;
            InsertButton.Click += (a, b) => Insert();
            RefreshButton.Click += (a, b) => this.Dispatcher.InvokeAsync(() =>
            {
                var data = Refresh();
                if (data != null)
                {
                    RaiseSeriesRetrievedEvent(data);
                    Data = data;
                }
            }, System.Windows.Threading.DispatcherPriority.Background);
        }

        public FileCRUDControl()
        {
            System.IO.Directory.CreateDirectory("../../Data");

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


        private void Insert()
        {
            {
                try
                {
                    var sw = System.Diagnostics.Stopwatch.StartNew();
                    var data = Data.Cast<object>().ToList();

                    object service = Service;
                    Type type = Service.GetType();
                    if (type.GetInterfaces().Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)))
                    {
                        var t2 = type.GetInterfaces().First(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)).GetGenericArguments().First();
                        string databaseName = GetDatabaseName(type);
                        var method = type.GetMethod("ToDb");
                        object pdata = ConvertList(data, t2);
                        Task.Run(() =>
                        method.Invoke(service, new object[] { pdata, databaseName }))
                               
                 .ContinueWith(x => sw.Stop())
                 .ContinueWith(x => Execution.Text = sw.ElapsedMilliseconds.ToString(), TaskScheduler.FromCurrentSynchronizationContext());
                    }
                    else if ((Service is IFileDbService))
                    {
                        string databaseName = DatabaseName == string.Empty ? "one" : DatabaseName;
                        Task.Run(() => (service as IFileDbService).ToDb(data, databaseName))
                  .ContinueWith(x => sw.Stop())
                  .ContinueWith(x => Execution.Text = sw.ElapsedMilliseconds.ToString(), TaskScheduler.FromCurrentSynchronizationContext());
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
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


        private IEnumerable Refresh()
        {
            try
            {
                dynamic service = null;
                Type type = Service.GetType();

                if (type.GetInterfaces().Any(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)))
                {
                    var method = type.GetMethod("FromDb");
                    string databaseName = GetDatabaseName(type);
                    var invoke = method.Invoke(Service, new object[] {databaseName });
                    return (IEnumerable)invoke;
                }
                else if ((Service is IFileDbService))
                {
                    return (Service as IFileDbService).FromDb(DatabaseName == string.Empty ? "one" : DatabaseName);
                }

            }
            catch(Exception e)
            {
            }
            finally
            {
            }
            return null;
        }


        private string GetDatabaseName(Type type)
        {
            return DatabaseName == string.Empty ? 
                type.GetInterfaces().Single(interfaceType => interfaceType.GetGenericTypeDefinition() == typeof(IFileDbService<>)).GetGenericArguments().First().Name :
                DatabaseName;
            
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
