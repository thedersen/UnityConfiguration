using System;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class TypeRegistrationTests
    {
        [Test]
        public void Can_register_a_func_with_parameters()
        {
            var container = new UnityContainer();

            var myService = new BarService();
            container.Initialize(x => x.Register<Func<int, IBarService>>(c => i => myService));

            Assert.That(container.Resolve<Func<int, IBarService>>()(1), Is.SameAs(myService));
        }

        [Test]
        public void Can_register_named_instance()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.Register<IBarService, BarService>().WithName("name"));

            Assert.That(container.Resolve<IBarService>("name"), Is.InstanceOf<BarService>());
        }

        [Test]
        public void Can_register_named_instance_using_factory_delegate()
        {
            var container = new UnityContainer();

            var myService = new BarService();
            container.Initialize(x => x.Register<IBarService>(c => myService).WithName("name"));

            Assert.That(container.Resolve<IBarService>("name"), Is.SameAs(myService));
        }

        [Test]
        public void Can_register_named_singleton_instance()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.Register<IBarService, BarService>().WithName("name").AsSingleton());

            Assert.That(container.Resolve<IBarService>("name"), Is.SameAs(container.Resolve<IBarService>("name")));
        }

        [Test]
        public void Can_register_singleton()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Register<IBarService, BarService>().AsSingleton());

            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
            Assert.That(container.Resolve<IBarService>(), Is.SameAs(container.Resolve<IBarService>()));
        }

        [Test]
        public void Can_register_transient_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Register<IBarService, BarService>());

            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
            Assert.That(container.Resolve<IBarService>(), Is.Not.SameAs(container.Resolve<IBarService>()));
        }

        [Test]
        public void Can_register_using_factory_delegate()
        {
            var container = new UnityContainer();

            var myService = new BarService();
            container.Initialize(x => x.Register<IBarService>(c => myService));

            Assert.That(container.Resolve<IBarService>(), Is.SameAs(myService));
        }
    }
}