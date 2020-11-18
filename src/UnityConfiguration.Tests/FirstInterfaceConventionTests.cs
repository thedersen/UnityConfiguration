using System.ComponentModel;
using NUnit.Framework;
using Unity;
using UnityConfiguration.Services;
using UnityConfiguration.UserInterface;

namespace UnityConfiguration
{
    [TestFixture]
    public class FirstInterfaceConventionTests
    {
        [Test]
        public void Can_ignore_interfaces_on_base_classes()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.WithFirstInterfaceConvention().IgnoreInterfacesOnBaseTypes();
            }));

            Assert.That(container.Resolve<IMyView>(), Is.InstanceOf<MyView>());
        }

        [Test]
        public void Includes_interfaces_on_base_classes_in_search_for_first()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.WithFirstInterfaceConvention();
            }));

            Assert.That(container.Resolve<IComponent>(), Is.InstanceOf<MyView>());
        }

        [Test]
        public void Registers_all_types_with_the_first_interface_implemented_on_the_type()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.WithFirstInterfaceConvention();
            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
        }
    }
}