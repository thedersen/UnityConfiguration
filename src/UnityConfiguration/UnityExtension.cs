using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public static class UnityExtension
    {
        public static void Initialize(this IUnityContainer container, Action<IInitializationExpression> expression)
        {
            var initializationExpression = new InitializationExpression();

            expression(initializationExpression);

            initializationExpression.Initialize(container);
        }
    }
}