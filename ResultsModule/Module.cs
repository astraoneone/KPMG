namespace ResultsModule
{
    using GuiShared;
    using Microsoft.Practices.Unity;
    using Prism.Events;
    using Prism.Modularity;
    using Prism.Regions;
    using System.Net.Http;
    using ViewModels;
    using Views;

    [Module(ModuleName = "Results")]
    public class Module : IModule
    {
        private readonly IUnityContainer unity;
        private readonly IRegionViewRegistry regionViewRegistry;

        public Module(IUnityContainer unity, IRegionViewRegistry regionViewRegistry)
        {
            this.unity = unity;
            this.regionViewRegistry = regionViewRegistry;
        }

        public void Initialize()
        {
            this.RegisterDataAccessTypes();
            this.RegisterBusinessTypes();
            this.RegisterViews();
            this.ResolveHubs();
        }

        public void RegisterBusinessTypes()
        {
            this.unity.RegisterType<ResultsViewModel>(new ContainerControlledLifetimeManager());
        }

        private void ResolveHubs()
        {
        }

        private void RegisterViews()
        {
            this.regionViewRegistry.RegisterViewWithRegion(RegionNames.ResultsRegion, typeof(ResultsView));
            ////this.componentService.Add(new CreatableComponent<ExampleView>(ComponentName.Eisec, "Eisec Search History", 1, true));
        }

        private void RegisterDataAccessTypes()
        {
            this.unity.RegisterType<HttpClient>(new ContainerControlledLifetimeManager());
            ////this.unity.RegisterType<ICallerLocationQueryService, CallerLocationQueryService>(new ContainerControlledLifetimeManager());
            ////this.unity.RegisterType<ICallerLocationQueryServiceConfig, CallerLocationQueryServiceConfig>(new ContainerControlledLifetimeManager());
        }
    }
}
