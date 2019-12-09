using ReactiveUI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using UtilityWpf;
using UtilityWpf.ViewModel;

namespace UtilityDAL.ViewModel
{
    public class DirectoriesViewModel : ReactiveObject
    {
        private object _output;
        private ObservableAsPropertyHelper<IEnumerable<DirectoryViewModel>> rootDirectories;
        private ObservableAsPropertyHelper<IEnumerable> items;
        private ObservableAsPropertyHelper<IEnumerable<FileViewModel>> files;

        //private List<DirectoryViewModel> selectedItems = new List<DirectoryViewModel>();


        public IEnumerable<DirectoryViewModel> RootDirectories => rootDirectories.Value;
        public IEnumerable<FileViewModel> Files => files.Value;

        public System.Collections.IEnumerable Items => items.Value;

        public object Output
        {
            get { return _output; }
            set => this.RaiseAndSetIfChanged(ref _output,  value);
        }


        //public ReactiveProperty<System.Collections.IEnumerable> SelectedItems { get; } = new ReactiveProperty<System.Collections.IEnumerable>();
        public object SelectedItem { get; } 

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
            rootDirectories = directories.Where(_ => _ != "").Select(_ =>
            {
                return System.IO.Directory.GetDirectories(_).Select(di =>
                {
                    try { return new DirectoryViewModel(di, a => System.IO.Path.GetFileNameWithoutExtension(a)); }
                    catch { return null; }
                }).Where(v => v != null);
            }).ToProperty(this, a=>a.RootDirectories);

            items = this.WhenAnyValue(a => a.Output).Where(_ => _ != null).Select(_ =>
               {
                   var x = outputfunc(((PathViewModel)_).FilePath);
                   return x;
               }).ToProperty(this, a => a.Items);

            files = this.WhenAnyValue(a => a.SelectedItem).Where(_ => _ != null).Select(p =>
            {
                PathViewModel value = p as PathViewModel;
                var yt = new System.IO.DirectoryInfo(value.Directory);

                var fvms = System.IO.Directory
                .GetFiles(yt.FullName, "*." + extension, System.IO.SearchOption.AllDirectories)
                .Select(a_ => new FileViewModel(a_, filemap)).ToArray();
                return fvms;
            }).ToProperty(this, a => a.Files);
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