using System;
using System.Collections.Generic;
using System.Linq;

namespace UtilityDAL.View
{
    internal static class PathHelper
    {
        public static string[] GetFilesInSubDirectories(this IList<System.IO.FileSystemInfo> value, string pattern = "*.*") =>

               (value)?.SelectMany(_ => System.IO.Directory.GetFiles(_.FullName, "*.*", System.IO.SearchOption.AllDirectories)).ToArray();

        public static string FileMap(string path)
        {
            var file = System.IO.Path.GetFileName(path).Replace(System.IO.Path.GetExtension(path), "");
            return DateTime.FromFileTime(Convert.ToInt64(file)).ToShortDateString();
        }
    }
}