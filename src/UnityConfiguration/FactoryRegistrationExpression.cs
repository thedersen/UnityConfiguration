using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class FactoryRegistrationExpression<TFrom> : Expression
    {
        private readonly Func<IUnityContainer, TFrom> factoryDelegate;
        private string name;

        public FactoryRegistrationExpression(Func<IUnityContainer, TFrom> factoryDelegate)
        {
            this.factoryDelegate = factoryDelegate;
        }

        /// <summary>
        /// Specify a name for this registration.
        /// </summary>
        /// <param name="name">The name for this registration.</param>
        public void WithName(string name)
        {
            this.name = name;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterType<TFrom>(name, new InjectionFactory(c => (object) factoryDelegate(c)));
        }
    }
}