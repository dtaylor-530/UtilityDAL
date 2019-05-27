using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityWpf.ViewModel;

namespace UtilityDAL.View
{
    public class FileDbControl : Control
    {
        public static readonly DependencyProperty DirectoryProperty = DependencyProperty.Register("Directory", typeof(string), typeof(FileDbControl), new PropertyMetadata(null, DirectoryChanged));

        public static readonly DependencyProperty ItemsProperty = DependencyProperty.Register("Items", typeof(IEnumerable), typeof(FileDbControl), new PropertyMetadata(null, ItemsChanged));

        public static readonly DependencyProperty RootDirectoriesProperty = DependencyProperty.Register("RootDirectories", typeof(IEnumerable<DirectoryViewModel>), typeof(FileDbControl), new PropertyMetadata(null, RootDirectoriesChanged));

        public static readonly DependencyProperty FilesProperty = DependencyProperty.Register("Files", typeof(IEnumerable<FileViewModel>), typeof(FileDbControl), new PropertyMetadata(null, FilesChanged));

        public static readonly DependencyProperty OutputProperty = DependencyProperty.Register("Output", typeof(object), typeof(FileDbControl), new PropertyMetadata(null, OutputChanged));

        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.Register("SelectedItem", typeof(object), typeof(FileDbControl), new PropertyMetadata(null, SelectedItemChanged));




        public string Directory
        {
            get { return (string)GetValue(DirectoryProperty); }
            set { SetValue(DirectoryProperty, value); }
        }

        public IEnumerable<DirectoryViewModel> RootDirectories
        {
            get { return (IEnumerable<DirectoryViewModel>)GetValue(RootDirectoriesProperty); }
            set { SetValue(RootDirectoriesProperty, value); }
        }

        public IEnumerable<FileViewModel> Files
        {
            get { return (IEnumerable<FileViewModel>)GetValue(FilesProperty); }
            set { SetValue(FilesProperty, value); }
        }

        public object Output
        {
            get { return (object)GetValue(OutputProperty); }
            set { SetValue(OutputProperty, value); }
        }

        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public IEnumerable Items
        {
            get { return (IEnumerable)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }



        private static void DirectoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileDbControl).DirectoryChanges.OnNext((string)e.NewValue);
        }

        private static void RootDirectoriesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileDbControl).RootDirectoriesChanges.OnNext((IEnumerable<DirectoryViewModel>)e.NewValue);
        }

        private static void FilesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileDbControl).FilesChanges.OnNext((IEnumerable<FileViewModel>)e.NewValue);
        }

        private static void OutputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileDbControl).OutputChanges.OnNext(e.NewValue);
        }

        private static void SelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileDbControl).SelectedItemChanges.OnNext(e.NewValue);
        }


        private static void ItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as FileDbControl).ItemsChanges.OnNext((IEnumerable)e.NewValue);
        }



        protected ISubject<string> DirectoryChanges = new Subject<string>();

        protected ISubject<IEnumerable<DirectoryViewModel>> RootDirectoriesChanges = new Subject<IEnumerable<DirectoryViewModel>>();

        protected ISubject<IEnumerable<FileViewModel>> FilesChanges = new Subject<IEnumerable<FileViewModel>>();

        protected ISubject<object> OutputChanges = new Subject<object>();

        protected ISubject<object> SelectedItemChanges = new Subject<object>();

        protected ISubject<IEnumerable> ItemsChanges = new Subject<IEnumerable>();



        static FileDbControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FileDbControl), new FrameworkPropertyMetadata(typeof(FileDbControl)));
        }




        public FileDbControl(string extension, Func<string, System.Collections.IEnumerable> outputfunc, Func<string, string> filemap = null)
        {
            Uri resourceLocater = new Uri("/UtilityDAL;component/Themes/FileDbControl.xaml", System.UriKind.Relative);
            ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            Style = resourceDictionary["FileDbStyle"] as Style;


            //Observable.FromEventPattern<EventHandler, EventArgs>(_ => this.Initialized += _, _ => this.Initialized -= _)
            //    .Take(1).Subscribe(_ =>    Init(DirectoryChanges, extension, outputfunc, filemap));

        }


        private void Init(IObservable<string> directories, string extension, Func<string, System.Collections.IEnumerable> outputfunc, Func<string, string> filemap = null)
        {

            directories
                .Where(_ => _ != "")
                .Select(_ => GetDirectoryViewModels(_))
                .Subscribe(_ => this.Dispatcher.InvokeAsync(() => RootDirectories = _, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken)));


            OutputChanges.Where(_ => _ != null)
                .Select(_ => outputfunc(((PathViewModel)_).FilePath))
                .Subscribe(_ => this.Dispatcher.InvokeAsync(() => { Items = _; }, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken)));


            SelectedItemChanges.Where(_ => _ != null)
                .Subscribe(_ =>
                this.Dispatcher.InvokeAsync(() =>
                {
                    Files = System.IO.Directory.GetFiles(new System.IO.DirectoryInfo((_ as PathViewModel).Directory).FullName, "*." + extension, System.IO.SearchOption.AllDirectories)
                    .Select(a_ => new FileViewModel(a_, filemap))
                    .ToArray();
                }, System.Windows.Threading.DispatcherPriority.Background, default(System.Threading.CancellationToken)));
        }

        private IEnumerable<DirectoryViewModel> GetDirectoryViewModels(string path)
            => System.IO.Directory.GetDirectories(path)
              .Select(di =>
              {
                  try { return new DirectoryViewModel(di, a => System.IO.Path.GetFileNameWithoutExtension(a)); }
                  catch { return null; }
              })
              .Where(v => v != null);

    }

}
