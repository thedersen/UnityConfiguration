using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class LifetimeManagementTests
    {
        [Test]
        public void Can_configure_concrete_types_as_singleton()
        {
            var container = new UnityContainer();

            container.Configure(x =>
            {
                x.Register<IBarService, BarService>();
                x.MakeSingleton<BarService>();
            });

            Assert.That(container.Resolve<IBarService>(), Is.SameAs(container.Resolve<IBarService>()));
        }

        [Test]
        public void Can_configure_interfaces_as_singleton()
        {
            var container = new UnityContainer();

            container.Configure(x =>
            {
                x.Register<IBarService, BarService>();
                x.MakeSingleton<IBarService>();
            });

            Assert.That(container.Resolve<IBarService>(), Is.SameAs(container.Resolve<IBarService>()));
        }


        [Test]
        public void Can_make_transient_sevice_a_singleton_in_child_container()
        {
            var container = new UnityContainer();
            container.Configure(x => x.Register<IFooService, FooService>());

            IUnityContainer childContainer = container.CreateChildContainer();
            childContainer.Configure(x => x.MakeSingleton<FooService>());

            Assert.That(container.Resolve<IFooService>(), Is.Not.SameAs(container.Resolve<IFooService>()));
            Assert.That(container.Resolve<IFooService>(), Is.Not.SameAs(childContainer.Resolve<IFooService>()));
            Assert.That(childContainer.Resolve<IFooService>(), Is.SameAs(childContainer.Resolve<IFooService>()));
        }

        [Test]
        public void Configure_interface_as_singletons_makes_all_registrations_a_singleton()
        {
            var container = new UnityContainer();

            container.Configure(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssemblyContaining<FooRegistry>();
                    scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
                });
                x.MakeSingleton<IHaveManyImplementations>();
            });

            Assert.That(container.Resolve<IHaveManyImplementations>("Implementation1"), Is.SameAs(container.Resolve<IHaveManyImplementations>("Implementation1")));
            Assert.That(container.Resolve<IHaveManyImplementations>("Implementation2"), Is.SameAs(container.Resolve<IHaveManyImplementations>("Implementation2")));
        }
    }
}