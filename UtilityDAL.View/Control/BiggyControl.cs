using MongoDB.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using UtilityWpf;
using UtilityWpf.View;
using UtilityWpf.ViewModel;

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



