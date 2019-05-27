using UtilityDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UtilityDAL.DemoApp
{
    public class BiggyControl : UtilityDAL.View.BiggyControl<Pricecsv>
    {
        public BiggyControl() : base(getKey)
        {

        }
        private static object getKey(object trade) => ((Pricecsv)trade).Date.Ticks;
    }

}
