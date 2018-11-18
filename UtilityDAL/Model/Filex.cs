
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL
{
    public class Filex
    {
        //public ulong Hash { get; set; }
        //public string Key { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Parent { get; set; }//{ get { return System.IO.Directory.GetParent(Path); } }
        public ICollection<KeyValuePair<DateTime,System.IO.FileSystemEventArgs>> Changes { get; set; }
    }





}
