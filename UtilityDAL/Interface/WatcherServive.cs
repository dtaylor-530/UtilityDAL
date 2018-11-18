using System;
using System.Collections.Generic;
using System.Text;
using System.Reactive.Linq;

namespace UtilityDAL
{
    public interface IDirectoryWatcher
    {
        IObservable<System.IO.FileSystemEventArgs> Changes { get; }
    }



    public interface IFileEventsRepository
    {
        List<System.IO.FileSystemEventArgs> GetNewOrChangedFiles();

    }


}

