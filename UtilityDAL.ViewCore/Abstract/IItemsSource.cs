using System.Collections;

namespace UtilityDAL.View
{
    public interface IItemsSource
    {
        IEnumerable ItemsSource { get; set; }
    }
}