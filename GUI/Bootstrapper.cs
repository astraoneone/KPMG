using GUI.ViewModels;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System.Diagnostics;
using System.Windows;

namespace GUI
{
    public class Bootstrapper : UnityBootstrapper
    {
        /// <summary>
        /// Register types with unity. 
        /// Method is shared with SpecFlow when configuring unity in the <see cref="UnityContext"/> class.
        /// </summary>
        /// <param name="container">The container used to register types</param>
        public static void RegisterTypes(IUnityContainer container)
        {
            ////container.RegisterType<IAllLines, AllLines>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<ITimeService, TimeService>();
            ////container.RegisterType<ITimerService, TimerService>(new InjectionConstructor(TimeSpan.FromSeconds(1)));
            ////container.RegisterType<INavigationService, NavigationService>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<IUrlSanitizer, UrlSanitizer>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<IWebApiDriver, WebApiDriver>();
            ////container.RegisterType<IRetry<HttpResponseMessage>, Retry<HttpResponseMessage>>();
            ////container.RegisterType<IRetryPolicy, ExponentialHoldOffRetryPolicy>();

            ////container.RegisterType<IOperatorDiagnostics, OperatorDiagnostics>();
            ////container.RegisterType<IDeveloperDiagnostics, DeveloperDiagnostics>();
            ////container.RegisterType<IPrismDiagnostics, PrismDiagnostics>();

            ////container.RegisterType<DevelopmentWindowViewModel>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<IDomainModelConversion<ApplicationSettingsDto, IApplicationSetting>, ApplicationSettingsConversion>();
            ////container.RegisterType<IApplicationSettingsRepository, ApplicationSettingsRepository>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<IUserSettingsRepository, UserSettingsRepository>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<IConfiguration, Configuration>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<ILocalWorkstation, LocalWorkstation>(new ContainerControlledLifetimeManager());
            ////container.RegisterType<ICompositeCommandAggregator, CompositeCommandAggregator>(new ContainerControlledLifetimeManager());
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.RegisterType<MainWindowViewModel>(new ContainerControlledLifetimeManager());

            RegisterTypes(this.Container);
        }

        protected override RegionAdapterMappings ConfigureRegionAdapterMappings()
        {
            var mappings = base.ConfigureRegionAdapterMappings();

            return mappings;
        }

        protected override DependencyObject CreateShell()
        {
            var shell = this.Container.Resolve<MainWindow>();
            Application.Current.MainWindow = shell;
            this.Container.RegisterInstance<Window>(Application.Current.MainWindow);
            shell.SnapsToDevicePixels = true;
            shell.Show();

            return shell as DependencyObject;
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            if (Debugger.IsAttached)
            {
                return new SubfolderModuleCatalog { ModulePath = "." };
            }
            else
            {
                return new DirectoryModuleCatalog { ModulePath = "Modules" };
            }
            
        }

        protected override void InitializeModules()
        {
            base.InitializeModules();

            this.Container.Resolve<MainWindow>().DataContext = this.Container.Resolve<MainWindowViewModel>();

        }
    }
}
