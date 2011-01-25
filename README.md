Convention based configuration API for the Unity IoC container. Heavily influenced by StructureMap.

###Scanning

	var container = new UnityContainer();

	container.Configure(x => x.Scan(scan =>
	{
		scan.AssemblyContaining<FooRegistry>();
		scan.With<FirstInterfaceConvention>();
		scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
	}));

###Working with registries

	var container = new UnityContainer();

	container.Configure(x =>
	{
		x.AddRegistry<FooRegistry>();
		x.AddRegistry<BarRegistry>();
	});

	public class FooRegistry : UnityRegistry
    {
        public FooRegistry()
        {
            Scan(scan =>
                 {
                     scan.AssemblyContaining<FooRegistry>();
                     scan.With<FirstInterfaceConvention>();
                     scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
                 });

            Register<IFooService, FooService>().WithName("Foo");

            MakeSingleton<ISingletonService>();
        }
    }