using System.Linq;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class AddAllConventionTests
    {
        [Test]
        public void Adds_all_types_implementing_the_specified_type_with_default_name()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
            }));

            Assert.That(container.ResolveAll<IHaveManyImplementations>().Count(), Is.EqualTo(2));
            Assert.That(container.ResolveAll<IHaveManyImplementations>().First(), Is.Not.SameAs(container.ResolveAll<IHaveManyImplementations>().First()));
        }

        [Test]
        public void Can_add_all_as_singletons()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>().AsSingleton();
            }));

            Assert.That(container.ResolveAll<IHaveManyImplementations>().First(), Is.SameAs(container.ResolveAll<IHaveManyImplementations>().First()));
        }

        [Test]
        public void Can_add_all_as_singletons_and_override_the_default_naming_convention()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>().WithName(t => "test").AsSingleton();
            }));

            Assert.That(container.Resolve<IHaveManyImplementations>("test"), Is.SameAs(container.Resolve<IHaveManyImplementations>("test")));
        }

        [Test]
        public void Can_override_the_default_naming_convention()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>().WithName(t => "test");
            }));

            Assert.That(container.Resolve<IHaveManyImplementations>("test"), Is.Not.Null);
        }

        [Test]
        public void Can_add_all_implementations_of_an_open_generic()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<AddAllConvention>().TypesImplementing(typeof(IHandler<>));
            }));

            Assert.That(container.ResolveAll<IHandler<Message>>().Count(), Is.EqualTo(2));
        }
    }
}