using System;
using Unity;
using Unity.Extension;

namespace UnityConfiguration
{
    public class ConfigureExtensionExpression<T> : Expression where T : IUnityContainerExtensionConfigurator
    {
        private readonly Action<T> configAction;

        public ConfigureExtensionExpression(Action<T> configAction)
        {
            this.configAction = configAction;
        }

        internal override void Execute(IUnityContainer container)
        {
            configAction(container.Configure<T>());
        }
    }
}