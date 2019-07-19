//using Biggy.Core;
//using Biggy.Data.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;


namespace UtilityDAL
{


    //public class BiggyDirectory
    //{

    //    //private JsonStore<Filex> store;
    //    //private string _folderpath;
    //    //private string _filter;
    //    //public ReadOnlyObservableCollection<FilexVM> Files { get; private set; }
    //    //private ObservableCollection<FilexVM> Files { get; private set; }



    //    public IObservable<Filex> Changes { get; }


    //    public BiggyDirectory(IObservable<KeyValuePair<DateTime, System.IO.FileSystemEventArgs>> eventargs, string folderpath, string filter)
    //    {
    //        //_folderpath = folderpath;
    //        Biggy.Data.Json.JsonStore<Filex> store;
    //        BiggyList<Filex> comps = null;
    //        try
    //        {
    //            store = new Biggy.Data.Json.JsonStore<Filex>("3ltyd3kkk");// System.IO.Path.GetFileName(folderpath));
    //            comps = new BiggyList<Filex>(store);
    //        }
    //        catch
    //        {

    //        }

    //        if (comps != null)
    //            Changes = eventargs.StartWith(FilexBuilder.GetChanges(folderpath))
    //                  .Where(_ => System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(_.Value.FullPath)) == System.IO.Path.GetFullPath(folderpath))
    //                  .Select(_ =>
    //                  {

    //                      Filex xl = null;
    //                      try
    //                      {
    //                          xl = comps.SingleOrDefault(a => a.Name == _.Value.Name);
    //                      }
    //                      catch (Exception e)
    //                      {
    //                          throw e;
    //                      }
    //                      if (xl != null)
    //                      {
    //                          xl.Changes.Add(_);
    //                          comps.Update(xl);
    //                      }
    //                      else
    //                      {
    //                          var filex = FilexBuilder.GetFilex(_.Value.FullPath);
    //                          comps.Add(filex);
    //                      }


    //                      return comps;
    //                  })
    //                  .SelectMany(_ => _);


    //    }
    //}



    //public class BiggyDirectory 
    //{

    //    private JsonStore<Filex> store;
    //    //private string _folderpath;
    //    //private string _filter;
    //    public ICollection<Filex> Files { get; private set; }

    //    public BiggyDirectory(IObservable<KeyValuePair<DateTime,System.IO.FileSystemEventArgs>> eventargs,string folderpath)
    //    {
    //        //_folderpath = folderpath;
    //        store = new Biggy.Data.Json.JsonStore<Filex>(folderpath);
    //        var comps = new BiggyList<Filex>(store);
    //        eventargs
    //            .Where(_=> System.IO.Path.GetDirectoryName(_.Value.FullPath)==folderpath )
    //            .Subscribe(_ =>
    //        {
    //            var x=comps.SingleOrDefault(a => a.Name == _.Value.Name);
    //            if (x == null)
    //                comps.Add(FilexBuilder.GetFilex(x.Path));
    //            else
    //                x.Changes.Add(_);

    //            comps.Update(x);
                
    //            Files = comps;
    //        });
    //    }


    //    //private ICollection<Filex> GetCurrentFiles()
    //    //{
    //    //    store = store??new Biggy.Data.Json.JsonStore<Filex>(nameof(Filex));
    //    //    var comps = new BiggyList<Filex>(store);

    //    //    //var ff = System.IO.Directory.GetFiles(_folderpath, _filter);

    //    //    //if (comps.Count() == 0)
    //    //    //{
    //    //    //    foreach (var x in ff)
    //    //    //    {
    //    //    //        comps.Add(FileEx.Create(x));

    //    //    //    }
    //    //    //}


    //    //    return comps;

    //    //}




    //    //public List<System.IO.FileSystemEventArgs> GetNewOrChangedFiles()
    //    //{
    //    //    var dbfiles = GetCurrentFiles();
    //    //    var ff = System.IO.Directory.GetFiles(_folderpath/*, _filter*/);
    //    //    List<System.IO.FileSystemEventArgs> files = new List<System.IO.FileSystemEventArgs>();
    //    //    return ff.Select(f =>
    //    //   {
    //    //       if (dbfiles.FirstOrDefault(_ => _.Path == f)?.Hash == null ? true : dbfiles.FirstOrDefault(_ => _.Path == f)?.Hash != (ulong?)FileEx.GetHash(f))
    //    //           return (new System.IO.FileSystemEventArgs(System.IO.WatcherChangeTypes.Created, _folderpath, System.IO.Path.GetFileNameWithoutExtension(f)));
    //    //       return null;
    //    //   }).Where(_ => _ != null).ToList();

    //    //}



    //    //public void UpdateCurrentFiles(IObservable<KeyValuePair<string, bool>> observable)
    //    //{
    //    //    var comps = new BiggyList<Filex>(store);
    //    //    observable.Subscribe(_ =>
    //    //    {
    //    //        if (_.Value)
    //    //        {
    //    //            var xx = comps.Single(_k => _k.Path == _.Key);
    //    //            xx.Hash = FileEx.GetHash(_.Key);
    //    //            comps.Update(xx);
    //    //        }
    //    //        else
    //    //        {

    //    //        }
    //    //    });
    //    //}
    //}
}