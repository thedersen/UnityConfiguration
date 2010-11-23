using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class PropertyInjectionTests
    {
        [Test]
        public void Can_configure_property_injection_using_convention()
        {
            var container = new UnityContainer();
            container.Initialize(x => x.Scan(scan =>
                                                 {
                                                     scan.AssemblyContaining<FooRegistry>();
                                                     scan.With<FirstInterfaceConvention>();
                                                     scan.With<SetAllPropertiesConvention>().OfType<ILogger>();
                                                 }));

            Assert.That(container.Resolve<FooService>().Logger, Is.Not.Null);
        }

        [Test]
        public void Can_set_property_after_building_up()
        {
            var container = new UnityContainer();
            container.Initialize(x =>
                                     {
                                         x.Register<ILogger, NullLogger>();
                                         x.AfterBuildingUp<FooService>().Call((c, s) => s.Logger = c.Resolve<ILogger>());
                                     });

            Assert.That(container.Resolve<FooService>().Logger, Is.Not.Null);
        }
    }
}