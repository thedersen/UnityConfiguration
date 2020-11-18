using NUnit.Framework;
using Unity;
using Unity.Lifetime;
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
                .RegisterType<IBarService, BarService>("Bar", new TransientLifetimeManager())
                .RegisterType<IFooService, FooService>("Foo", new ContainerControlledLifetimeManager());

            var report = container.WhatDoIHave();

            var expexted = new string[]
            {
                "Unity.IUnityContainer - Unity.UnityContainer with ContainerLifetimeManager",
                "UnityConfiguration.Services.IBarService - UnityConfiguration.Services.BarService named \"Bar\" with TransientLifetimeManager",
                "UnityConfiguration.Services.IBarService - UnityConfiguration.Services.BarService with ContainerControlledLifetimeManager",
                "UnityConfiguration.Services.IFooService - UnityConfiguration.Services.FooService with TransientLifetimeManager",
                "UnityConfiguration.Services.IFooService - UnityConfiguration.Services.FooService named \"Foo\" with ContainerControlledLifetimeManager",
            };

            foreach (var s in expexted)
            {
                Assert.IsTrue(report.Contains(s));
            }

        }
    }
}