using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public static class UnityExtension
    {
        /// <summary>
        /// Configure an instance of an <see cref="IUnityContainer"/>.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="expression">An expression used for configuring the container.</param>
        public static IUnityContainer Configure(this IUnityContainer container, Action<IUnityRegistry> expression)
        {
            var registry = new UnityRegistry();

            expression(registry);

            registry.Configure(container);

            return container;
        }
    }
}