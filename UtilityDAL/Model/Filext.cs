
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL
{

    public struct Filext
    {
        public Guid Key { get; set; }
        public Guid Path { get; set; }
        public TeaTime.Time Time { get; set; }
    }

}
