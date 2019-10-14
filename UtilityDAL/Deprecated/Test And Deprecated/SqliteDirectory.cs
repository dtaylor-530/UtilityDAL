namespace UtilityDAL
{
    //public class SqliteDirectory
    //{
    //private JsonStore<Filex> store;
    //private string _folderpath;
    //private string _filter;
    //public ReadOnlyObservableCollection<FilexVM> Files { get; private set; }
    //private ObservableCollection<FilexVM> Files { get; private set; }

    //    public IObservable<Filex> Changes { get; }

    //    public SqliteDirectory(IObservable<KeyValuePair<DateTime, System.IO.FileSystemEventArgs>> eventargs, string folderpath, string filter)
    //    {
    //        //_folderpath = folderpath;
    //        Biggy.Data.Json.JsonStore<Filex> store;
    //        BiggyList<Filex> comps = null;
    //        try
    //        {
    //            store = new Biggy.Data.Json.JsonStore<Filex>("3ltyd3kk");// System.IO.Path.GetFileName(folderpath));
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
    //}
}