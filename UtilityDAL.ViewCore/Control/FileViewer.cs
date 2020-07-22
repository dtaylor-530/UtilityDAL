using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Windows;
using System.Windows.Controls;
using UtilityWpf.ViewModel;
using System.Windows.Data;
using System.Windows.Threading;
using System.Windows.Input;
using UtilityWpf.View;
using System.Globalization;
using UtilityDAL.Abstract;

namespace UtilityDAL.View
{
    public class FileViewer : Control
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.Register("Path", typeof(string), typeof(FileViewer), new PropertyMetadata(null, PathChanged));

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

        //public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(FileViewer), new PropertyMetadata(null, OutputChanged));


        //public IValueConverter DataConverter
        //{
        //    get { return (IValueConverter)GetValue(DataConverterProperty); }
        //    set { SetValue(DataConverterProperty, value); }
        //}

        //public static readonly DependencyProperty DataConverterProperty = DependencyProperty.Register("DataConverter", typeof(IValueConverter), typeof(FileViewer), new PropertyMetadata(null, DataCoverterChanged));

        //private static void DataCoverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as FileViewer).DataConverterChanges.OnNext(e.NewValue as IValueConverter);
        //}

        public Control OutputView
        {
            get { return (Control)GetValue(OutputViewProperty); }
            set { SetValue(OutputViewProperty, value); }
        }

        public static readonly DependencyProperty OutputViewProperty = DependencyProperty.Register("OutputView", typeof(Control), typeof(FileViewer), new PropertyMetadata(null));

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

        //public object Output
        //{
        //    get { return (object)GetValue(OutputProperty); }
        //    set { SetValue(OutputProperty, value); }
        //}

        //private static void OutputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    (d as FileViewer).OutputChanges.OnNext(e.NewValue);
        //}

