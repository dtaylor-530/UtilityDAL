using System;
using System.Collections.Generic;
using System.Text;
using UtilityDAL.Common;

namespace UtilityDAL
{
    public class LiteDbSaveLoadService<T,R> : DocumentSaveLoadService<T,R>
    {

        public LiteDbSaveLoadService(Func<T,R> key, string directory = null)
        {
            Service = new LiteDbRepo<T,R>(key, DbEx.GetDirectory(directory, Constants.LiteDbExtension, typeof(T)));
        }

    }


}
