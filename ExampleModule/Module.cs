namespace ExampleModule
{
    using GuiShared;
    using Microsoft.Practices.Unity;
    using Prism.Modularity;
    using Prism.Regions;
    using System.Net.Http;
    using ViewModels;
    using Views;

    [Module(ModuleName = "Example")]
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
            this.unity.RegisterType<ExampleViewModel>(new ContainerControlledLifetimeManager());
        }

        private void ResolveHubs()
        {
        }

        private void RegisterViews()
        {
            this.regionViewRegistry.RegisterViewWithRegion(RegionNames.ExampleRegion, typeof(ExampleView));
        }

        private void RegisterDataAccessTypes()
        {
            this.unity.RegisterType<HttpClient>(new ContainerControlledLifetimeManager());
        }
    }
}
