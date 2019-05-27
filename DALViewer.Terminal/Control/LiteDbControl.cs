using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityWpf.ViewModel;

namespace UtilityDAL.DemoApp
{
    public class LiteDbControl : View.LiteDbControl<DummyDbObject>
    {
        public LiteDbControl() : base(_ => ((DummyDbObject)_).Id) { }
    }


}
