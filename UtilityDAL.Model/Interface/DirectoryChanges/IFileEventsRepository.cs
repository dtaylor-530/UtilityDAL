using System;
using System.Collections.Generic;
using System.Text;

namespace UtilityDAL.Contract
{



    public interface IFileEventsRepository
    {
        List<System.IO.FileSystemEventArgs> GetNewOrChangedFiles();

    }


}

