using Microsoft.Practices.Unity;
using NUnit.Framework;
using UnityConfiguration.Services;

namespace UnityConfiguration.Diagnostics
{
    [TestFixture]
    public class DiagnosticsTester
    {
        [Test]
        public void Prints_all_configured_types()
        {
            var container = new UnityContainer();
            container.AddNewExtension<DiagnosticsExtension>();
            container.RegisterType<IFooService, FooService>();
            container.RegisterType<IBarService, BarService>(new ContainerControlledLifetimeManager());
            
            string report = container.WhatDoIHave();

            string expexted = @"IFooService - FooService";

            Assert.That(report, Is.EqualTo(expexted));
        }
    }
}