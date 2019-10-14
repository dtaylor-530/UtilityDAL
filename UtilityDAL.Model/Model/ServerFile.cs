using System;
using System.IO;

namespace UtilityDAL.Model
{
    public class ServerFile
    {
        public string Name { get; set; }

        public string Link { get; set; }

        public FileInfo File { get; set; }

        public DateTime Download { get; set; }

        public DateTime Upload { get; set; }

        public bool OutOfDate => Download < Upload;
    }
}