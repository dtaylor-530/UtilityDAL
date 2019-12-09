namespace UtilityDAL.DemoApp
{
    public class BiggyControl //: UtilityDAL.View.BiggyControl<Pricecsv>
    {
        public BiggyControl() //: base(getKey)
        {
        }

        private static object getKey(object trade) => ((Pricecsv)trade).Date.Ticks;
    }
}