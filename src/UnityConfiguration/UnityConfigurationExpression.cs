using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    internal class UnityConfigurationExpression : UnityRegistry, IUnityConfigurationExpression
    {
        private readonly List<UnityRegistry> registries = new List<UnityRegistry>();

        public void AddRegistry<T>() where T : UnityRegistry, new()
        {
            AddRegistry(new T());
        }

        public void AddRegistry(UnityRegistry registry)
        {
            if(!registries.Contains(registry))
                registries.Add(registry);
        }

        public override void Configure(IUnityContainer container)
        {
            base.Configure(container);

            registries.ForEach(registry => registry.Configure(container));
        }
    }
}