using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class ConstructorConfigurationTests
    {
        [Test]
        public void Can_configure_ctor_arguments_for_type()
        {
            var container = new UnityContainer();

            container.Configure(x =>
                                     {
                                         x.Register<IServiceWithCtorArgs, ServiceWithCtorArgs>();
                                         x.Register<IFooService, FooService>();
                                         x.Configure<ServiceWithCtorArgs>().UseArguments("some string", typeof (IFooService));
                                     });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
            Assert.That(serviceWithCtorArgs.SomeString, Is.EqualTo("some string"));
            Assert.That(serviceWithCtorArgs.FooService, Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_select_constructor_to_use()
        {
            var container = new UnityContainer();

            container.Configure(x =>
                                     {
                                         x.Register<IServiceWithCtorArgs, ServiceWithCtorArgs>();
                                         x.Configure<ServiceWithCtorArgs>().UseConstructor();
                                     });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
            Assert.That(serviceWithCtorArgs.SomeString, Is.Null);
            Assert.That(serviceWithCtorArgs.FooService, Is.Null);
        }

        [Test]
        public void Can_select_constructor_to_use_on_named_registration()
        {
            var container = new UnityContainer();

            container.Configure(x =>
                                     {
                                         x.Register<IServiceWithCtorArgs, ServiceWithCtorArgs>().WithName("name");
                                         x.Configure<ServiceWithCtorArgs>().WithName("name").UseConstructor();
                                     });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>("name");
            Assert.That(serviceWithCtorArgs.SomeString, Is.Null);
            Assert.That(serviceWithCtorArgs.FooService, Is.Null);
        }

        [Test]
        public void Can_select_constructor_to_use_2()
        {
            var container = new UnityContainer();

            container.Configure(x =>
                                     {
                                         x.Register<IServiceWithCtorArgs, ServiceWithCtorArgs>();
                                         x.Register<IFooService, FooService>();
                                         x.Configure<ServiceWithCtorArgs>().UseConstructor(typeof (IFooService));
                                     });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
            Assert.That(serviceWithCtorArgs.SomeString, Is.Null);
            Assert.That(serviceWithCtorArgs.FooService, Is.InstanceOf<FooService>());
        }
    }
}