using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Contract
{
    public interface IDirectoryWatcher
    {
        IObservable<System.IO.FileSystemEventArgs> Changes { get; }
    }

}
