using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Unity;
using Unity.Injection;
using Unity.Lifetime;
using Unity.RegistrationByConvention;
using Unity.Resolution;
using UtilityWpf.ViewModel;
using UtilityDAL.Model;
using UtilityDAL.ViewModel;


namespace UtilityDAL.DemoApp
{

    public class MainViewModelLocator
    {
        static MainViewModelLocator()
        {
            Container = new UnityContainer();

            //if (UtilityWpf.DesignModeHelper.IsInDesignModeStatic)
                //if in design mode, use design data service
                //Container.RegisterType<IService<DataFile>, DummyTeaDataService>();

            //else
            //    Container.RegisterType<IDataService<Thing>, ThingService>();
            //Container.Resolve<IDispatcherService>(new ParameterOverride("dispatcher", Application.Current.Dispatcher));


            InjectionConstructor injectionConstructor = new InjectionConstructor(Application.Current.Dispatcher);

            Container.RegisterType<UtilityWpf.IDispatcherService, UtilityWpf.DispatcherService>(injectionConstructor);


            //Container.RegisterType<IRepoChangeService<DummyDbObject>, RepoChangeService >();



              //Container.RegisterType<MainViewModel>(new ContainerControlledLifetimeManager());

            //Container.RegisterType<TeaProviderDummyViewModel>(new ContainerControlledLifetimeManager());

            //Container.RegisterType<CsvProviderDummyViewModel>(new ContainerControlledLifetimeManager());

            Container.RegisterType<PaginatedViewModel>(new ContainerControlledLifetimeManager());

            //Container.RegisterType<LiteDbDummyViewModel>(new ContainerControlledLifetimeManager());

            Container.RegisterType<LiteDbDummyViewModel>(new ContainerControlledLifetimeManager());

            //Container.RegisterType<ToolKitViewModel>(new ContainerControlledLifetimeManager());


        }




        public static IUnityContainer Container
        {
            get;
            private set;
        }

        //public MainViewModel MainVM
        //{
        //    get
        //    {
        //        var vm = Container.Resolve<MainViewModel>().TeaTimeVM;
        //        return vm;
        //    }
        //}

        //public TeaProviderDummyViewModel TeaProviderVM
        //{
        //    get
        //    {
        //        var vm = Container.Resolve<TeaProviderDummyViewModel>();
        //        return vm;
        //    }
        //}

        //public CsvProviderDummyViewModel CsvProviderVM
        //{
        //    get
        //    {
        //        var vm = Container.Resolve<CsvProviderDummyViewModel>();
        //        return vm;
        //    }
        //}

        public PaginatedViewModel MainVM
        {
            get
            {
                var vm = Container.Resolve<PaginatedViewModel>();
                return vm;
            }
        }


        public LiteDbDummyViewModel LiteDbVM
        {
            get
            {
                var vm = Container.Resolve<LiteDbDummyViewModel>();
                return vm;
            }
        }

        public BiggyViewModel BiggyVM
        {
            get
            {
                var vm = Container.Resolve<BiggyViewModel>();
                return vm;
            }
        }

        //public ToolKitViewModel ToolKitVM
        //{
        //    get
        //    {
        //        var vm = Container.Resolve<ToolKitViewModel>();
        //        return vm;
        //    }
        //}

        public static void Cleanup()
        {
            //Container.Resolve<MainViewModel>().Cleanup();
        }
    }

}
