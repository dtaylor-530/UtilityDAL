using System.Collections.Generic;

namespace UtilityDAL.Contract
{
    public interface IFileEventsRepository
    {
        List<System.IO.FileSystemEventArgs> GetNewOrChangedFiles();
    }
}