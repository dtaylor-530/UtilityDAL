using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UtilityDAL
{
    public class FilexBuilder
    {
        public static List<Filex> GetFiles(string dir)
        {

            return System.IO.Directory
                .GetFiles(dir)
                .Select(_ => GetFilex(_))
                .ToList();

        }

        public static List<KeyValuePair<DateTime, System.IO.FileSystemEventArgs>> GetChanges(string dir)
        {

            return System.IO.Directory
                .GetFiles(dir)
                .Select(_ => GetFileChanges(_))
                .SelectMany(_=>_)
                .ToList();

        }


        public static Filex GetFilex(string path)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(path);
            var parent = System.IO.Path.GetDirectoryName(path);

            return new Filex
            {
                Changes =  GetFileChanges(path),
                Name = name,
                Parent = parent,
                Path = path
            };

        }

        public static List<KeyValuePair<DateTime, System.IO.FileSystemEventArgs>> GetFileChanges(string path)
        {
            var name = System.IO.Path.GetFileNameWithoutExtension(path);
            var parent = System.IO.Path.GetDirectoryName(path);

            return new List<KeyValuePair<DateTime, System.IO.FileSystemEventArgs>>
                {
                    new KeyValuePair<DateTime, System.IO.FileSystemEventArgs>(System.IO.File.GetCreationTime(path), new System.IO.FileSystemEventArgs(System.IO.WatcherChangeTypes.Created, parent, name)),
                    new KeyValuePair<DateTime, System.IO.FileSystemEventArgs>(System.IO.File.GetLastWriteTime(path), new System.IO.FileSystemEventArgs(System.IO.WatcherChangeTypes.Changed, parent, name)),
                };

            

        }
    }

}
