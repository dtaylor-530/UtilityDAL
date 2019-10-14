namespace UtilityDAL.View
{
    //public class BiggyControl<T> : DocumentStoreControl<T> where T : new()
    //{
    //    static BiggyControl()
    //    {
    //        DefaultStyleKeyProperty.OverrideMetadata(typeof(BiggyControl<T>), new FrameworkPropertyMetadata(typeof(BiggyControl<T>)));
    //    }

    //    public BiggyControl(Func<object, object> getkey) : base(new UtilityDAL.BiggyRepo<SHDOObject>(_=>(IConvertible)getkey(_.Object)), getkey)
    //    {
    //    }

    //    public object GetKey(T trade)
    //    {
    //        return (trade as Newtonsoft.Json.Linq.JObject)[Key];

    //    }
    //}

    //public class BiggyControl : DocumentStoreControl
    //{
    //    public BiggyControl(Func<object, IConvertible> getKey, string directory = null, string key = null) : base(new UtilityDAL.BiggyRepo<SHDOObject>(getKey), getKey)
    //    {
    //    }
    //    public BiggyControl() : base(new UtilityDAL.BiggyRepo<SHDOObject>())
    //    {
    //    }

    //    //public static IConvertible GetKey(object trade)
    //    //{
    //    //    return 1;// (trade as Newtonsoft.Json.Linq.JObject)[Key];

    //    //}

    //}
}