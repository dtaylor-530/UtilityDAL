using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityDAL.View
{
    public interface IItemsSource
    {
        IEnumerable ItemsSource { get; set; }
    }

}
