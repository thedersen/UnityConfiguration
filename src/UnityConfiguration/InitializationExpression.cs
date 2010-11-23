using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    internal class InitializationExpression : UnityRegistry, IInitializationExpression
    {
        private readonly List<UnityRegistry> registries = new List<UnityRegistry>();

        #region IInitializationExpression Members

        public void AddRegistry<T>() where T : UnityRegistry, new()
        {
            registries.Add(new T());
        }

        #endregion

        internal void Initialize(IUnityContainer container)
        {
            registries.Add(this);
            registries.ForEach(registry => registry.Configure(container));
        }
    }
}