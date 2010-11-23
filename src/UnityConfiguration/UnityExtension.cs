using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public static class UnityExtension
    {
        public static void Configure(this IUnityContainer container, Action<IUnityConfigurationExpression> expression)
        {
            var configurationExpression = new UnityConfigurationExpression();

            expression(configurationExpression);

            configurationExpression.Configure(container);
        }
    }
}