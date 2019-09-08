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
using System.Windows.Data;

namespace UtilityDAL.View
{
    public class FileViewer : Control
    {
        static DependencyHelper<string> PathObserver = new DependencyHelper<string>();
        // static DependencyHelper<IFileParser> SelectedFileParserObserver = new DependencyHelper<IFileParser>();
        public ISubject<IFileParser> fileParserChanges { get; } = new Subject<IFileParser>();
        public ISubject<PropertyGroupDescription> PropertyGroupDescriptionChanges { get; } = new Subject<PropertyGroupDescription>();

        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(FileViewer), new PropertyMetadata(null, PathObserver.Changed));

        public static readonly DependencyProperty FileParsersProperty = DependencyProperty.Register("FileParsers", typeof(Dictionary<string, IFileParser>), typeof(FileViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty SelectedFileParserProperty = DependencyProperty.Register("SelectedFileParser", typeof(object), typeof(FileViewer), new PropertyMetadata(null, FileParserChanged));

        private static void FileParserChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FileViewer fileViewer = d as FileViewer;
            if (e.NewValue is KeyValuePair<string, IFileParser> kvp)
            {
                fileViewer.fileParserChanges.OnNext(kvp.Value);
            }
            else
            {
                fileViewer.fileParserChanges.OnNext(e.NewValue as IFileParser);
            }
        }

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(FileViewer), new PropertyMetadata(null));

        public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(FileViewer), new PropertyMetadata(null, OutputChanged));

        //public static readonly DependencyProperty DataProperty = DependencyProperty.Register("Data", typeof(IEnumerable), typeof(FileViewer));



        public IValueConverter DataConverter
        {
            get { return (IValueConverter)GetValue(DataConverterProperty); }
            set { SetValue(DataConverterProperty, value); }
        }


        public static readonly DependencyProperty DataConverterProperty = DependencyProperty.Register("DataConverter", typeof(IValueConverter), typeof(FileViewer), new PropertyMetadata(null, DataCoverterChanged));

        private static void DataCoverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileViewer).DataConverterChanges.OnNext(e.NewValue as IValueConverter);
        }

        public Control OutputView
        {
            get { return (Control)GetValue(OutputViewProperty); }
            set { SetValue(OutputViewProperty, value); }
        }

        public static readonly DependencyProperty OutputViewProperty = DependencyProperty.Register("OutputView", typeof(Control), typeof(FileViewer), new PropertyMetadata(null));


        public override void OnApplyTemplate()
        {
            this.DockPanelChanges.OnNext(this.GetTemplateChild("DockPanel") as DockPanel);

        }

        public PropertyGroupDescription PropertyGroupDescription
        {
            get { return (PropertyGroupDescription)GetValue(PropertyGroupDescriptionProperty); }
            set { SetValue(PropertyGroupDescriptionProperty, value); }
        }


        public static readonly DependencyProperty PropertyGroupDescriptionProperty = DependencyProperty.Register("PropertyGroupDescription", typeof(PropertyGroupDescription), typeof(FileViewer), new PropertyMetadata(null, PropertyGroupDescriptionChanged));

        private static void PropertyGroupDescriptionChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileViewer).PropertyGroupDescriptionChanges.OnNext(e.NewValue as PropertyGroupDescription);
        }

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
        private ISubject<DockPanel> DockPanelChanges = new Subject<DockPanel>();
        private ISubject<IValueConverter> DataConverterChanges = new Subject<IValueConverter>();

        static FileViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileViewer), new FrameworkPropertyMetadata(typeof(FileViewer)));
        }



        public FileViewer()
        {
            var type = typeof(IFileParser);

            var types = UtilityHelper.AssemblyHelper.GetNonSystemAssembliesInCurrentDomain().SelectMany(assembly => assembly.GetTypes())
                .Where(p => p.GetInterfaces().Contains(type) && !p.IsAbstract && !p.IsInterface)
                .ToDictionary(_ => _.GetDescription(), _ => (IFileParser)Activator.CreateInstance(_));

            this.SetValue(FileParsersProperty, types);

            PathObserver.Changes.Where(_ => _ != null)
                     .CombineLatest(fileParserChanges, (a, b) =>
                     new { Files = System.IO.Directory.GetFiles(a, b.Filter(), System.IO.SearchOption.AllDirectories), b })

           .Select(_ => _.Files.Select(__ => new PathViewModel(__, _.b.Map)))
           //.CombineLatest(PropertyGroupDescriptionChanges,(a,b)=>a)
           .ObserveOnDispatcher()
          .Subscribe(_ => this.Dispatcher.InvokeAsync(() => Items = _, System.Windows.Threading.DispatcherPriority.Render));


            OutputChanges.Where(_ => _ != null).WithLatestFrom(fileParserChanges, (a, b) =>
                (b as IFileParser).Parse((a as PathViewModel).FullName))
            .CombineLatest(DataConverterChanges.StartWith(default(IValueConverter)), (a, b) => (a, b))
            .SubscribeOn(TaskPoolScheduler.Default)
            .ObserveOnDispatcher()
             .Subscribe(_ => this.Dispatcher.InvokeAsync(() =>

              (OutputView is IItemsSource oview) ?
                 _.b == default(IValueConverter) ?
                 oview.ItemsSource = _.a :
                 oview.ItemsSource = _.b.Convert(_.a, null, null, null) as IEnumerable :

                _.b == default(IValueConverter) ?
             (OutputView as ItemsControl).ItemsSource = _.a :
             (OutputView as ItemsControl).ItemsSource = _.b.Convert(_.a, null, null, null) as IEnumerable,
             System.Windows.Threading.DispatcherPriority.Normal));


            PropertyGroupDescriptionChanges.WithLatestFrom(DockPanelChanges, (pgd, DockPanel) => (pgd, DockPanel)).Subscribe(_ =>
              {
                  var collectionViewSource = _.DockPanel?.FindResource("GroupedItems") as CollectionViewSource;
                  if (collectionViewSource != null)
                      collectionViewSource.GroupDescriptions.Add(_.pgd);
              });

            OutputView = OutputView ?? new NavigatorView();
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
