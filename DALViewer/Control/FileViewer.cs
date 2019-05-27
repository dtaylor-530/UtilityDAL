using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityWpf.ViewModel;
using UtilityDAL.Contract;

namespace UtilityDAL.View
{
    public class FileViewer : Control
    {
        static DependencyHelper<string> PathObserver = new DependencyHelper<string>();
        static DependencyHelper<IFileParser> SelectedFileParserObserver = new DependencyHelper<IFileParser>();


        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(FileViewer), new PropertyMetadata(null, PathObserver.Changed));

        public static readonly DependencyProperty FileParsersProperty = DependencyProperty.Register("FileParsers", typeof(Dictionary<string, IFileParser>), typeof(FileViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedFileParserProperty = DependencyProperty.Register("SelectedFileParser", typeof(object), typeof(FileViewer), new PropertyMetadata(null, SelectedFileParserObserver.Changed));

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(FileViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(FileViewer), new PropertyMetadata(null, OutputChanged));






        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(IEnumerable), typeof(FileViewer));



        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public string Path
        {
            get { return (string)GetValue(PathProperty); }
            set { SetValue(PathProperty, value); }
        }

        public object Output
        {
            get { return (object)GetValue(OutputProperty); }
            set { SetValue(OutputProperty, value); }
        }
        private static void OutputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileViewer).OutputChanges.OnNext(e.NewValue);
        }
        protected ISubject<object> OutputChanges = new Subject<object>();


        static FileViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileViewer), new FrameworkPropertyMetadata(typeof(FileViewer)));
        }



        public FileViewer()
        {
            //Uri resourceLocater = new Uri("/UtilityDAL;component/Themes/FileViewer.xaml", System.UriKind.Relative);
            //ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            //Style = resourceDictionary["FileViewer"] as Style;

            var type = typeof(IFileParser);

            var types = type.Assembly.GetTypes()
                .Where(p =>
                p.GetInterfaces().Contains(type) && !p.IsAbstract && !p.IsInterface)
                .ToDictionary(_ => _.GetDescription(), _ => (IFileParser)Activator.CreateInstance(_));

            this.SetValue(FileParsersProperty, types);

            var x = TaskPoolScheduler.Default;

            PathObserver.Changes.Where(_ => _ != null)
                     .CombineLatest(SelectedFileParserObserver.Changes, (a, b) =>
                     new { Files = System.IO.Directory.GetFiles(a, b.Filter(), System.IO.SearchOption.AllDirectories), b })
           .Select(_ => _.Files.Select(__ => new PathViewModel(__, _.b.Map)))
          .Subscribe(_ => this.Dispatcher.InvokeAsync(() => { Items = _; }, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken)));


            OutputChanges.WithLatestFrom(SelectedFileParserObserver.Changes, (a, b) =>
            (b as IFileParser).Parse((a as PathViewModel).FullName))
            .SubscribeOn(x)
             .Subscribe(_ =>
             this.Dispatcher.InvokeAsync(() =>
             this.SetValue(DataProperty, _), System.Windows.Threading.DispatcherPriority.Background));

        }
    }

    public class DependencyHelper<T>
    {
        public IObservable<T> Changes { get; } = new Subject<T>();

        public void Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (Changes as ISubject<T>).OnNext((T)e.NewValue);
        }
    }



}
