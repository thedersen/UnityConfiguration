# UnityConfiguration v1.4.1

Convention based configuration for the Microsoft Unity IoC container. With just a few lines of code, you can now registere all your classes in the entire solution. If the built-in conventions doesn't fit your needs, it is very easy to extend with your own.

## Download and install

UnityConfiguration is available for download on [NuGet](http://nuget.org/packages/UnityConfiguration).

To install it run the following command in the [NuGet Package Manager Console](http://docs.nuget.org/docs/start-here/using-the-package-manager-console).

	PM> Install-Package UnityConfiguration
   
This will download all the binaries, and add necessary references to your project.

## Configuring your container

You get access to the configuration api by using the extension method Configure on the IUnityContainer interface.

	var container = new UnityContainer();
	container.Configure(x =>
	{
		x.AddRegistry<FooRegistry>();
		x.AddRegistry(new BarRegistry());
	});

## Configure using registries

Configuration is done in one or several registries that inherit the UnityRegistry base class.

	public class FooRegistry : UnityRegistry
	{
		public FooRegistry()
		{
			// Scan one or several assemblies and auto register types based on a 
			// convention. You can also include or exclude certain types and/or 
			// namespaces using a filter.
			Scan(scan =>
			{
				scan.AssembliesInBaseDirectory();
				scan.ForRegistries();
				scan.With<FirstInterfaceConvention>();
				scan.With<AddAllConvention>()
					.TypesImplementing<IHaveManyImplementations>();
				scan.With<SetAllPropertiesConvention>().OfType<ILogger>();
				scan.ExcludeType<FooService>();
			});

			// Manually register a service
			Register<IFooService, FooService>().WithName("Foo").AsSingleton();

			// Make services a singleton. Useful for types registered by the scanner.
			Configure<ISingletonService>().AsSingleton();

			// If you want to inject values or objects that are not registered in
			// the container, you can do so by adding them using this statement.
			// For instances you want the container to create, just specify the type.
			Configure<ServiceWithCtorArgs>().WithConstructorArguments("some string", typeof (IFooService));

			// Unity follows the greedy constructor pattern when creating instances.
			// If you want to use a different constructor, you specify it by listing 
			// the types of the arguments in the constructor you want it to use.
			SelectConstructor<ServiceWithCtorArgs>(typeof (IFooService));

			// You can automatically configure the container to call
			// a method on any service when they are created
			AfterBuildingUp<IStartable>().Call((c, s) => s.Start());

			// You can automatically configure the container to 
			// decorate services when they are created
			AfterBuildingUp<IFooService>().DecorateWith((c, t) => new FooDecorator(t));
		}
	}
	
## Custom conventions

At the moment, built in conventions includes AddAllConvention, FirstInterfaceConvention, NamingConvention, SetAllPropertiesConvention and ScanForRegistriesConvention. If these doesn't suit you, creating custom conventions is as easy as creating a class that implements the IAssemblyScanner interface.

	public class CustomConvention : IAssemblyScannerConvention
	{
		void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
		{
			if (type == typeof(FooService))
				registry.Register<IFooService, FooService>().WithName("Custom");
		}
	}
	
## Release Notes

#### v1.4.1
* Fixed #6: AssemblyScanner not bin folder friendly for web apps
* Added #5: `IUnityContainer.Configure()` returns the `IUnityContainer` so other calls can be chained

#### v1.4
* Added extension methods to the `IAssemblyScanner` for easier discovery and configuration of conventions
* Added non-generic overload to the `AddAllConvention`
* Added option to scan for internal types in an assembly - `IAssemblyScanner.InternalTypes()`
* Strong named the assembly
* Switched to [Semantic Versioning](http://semver.org/)

#### v1.3
* Documented public facing methods
* Included the xml documentation in the NuGet package
* Debugging symbols are available on SymbolSource.org
* Several more overloads for adding assemblies to the scanner
* One more overload for adding a registry that takes an instance of a UnityRegistry
* New convention that scans for registries
* New convention that uses an overridable naming convention for registering a type mapping
* MakeSingleton() and MakeSingleton(string) is marked as obsolete. Use Configure().AsSingleton() or Configure().WithName(name).AsSingleton() instead
* ConfigureCtorArgsFor(params object[] args) is marked as obsolete. Use Configure().WithConstructorArguments(params object[]) instead
* Added Transient, PerThread, PerResolve, ExternallyControlled and HierarchicalControlled lifetime scope configuration

## License

http://thedersen.mit-license.org/