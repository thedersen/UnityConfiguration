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
            var container = new UnityContainer()
                .RegisterType<IFooService, FooService>()
                .RegisterType<IBarService, BarService>(new ContainerControlledLifetimeManager())
                .RegisterType<IBarService, BarService>("Bar")
                .RegisterType<IFooService, FooService>("Foo", new ContainerControlledLifetimeManager());
            
            string report = container.WhatDoIHave();

            string expexted = "Microsoft.Practices.Unity.IUnityContainer - Microsoft.Practices.Unity.IUnityContainer as Singleton\r\n" +
                              "UnityConfiguration.Services.IBarService - UnityConfiguration.Services.BarService as Singleton\r\n" +
                              "UnityConfiguration.Services.IBarService - UnityConfiguration.Services.BarService named \"Bar\"\r\n" +
                              "UnityConfiguration.Services.IFooService - UnityConfiguration.Services.FooService\r\n" +
                              "UnityConfiguration.Services.IFooService - UnityConfiguration.Services.FooService named \"Foo\" as Singleton\r\n";

            Assert.That(report, Is.EqualTo(expexted));
        }
    }
}