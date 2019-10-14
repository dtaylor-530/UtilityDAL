using System;

namespace UtilityDAL.Contract
{
    public interface IDirectoryWatcher
    {
        IObservable<System.IO.FileSystemEventArgs> Changes { get; }
    }
}