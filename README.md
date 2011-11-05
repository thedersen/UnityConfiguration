UnityConfiguration
==================
Convention based configuration API for the Unity IoC container. Heavily influenced by StructureMap.

Configuring your container
--------------------------
You get access to the configuration api by using the extension method Configure on the IUnityContainer interface.

	var container = new UnityContainer();
	container.Configure(x =>
	{
		x.AddRegistry<FooRegistry>();
		x.AddRegistry(new BarRegistry());
	});

Working with registries
-----------------------
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
	
Custom conventions
------------------
At the moment, built in conventions includes AddAllConvention, FirstInterfaceConvention, NamingConvention, SetAllPropertiesConvention and ScanForRegistriesConvention. If these doesn�t suit you, creating custom conventions is as easy as creating a class that implements the IAssemblyScanner interface.
	public class CustomConvention : IAssemblyScannerConvention
	{
		void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
		{
			if (type == typeof(FooService))
				registry.Register<IFooService, FooService>().WithName("Custom");
		}
	}
	
Release Notes
-------------

### vNext
* Added non-generic overload to the `AddAllConvention`
* Added option to scan for internal types in an assembly - `IAssemblyScanner.InternalTypes()`
* Strong named the assembly

### v1.3
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

License
-------
The MIT License

Copyright (c) 2011 Thomas Pedersen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.