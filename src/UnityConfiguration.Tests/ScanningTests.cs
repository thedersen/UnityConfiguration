using System.Linq;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration
{
    [TestFixture]
    public class ScanningTests
    {
        [Test]
        public void Can_scan_using_several_conventions()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.With<AddAllConvention>().TypesImplementing<IHaveManyImplementations>();
            }));

            Assert.That(container.Resolve<IFooService>(), Is.InstanceOf<FooService>());
            Assert.That(container.Resolve<IBarService>(), Is.InstanceOf<BarService>());
            Assert.That(container.ResolveAll<IHaveManyImplementations>().Count(), Is.EqualTo(2));
        }
    }
}