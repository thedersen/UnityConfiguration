using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class FactoryRegistrationExpression<TFrom> : Expression
    {
        private readonly Func<IUnityContainer, object> factoryDelegate;
        private string name;

        public FactoryRegistrationExpression(Func<IUnityContainer, object> factoryDelegate)
        {
            this.factoryDelegate = factoryDelegate;
        }

        public void WithName(string name)
        {
            this.name = name;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterType<TFrom>(name, new InjectionFactory(factoryDelegate));
        }
    }
}