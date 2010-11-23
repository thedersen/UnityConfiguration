using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class PostBuildUpActionTests
    {
        [Test]
        public void Can_call_method_on_concrete_after_build_up()
        {
            var container = new UnityContainer();
            container.Configure(x => x.AfterBuildingUp<StartableService1>().Call((c, s) => s.Start()));

            Assert.That(container.Resolve<StartableService1>().StartWasCalled);
        }

        [Test]
        public void Can_call_method_on_interface_after_build_up()
        {
            var container = new UnityContainer();
            container.Configure(x =>
            {
                x.Register<IStartable, StartableService1>();
                x.AfterBuildingUp<IStartable>().Call((c, s) => s.Start());
            });

            Assert.That(container.Resolve<IStartable>().StartWasCalled);
        }

        [Test]
        public void Can_call_method_on_interface_after_build_up_2()
        {
            var container = new UnityContainer();
            container.Configure(x =>
            {
                x.Register<IStartable, StartableService1>().WithName("1");
                x.Register<IStartable, StartableService2>().WithName("2");
                x.AfterBuildingUp<IStartable>().Call((c, s) => s.Start());
            });

            Assert.That(container.Resolve<IStartable>("1").StartWasCalled);
            Assert.That(container.Resolve<IStartable>("2").StartWasCalled);
        }

        [Test]
        public void Can_call_method_on_several_interfaces_after_build_up()
        {
            var container = new UnityContainer();
            container.Configure(x =>
            {
                x.Register<IStartable, StartableService1>();
                x.Register<IStoppable, StoppableService>();
                x.AfterBuildingUp<IStartable>().Call((c, s) => s.Start());
                x.AfterBuildingUp<IStoppable>().Call((c, s) => s.Stop());
            });

            Assert.That(container.Resolve<IStartable>().StartWasCalled);
            Assert.That(container.Resolve<IStoppable>().StopWasCalled);
        }

        [Test]
        public void Can_decorate_concrete_service_after_build_up()
        {
            var container = new UnityContainer();
            container.Configure(x =>
            {
                x.Register<IFooService, FooService>();
                x.AfterBuildingUp<FooService>().DecorateWith((c, t) => new FooDecorator(t));
            });

            var fooService = container.Resolve<IFooService>();
            Assert.That(fooService, Is.InstanceOf<FooDecorator>());
            Assert.That(fooService.As<FooDecorator>().InnerService, Is.InstanceOf<FooService>());
        }

        [Test]
        public void Can_decorate_interface_after_build_up()
        {
            var container = new UnityContainer();
            container.Configure(x =>
            {
                x.Register<IFooService, FooService>();
                x.AfterBuildingUp<IFooService>().DecorateWith((c, t) => new FooDecorator(t));
            });

            var fooService = container.Resolve<IFooService>();
            Assert.That(fooService, Is.InstanceOf<FooDecorator>());
            Assert.That(fooService.As<FooDecorator>().InnerService, Is.InstanceOf<FooService>());
        }
    }
}