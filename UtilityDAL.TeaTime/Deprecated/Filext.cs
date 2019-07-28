
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeaTime;

namespace UtilityDAL.Teatime
{

    //public struct Filext
    //{
    //    public Guid Key { get; set; }
    //    public Guid Path { get; set; }
    //    public Time Time { get; set; }
    //}

    //public static class FilextFactory
    //{

    //    public static Filext Create(string path, string name = null) => new Filext
    //    //var parent = System.IO.Path.GetDirectoryName(path);
    //    {
    //        //Changes =  GetFileChanges(path),
    //        Time = DateTime.Now,
    //        //Key = name ?? System.IO.Path.GetFileNameWithoutExtension(path),
    //        //Path = BitConverter.ToInt64(SimpleBase.Base32.Crockford.Decode(path).ToArray(),0),
    //    };




    //    public static IEnumerable<Filext> CreateByDirectory(string dir)
    //    {
    //        return from file in System.IO.Directory.GetFiles(dir)
    //               select Create(file);

    //    }
    //}
}
