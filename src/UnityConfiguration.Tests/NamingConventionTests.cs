using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace UnityConfiguration
{
    [TestFixture]
    public class NamingConventionTests
    {
        [Test]
        public void Registers_with_default_naming_convention()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<NamedService>();
                scan.With<NamingConvention>();
            }));

            Assert.That(container.Resolve<INamedService>(), Is.InstanceOf<NamedService>());
        }
        
        [Test]
        public void Can_override_the_default_naming_convention()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<NamedService>();
                scan.ExcludeType<NamedService>();

                scan.With<NamingConvention>().WithInterfaceName(t => "I" + t.Name + "Service");
            }));

            Assert.That(container.Resolve<INamedService>(), Is.InstanceOf<Named>());
        }
        
        [Test]
        public void Does_not_register_when_interface_is_not_found()
        {
            var container = new UnityContainer();

            container.Configure(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<NamedService>();

                scan.With<NamingConvention>().WithInterfaceName(t => "IDoNotExist");
            }));

            Assert.That(container.IsRegistered<INamedService>(), Is.False);
        }
    }

    public class NamedService : IInterface, INamedService
    {
    }

    public class Named : INamedService
    {
    }

    public interface INamedService
    {
    }

    public interface IInterface{}
}