//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace UtilityDAL.Factory
//{
//    public static class Filex
//    {

//        public static Model.Filex Create(string path,string name=null) => new Model.Filex
//        //var parent = System.IO.Path.GetDirectoryName(path);
//        {
//            //Changes =  GetFileChanges(path),
//            Date=DateTime.Now,
//            Name = name??System.IO.Path.GetFileNameWithoutExtension(path),
//            FileInfo = new System.IO.FileInfo(path),
//        };




//        public static IEnumerable<Model.Filex> CreateByDirectory(string dir)
//        {
//            return from file in System.IO.Directory.GetFiles(dir)
//                   select Create(file);

//        }
//    }

//    public static class FilextFactory
//    {

//        public static Filext Create(string path, string name = null) => new Filext
//        //var parent = System.IO.Path.GetDirectoryName(path);
//        {
//            //Changes =  GetFileChanges(path),
//            Time = DateTime.Now,
//            //Key = name ?? System.IO.Path.GetFileNameWithoutExtension(path),
//            //Path = BitConverter.ToInt64(SimpleBase.Base32.Crockford.Decode(path).ToArray(),0),
//        };




//        public static IEnumerable<Filext> CreateByDirectory(string dir)
//        {
//            return from file in System.IO.Directory.GetFiles(dir)
//                   select Create(file);

//        }
//    }
//}
