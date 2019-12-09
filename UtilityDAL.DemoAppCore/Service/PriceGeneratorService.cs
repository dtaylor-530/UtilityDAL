namespace UtilityDAL.DemoApp
{
    //public class PriceGeneratorService : IService<IEnumerable<Price>>
    //{
    //    public IObservable<IEnumerable<Price>> Resource => Resource;
    //    IObservable<IEnumerable<Price>> resource;

    //    public PriceGeneratorService(int size)
    //    {
    //        resource = System.Reactive.Linq.Observable.Repeat(Generate(size));
    //    }

    //    internal static IEnumerable<Price> Generate(int count = 50)
    //    {
    //        var nw = new TeaTime.Time(DateTime.Now.Ticks);
    //        var r = new Random();
    //        for (int i = 0; i < count; i++)
    //        {
    //            var price = r.NextDouble() + i;
    //            yield return new Price { Date = nw.AddDays(i), Bid = r.NextDouble() + price, Offer = price };

    //        }

    //    }
    //    //private static IEnumerable<Pricecsv> Generate(int count)
    //    //{
    //    //    var nw = new DateTime(DateTime.Now.Ticks);

    //    //    for (int i = 0; i < count; i++)
    //    //    {
    //    //        var price = r.NextDouble() + i;
    //    //        yield return new Pricecsv { Date = nw.AddDays(i), Bid = r.NextDouble() + price, Offer = price };

    //    //    }

    //    //}
    //}
}