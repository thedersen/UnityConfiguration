using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class RegistrationExpression : Expression, ILifetimePolicyExpression
    {
        private readonly Type typeFrom;
        private readonly Type typeTo;
        private string name;
        private LifetimeManager lifetimeManager;
        private InjectionMember[] injectionMembers;

        public RegistrationExpression(Type typeFrom, Type typeTo)
        {
            this.typeFrom = typeFrom;
            this.typeTo = typeTo;
            lifetimeManager = new TransientLifetimeManager();
            injectionMembers = new InjectionMember[0];
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

        internal void WithInjectionMembers(params InjectionMember[] injectionMember)
        {
            injectionMembers = injectionMember;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterType(typeFrom, typeTo, name, lifetimeManager, injectionMembers);
        }
    }
}