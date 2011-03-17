using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public static class UnityExtension
    {
        public static void Configure(this IUnityContainer container, Action<IUnityRegistry> expression)
        {
            var registry = new UnityRegistry();

            expression(registry);

            registry.Configure(container);
        }
    }
}