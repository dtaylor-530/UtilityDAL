using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityDAL.Factory
{
    public static class Filex
    {

        public static Model.Filex Create(string path,string name=null) => new Model.Filex
        //var parent = System.IO.Path.GetDirectoryName(path);
        {
            //Changes =  GetFileChanges(path),
            Date=DateTime.Now,
            Name = name??System.IO.Path.GetFileNameWithoutExtension(path),
            FileInfo = new System.IO.FileInfo(path),
        };




        public static IEnumerable<Model.Filex> CreateByDirectory(string dir)
        {
            return from file in System.IO.Directory.GetFiles(dir)
                   select Create(file);

        }
    }

   
}
