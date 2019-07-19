using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using UtilityWpf.View;
using CH.Bson;
using LazyData;

using LazyData.Json;
using LazyData.Mappings.Mappers;
using LazyData.Mappings.Types;
using LazyData.Registries;
using System.Reflection;
using System.Collections;
using System.Reactive.Linq;
using DynamicData;
using UtilityWpf;
using LiteDB;
using UtilityDAL;
using UtilityWpf.ViewModel;
using UtilityInterface.Generic;
using UtilityDAL.Common;

namespace UtilityDAL.View
{
    //public class LiteDbControl : LiteDbControl<object>
    //{
    //    public LiteDbControl(Func<object, IConvertible> gk) : base(gk) { }
    //}


    public class LiteDbControl : DocumentStoreControl //where T : new()
    {
        public LiteDbControl(Func<object, IConvertible> getKey, string directory = null, string key = null) : base(new UtilityDAL.LiteDbRepo<SHDOObject, IConvertible>(getKey, DbEx.GetDirectory(directory, UtilityDAL.Constants.LiteDbExtension, typeof(IContain<object>))), getKey)
        {
            try
            {
                var mapper = BsonMapper.Global;

                mapper.Entity<SHDOObject>()
                    .Id(x => x.Id) 
                    //.DbRef(x => x.Object, "objects")
                    // set your document ID
                    .Ignore(x => x.Deletions) // ignore this property (do not store)
                      .Ignore(x => x.DeleteCommand);


                mapper.Entity<SHDObject<object>>()
    .Id(x => x.Object, true) // set your document ID
    .Ignore(x => x.Deletions) // ignore this property (do not store)
      .Ignore(x => x.DeleteCommand);

            }
            catch
            {

            }

        }

        public LiteDbControl() : base(new UtilityDAL.LiteDbRepo<SHDOObject, IConvertible>(GetDirectory))
        {

        }


        private static string GetDirectory =>
            UtilityDAL.Common.DbEx.GetDirectory("../../data", UtilityDAL.Constants.LiteDbExtension, typeof(SHDOObject));

    }




    public class LiteDbControl<T> : DocumentStoreControl where T : new()
    {
        public static readonly DependencyProperty MapperProperty = DependencyProperty.Register("Mapper", typeof(BsonMapper), typeof(LiteDbControl<T>), new PropertyMetadata(LiteDB.BsonMapper.Global, MapperChange));

        public BsonMapper Mapper
        {
            get { return (BsonMapper)GetValue(MapperProperty); }
            set { SetValue(MapperProperty, value); }
        }

        private static void MapperChange(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as LiteDbControl<T>).MapperSubject.OnNext((BsonMapper)e.NewValue);
        }
        protected ISubject<BsonMapper> MapperSubject = new Subject<BsonMapper>();


        //static LiteDbControl()
        //{
        //    UtilityWpf.View.CollectionEditor.InputProperty.OverrideMetadata(typeof(LiteDbControl<T>), new PropertyMetadata(default(DatabaseCommand), DatabaseCommandChange));
        //    DefaultStyleKeyProperty.OverrideMetadata(typeof(LiteDbControl<T>), new FrameworkPropertyMetadata(typeof(LiteDbControl<T>)));
        //}


        public LiteDbControl(Func<object, object> getKey, string directory = null, string key = null) : base(GetRepo(_ => (IConvertible)getKey(((SHDOObject)_).Object), directory), getKey)
        {
            //Uri resourceLocater = new Uri("/UtilityDAL;component/Themes/LiteDbControl.xaml", System.UriKind.Relative);
            //ResourceDictionary resourceDictionary = (ResourceDictionary)Application.LoadComponent(resourceLocater);
            //Style = resourceDictionary["LiteDbStyle"] as Style;
        }

        //private static LiteDbRepo<SHDObject<T>, IConvertible> GetRepo(Func<object, object> getKey, string directory)
        //    => new LiteDbRepo<SHDObject<T>, IConvertible>(_ => (IConvertible)getKey(_), UtilityDAL.DbEx.GetDirectory<T>(directory, UtilityDAL.Constants.LiteDbExtension));

        private static LiteDbRepo<SHDOObject, IConvertible> GetRepo(Func<object, object> getKey, string directory)
        => new LiteDbRepo<SHDOObject, IConvertible>(_ => (IConvertible)getKey(_), UtilityDAL.Common.DbEx.GetDirectory<T>(directory, UtilityDAL.Constants.LiteDbExtension));


    }





}


