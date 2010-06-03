using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class RegistrationExpression : Expression, ILifetimePolicyExpression, INamedRegistrationExpression
    {
        private readonly Type typeFrom;
        private readonly Type typeTo;
        private string name;
        private LifetimeManager lifetimeManager;

        public RegistrationExpression(Type typeFrom, Type typeTo)
        {
            this.typeFrom = typeFrom;
            this.typeTo = typeTo;
            lifetimeManager = new TransientLifetimeManager();
        }

        public ILifetimePolicyExpression WithName(string name)
        {
            this.name = name;
            return this;
        }

        public void AsSingleton()
        {
            lifetimeManager = new ContainerControlledLifetimeManager();
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterType(typeFrom, typeTo, name, lifetimeManager);
        }
    }

    public interface INamedRegistrationExpression
    {

    }
}