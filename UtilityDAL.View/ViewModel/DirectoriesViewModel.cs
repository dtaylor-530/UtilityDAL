using Reactive.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using UtilityWpf;
using UtilityWpf.ViewModel;

namespace UtilityDAL.ViewModel
{
    public class DirectoriesViewModel : NPC
    {
        private object _output;

        //private List<DirectoryViewModel> selectedItems = new List<DirectoryViewModel>();

        public ReadOnlyReactiveProperty<IEnumerable<DirectoryViewModel>> RootDirectories { get; private set; }
        public ReactiveProperty<IEnumerable<FileViewModel>> Files { get; private set; } = new ReactiveProperty<IEnumerable<FileViewModel>>();

        public object Output
        {
            get { return _output; }
            set { _output = value; OnPropertyChanged(); }
        }

        public ReadOnlyReactiveProperty<System.Collections.IEnumerable> Items { get; private set; }

        //public ReactiveProperty<System.Collections.IEnumerable> SelectedItems { get; } = new ReactiveProperty<System.Collections.IEnumerable>();
        public ReactiveProperty<object> SelectedItem { get; } = new ReactiveProperty<object>();

        public DirectoriesViewModel(string directories, UtilityWpf.IDispatcherService ds, string extension, Func<string, System.Collections.IEnumerable> outputfunc, Func<string, string> filemap = null)
        {
            Init(Observable.Start(() => directories), ds, extension, outputfunc, filemap);
        }

        public DirectoriesViewModel(IObservable<string> directories, UtilityWpf.IDispatcherService ds, string extension, Func<string, System.Collections.IEnumerable> outputfunc, Func<string, string> filemap = null)
        {
            Init(directories, ds, extension, outputfunc, filemap);
        }

        private void Init(IObservable<string> directories, UtilityWpf.IDispatcherService ds, string extension, Func<string, System.Collections.IEnumerable> outputfunc, Func<string, string> filemap = null)
        {
            RootDirectories = directories.Where(_ => _ != "").Select(_ =>
            {
                return System.IO.Directory.GetDirectories(_).Select(di =>
                {
                    try { return new DirectoryViewModel(di, a => System.IO.Path.GetFileNameWithoutExtension(a)); }
                    catch { return null; }
                }).Where(v => v != null);
            }).ToReadOnlyReactiveProperty();

            Items = this.OnPropertyChange<NPC, object>(nameof(Output)).Where(_ => _ != null).Select(_ =>
             {
                 var x = outputfunc(((PathViewModel)_).FilePath);
                 return x;
             }).ToReadOnlyReactiveProperty();

            SelectedItem.Where(_ => _ != null).Subscribe(_ =>
            {
                PathViewModel value = _ as PathViewModel;
                var yt = new System.IO.DirectoryInfo(value.Directory);

                var fvms = System.IO.Directory
                .GetFiles(yt.FullName, "*." + extension, System.IO.SearchOption.AllDirectories)
                .Select(a_ => new FileViewModel(a_, filemap)).ToArray();
                Files.Value = fvms;
            });
            //SelectedItems.Where(_ => _ != null).Subscribe(_ =>
            //{
            //    IEnumerable<PathViewModel> value = _ as IEnumerable<PathViewModel>;
            //    var yt = value.Select(a_ => new System.IO.DirectoryInfo(a_.Directory)).ToArray();
            //    var fvms = yt.GetFilesInSubDirectories("*." + extension).Select(a_ => new FileViewModel(a_, filemap)).ToArray();
            //    Files.Value = fvms;

            //});
        }
    }
}