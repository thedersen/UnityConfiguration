using System;
using NUnit.Framework;
using Unity;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class CustomConventionTests
    {
        [Test]
        public void Can_create_custom_conventions()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<CustomConvention>();
            }));

            Assert.That(container.Resolve<IFooService>("Custom"), Is.InstanceOf<FooService>());
        }
    }

    public class CustomConvention : IAssemblyScannerConvention
    {
        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            if (type == typeof(FooService))
                registry.Register<IFooService, FooService>().WithName("Custom");
        }
    }
}