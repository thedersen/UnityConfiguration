using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;
using UnityConfiguration.Services.OtherNamespace;

namespace UnityConfiguration
{
    [TestFixture]
    public class FilterTests
    {
        [Test]
        public void Can_exclude_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.ExcludeType<BarService>();
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IBarService>());
        }

        [Test]
        public void Can_exclude_type_using_delegate()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.Exclude(t => t == typeof(BarService));
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IBarService>());
        }

        [Test]
        public void Can_exclude_namespace_containing_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.ExcludeNamespaceContaining<ServiceInOtherNamespace>();
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IServiceInOtherNamespace>());
        }

        [Test]
        public void Can_include_namespace_containing_type()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.IncludeNamespaceContaining<ServiceInOtherNamespace>();
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IFooService>());
            Assert.That(container.Resolve<IServiceInOtherNamespace>(), Is.InstanceOf<ServiceInOtherNamespace>());
        }

        [Test]
        public void Can_include_namespace()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.IncludeNamespace("UnityConfiguration.Services.OtherNamespace");
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IFooService>());
            Assert.That(container.Resolve<IServiceInOtherNamespace>(), Is.InstanceOf<ServiceInOtherNamespace>());
        }

        [Test]
        public void Can_include_using_delegate()
        {
            var container = new UnityContainer();

            container.Initialize(x => x.Scan(scan =>
            {
                scan.AssemblyContaining<FooRegistry>();
                scan.With<FirstInterfaceConvention>();
                scan.Include(t => t == typeof(ServiceInOtherNamespace));
            }));

            Assert.Throws<ResolutionFailedException>(() => container.Resolve<IFooService>());
            Assert.That(container.Resolve<IServiceInOtherNamespace>(), Is.InstanceOf<ServiceInOtherNamespace>());
        }
    }
}