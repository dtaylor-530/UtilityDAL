using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UtilityDAL.Model;

namespace UtilityDAL
{
    public static class FilexToKeyValueDate
    {

        public static KeyValueDate ToKeyValueDate(this Filex fx)
        {
            return new KeyValueDate
            {
                Date= fx.Date.Ticks,
                Key=fx.Name,
                Value=fx.FileInfo.FullName
            };

        }
    }


    public static class KeyValueDateToFilex
    {
        public static Filex ToFileWrite(this KeyValueDate kvd)
        {

            return new Filex
            {
                Date = new DateTime(kvd.Date),
                Name = kvd.Key,
                FileInfo =new System.IO.FileInfo(kvd.Value)
            };

        }
    }
}
