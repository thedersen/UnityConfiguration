using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class ScanForRegistriesConventionTests
    {
        [Test]
        public void Finds_all_registries()
        {
            var container = new UnityContainer();

            container.Configure(x => x.AddRegistry<MyRegistry>());

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
        }

        [Test]
        public void Finds_all_registries_2()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
                                            {
                                                scan.AssemblyContaining<FooRegistry>();
                                                scan.ForRegistries();
                                                scan.With<FirstInterfaceConvention>();
                                            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
        }
    }

    public class MyRegistry : UnityRegistry
    {
        public MyRegistry()
        {
            Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.ForRegistries();
                scan.With<FirstInterfaceConvention>();
            });
        }
    }
}