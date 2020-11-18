using NUnit.Framework;
using Unity;
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
                                         x.Configure<ServiceWithCtorArgs>().WithConstructorArguments("some string", typeof (IFooService));
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
                                         x.SelectConstructor<ServiceWithCtorArgs>();
                                     });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
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
                                         x.SelectConstructor<ServiceWithCtorArgs>(typeof (IFooService));
                                     });

            var serviceWithCtorArgs = container.Resolve<IServiceWithCtorArgs>();
            Assert.That(serviceWithCtorArgs.SomeString, Is.Null);
            Assert.That(serviceWithCtorArgs.FooService, Is.InstanceOf<FooService>());
        }
    }
}