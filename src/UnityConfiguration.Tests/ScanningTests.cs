using System.Linq;
using NUnit.Framework;
using Unity;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class ScanningTests
    {
        [Test]
        public void Can_scan_using_several_conventions()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
            Assert.That(container.ResolveAll<IHaveManyImplementations>().Count(), Is.EqualTo(2));
        }

        [Test]
        public void Can_scan_assembly()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.Assembly(typeof(FooRegistry).Assembly);
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_scan_named_assembly()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.Assembly("UnityConfiguration.Tests");
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_scan_named_assembly_with_extension()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.Assembly("UnityConfiguration.Tests.dll");
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_scan_folder()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.AssembliesInDirectory(TestContext.CurrentContext.TestDirectory);
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            var registration = container.Registrations.FirstOrDefault(x => x.RegisteredType == typeof(IFooService));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_scan_folder_and_exclude_assemblies_by_using_a_predicate()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.AssembliesInDirectory(TestContext.CurrentContext.TestDirectory, a => a.GetName().Name != "UnityConfiguration.Tests");
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            Assert.That(container.IsRegistered<IFooService>(), Is.False);
        }

        [Test]
        public void Can_scan_base_folder()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.AssembliesInBaseDirectory();
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_scan_base_folder_and_exclude_assemblies_by_using_a_predicate()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.AssembliesInBaseDirectory(a => a.GetName().Name != "UnityConfiguration.Tests");
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            Assert.That(container.IsRegistered<IFooService>(), Is.False);
        }

        [Test]
        public void Can_scan_for_registries()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.AssembliesInBaseDirectory();
                                                scan.ForRegistries();
                                            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_scan_internal_types_when_specified()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssembliesInBaseDirectory();
                scan.InternalTypes();
                scan.With<FirstInterfaceConvention>();
            }));

            Assert.That(container.Resolve<IInternalService>(), Is.InstanceOf<InternalService>());
        }
    }
}