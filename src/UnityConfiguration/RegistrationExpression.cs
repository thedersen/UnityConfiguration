using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class RegistrationExpression : Expression, ILifetimePolicyExpression
    {
        private readonly Type typeFrom;
        private readonly Type typeTo;
        private InjectionMember[] injectionMembers;
        private LifetimeManager lifetimeManager;
        private string name;

        public RegistrationExpression(Type typeFrom, Type typeTo)
        {
            this.typeFrom = typeFrom;
            this.typeTo = typeTo;
            lifetimeManager = new TransientLifetimeManager();
            injectionMembers = new InjectionMember[0];
        }

        /// <summary>
        /// Register as singleton.
        /// </summary>
        public void AsSingleton()
        {
            lifetimeManager = new ContainerControlledLifetimeManager();
        }

        /// <summary>
        /// Specify a name for this registration mapping.
        /// </summary>
        /// <param name="name">The name for this registration mapping.</param>
        public ILifetimePolicyExpression WithName(string name)
        {
            this.name = name;
            return this;
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