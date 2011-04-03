using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class RegistryTests
    {
        [Test]
        public void Can_initalize_container_with_one_registry()
        {
            var container = new UnityContainer();

            container.Configure(x => x.AddRegistry<FooRegistry>());

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_initalize_container_with_two_registries()
        {
            var container = new UnityContainer();

            container.Configure(x =>
            {
                x.AddRegistry<FooRegistry>();
                x.AddRegistry(new BarRegistry());
            });

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
        }

        [Test]
        public void Can_add_same_registry_twice_without_breaking_anything()
        {
            var container = new UnityContainer();

            container.Configure(x =>
            {
                x.AddRegistry<FooRegistry>();
                x.AddRegistry<FooRegistry>();
            });

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
        }
    }
}