        private static void PathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileViewer).PathChanges.OnNext((string)e.NewValue);
        }

        protected ISubject<string> PathChanges = new Subject<string>();
        //protected ISubject<object> OutputChanges = new Subject<object>();
        protected ISubject<FrameworkElement> ControlChanges = new Subject<FrameworkElement>();
        //protected ISubject<IValueConverter> DataConverterChanges = new Subject<IValueConverter>();
        protected ISubject<string> GroupNameChanges = new Subject<string>();
        //protected ISubject<string> NameChanges = new Subject<string>();
        protected ISubject<IFileParser> fileParserChanges { get; } = new Subject<IFileParser>();
        protected ISubject<PropertyGroupDescription> PropertyGroupDescriptionChanges { get; } = new Subject<PropertyGroupDescription>();

        public override void OnApplyTemplate()
        {
            //this.ControlChanges.OnNext(this.GetTemplateChild("DockPanel") as DockPanel);
            //this.ControlChanges.OnNext(this.GetTemplateChild("TextBlock1") as TextBlock);
            this.ControlChanges.OnNext(this.GetTemplateChild("MasterDetailView1") as MasterDetailView);

        }

        static FileViewer()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileViewer), new FrameworkPropertyMetadata(typeof(FileViewer)));
        }

        public FileViewer()
        {
            var type = typeof(IFileParser);

            var types = UtilityHelper.AssemblyHelper.GetNonSystemAssembliesInCurrentDomain()
                .SelectMany(assembly =>
                {
                    try
                    {
                        return assembly.GetTypes();
                    }
                    catch (Exception ex)
                    {
                        return new Type[0];
                    }
                })
                .Where(p => p.GetInterfaces().Contains(type) && !p.IsAbstract && !p.IsInterface)
                .ToDictionary(_ => _.GetDescription(), _ => (IFileParser)Activator.CreateInstance(_));

            this.SetValue(FileParsersProperty, types);

            PathChanges
                        .Where(a => a != null)
                        .CombineLatest(fileParserChanges, ControlChanges.Where(a => a is MasterDetailView), (a, b, mv) =>
                           new { Files = System.IO.Directory.GetFiles(a, b.Filter(), System.IO.SearchOption.AllDirectories), b, master = mv as MasterDetailView })
                        .Select(a => (a.master, map: new Func<string, ICollection>(a.b.Parse), items: a.Files.Select(__ => new PathViewModel(__, a.b.Map))))
                        .ObserveOnDispatcher()
                        .Subscribe(controlMapAndItems => this.Dispatcher.InvokeAsync(() =>
                        {
                            Items = controlMapAndItems.items;
                            if((controlMapAndItems.master.Content is PageView )==false)
                            controlMapAndItems.master.Content = new PageView();
                            controlMapAndItems.master.DataConverter = new CollectionConverter(controlMapAndItems.map);
                        }, System.Windows.Threading.DispatcherPriority.Render));

            //var viewModelChanges = OutputChanges.Where(obj => obj != null)
            //                                    .Select(p => p as PathViewModel);

            //viewModelChanges.Select(vm => vm.Name).Subscribe(NameChanges);

            //    viewModelChanges
            //                  .WithLatestFrom(fileParserChanges, (p, parser) => (parsed: (parser as IFileParser).Parse(p.FullName), name: p.Name))
            //                .CombineLatest(DataConverterChanges.StartWith(default(IValueConverter)), (a, b) => (a, b))
            //                .SubscribeOn(TaskPoolScheduler.Default)
            //                .ObserveOnDispatcher()
            //                .Subscribe(collConv => this.Dispatcher.InvokeAsync(() =>
            //                {
            //                    (IEnumerable itemsSource, string nnn) = collConv.a;
            //                    //fsd(itemsSource, collConv.b);
            //                    Convert(itemsSource, collConv.b, (items, conv) => conv.Convert(items, null, null, null) as IEnumerable);
            //                }, DispatcherPriority.Normal));

            //    PropertyGroupDescriptionChanges.CombineLatest(ControlChanges.Where(c => c.GetType() == typeof(DockPanel)).Take(1), (pgd, DockPanel) => (pgd, DockPanel))
            //        .Subscribe(xx =>
            //      {
            //          var collectionViewSource = xx.DockPanel?.FindResource("GroupedItems") as CollectionViewSource;
            //          if (collectionViewSource != null)
            //              collectionViewSource.GroupDescriptions.Add(xx.pgd);
            //      });

            //    OutputView = OutputView ?? new PageView();


            //    GroupClick = new UtilityWpf.Commmand.RelayCommand<string>(a => GroupNameChanges.OnNext(a));

            //    NameChanges
            //.Merge(GroupNameChanges)
            //.CombineLatest(ControlChanges.Select(c => c as TextBlock).Where(c => c != null),
            //        (text, textBlock) => (text, textBlock))
            //        .Subscribe(input =>
            //        {
            //            input.textBlock.Text = input.text;
            //            input.textBlock.Visibility = Visibility.Visible;
            //            input.textBlock.IsEnabled = true;
            //            input.textBlock.IsEnabled = false;
            //        });

            //    GroupNameChanges.CombineLatest(
            //        PropertyGroupDescriptionChanges.StartWith(default(PropertyGroupDescription)),
            //        DataConverterChanges.StartWith(default(IValueConverter)),
            //                  fileParserChanges,
            //        (text, pg, conv, parser) => (text, pg, conv, parser))
            //        .Subscribe(async input =>
            //        {
            //            await this.Dispatcher.InvokeAsync(() =>
            //                     {

            //                         var paths = Items.Cast<PathViewModel>().ToArray();
            //                         var prop = typeof(PathViewModel).GetProperty(input.pg.PropertyName);

            //                         // property-group-converter
            //                         var converter = input.pg.Converter;

            //                         var group = paths.Where(ad =>

            //                              converter != default ?
            //                               input.text
            //                                       .Equals(converter.Convert(prop.GetValue(ad), null, null, null)) :
            //                               input.text
            //                                       .Equals(prop.GetValue(ad))
            //                         ).Select(viewmodel =>
            //                         new Model.KeyCollection(viewmodel.Name, input.parser.Parse(viewmodel.FullName)));

            //                         Convert(group, input.conv, (items, conv) => items.Cast<object>().Select(ka => conv.Convert(ka, null, null, null) as IEnumerable).ToArray());

            //                     }, DispatcherPriority.Background);
            //        });
        }

        //private void Convert(IEnumerable items, IValueConverter conv, Func<IEnumerable, IValueConverter, IEnumerable> func)
        //{
        //    if (OutputView is IItemsSource oview)
        //    {
        //        oview.ItemsSource = convert(conv, func, items);
        //    }
        //    else if (OutputView is ItemsControl itemsControl)
        //    {
        //        itemsControl.ItemsSource = convert(conv, func, items);
        //    }
        //    else
        //        throw new Exception(nameof(OutputView) + " needs to have property OutputView");

        //    static IEnumerable convert(IValueConverter conv, Func<IEnumerable, IValueConverter, IEnumerable> func, IEnumerable items)
        //    {
        //        return
        //              conv == default ?
        //              items :
        //              func(items, conv);
        //    }
        //}

        public ICommand GroupClick { get; }

    }

    class CollectionConverter : IValueConverter
    {
        private readonly Func<string, ICollection> map;

        public CollectionConverter(Func<string, ICollection> map)
        {
            this.map = map;
        }


        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.map.Invoke(value.ToString());
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}