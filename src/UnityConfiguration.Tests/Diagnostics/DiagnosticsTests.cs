using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration.Diagnostics
{
    [TestFixture]
    public class DiagnosticsTests
    {
        [Test]
        public void Prints_all_configured_types()
        {
            IUnityContainer container = new UnityContainer()
                .RegisterType<IFooService, FooService>()
                .RegisterType<IBarService, BarService>(new ContainerControlledLifetimeManager())
                .RegisterType<IBarService, BarService>("Bar", new TransientLifetimeManager())
                .RegisterType<IFooService, FooService>("Foo", new ContainerControlledLifetimeManager());

            string report = container.WhatDoIHave();

            string expexted =
                "Microsoft.Practices.Unity.IUnityContainer - Microsoft.Practices.Unity.IUnityContainer with ContainerLifetimeManager\r\n" +
                "UnityConfiguration.Services.IBarService - UnityConfiguration.Services.BarService named \"Bar\" with TransientLifetimeManager\r\n" +
                "UnityConfiguration.Services.IBarService - UnityConfiguration.Services.BarService with ContainerControlledLifetimeManager\r\n" +
                "UnityConfiguration.Services.IFooService - UnityConfiguration.Services.FooService\r\n" +
                "UnityConfiguration.Services.IFooService - UnityConfiguration.Services.FooService named \"Foo\" with ContainerControlledLifetimeManager\r\n";

            Assert.That(report, Is.EqualTo(expexted));
        }
    }
}