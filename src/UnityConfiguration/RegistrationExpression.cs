using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class RegistrationExpression : Expression, ILifetimePolicyExpression
    {
        private readonly Type typeFrom;
        private Type typeTo;
        private InjectionMember[] injectionMembers;
        private Func<LifetimeManager> lifetimeManagerFunc;
        private string name;

        public RegistrationExpression(Type typeFrom, Type typeTo)
            : this(typeFrom, typeTo, null, () => new TransientLifetimeManager())
        {
        }

        internal RegistrationExpression(Type typeFrom, Type typeTo, string name, Func<LifetimeManager> lifetimeManager)
        {
            this.typeFrom = typeFrom;
            this.typeTo = typeTo;
            this.name = name;
            lifetimeManagerFunc = lifetimeManager;
            injectionMembers = new InjectionMember[0];
        }

        /// <summary>
        /// Indicates that only a single instance should be created, and then
        /// should be re-used for all subsequent requests.
        /// </summary>
        public void AsSingleton()
        {
            lifetimeManagerFunc = () => new ContainerControlledLifetimeManager();
        }

        /// <summary>
        /// Indicates that instances should not be re-used, nor have
        /// their lifecycle managed by Unity.
        /// </summary>
        public void AsTransient()
        {
            lifetimeManagerFunc = () => new TransientLifetimeManager();
        }

        /// <summary>
        /// Indicates that instances should be re-used within the same thread.
        /// </summary>
        public void AsPerThread()
        {
            lifetimeManagerFunc = () => new PerThreadLifetimeManager();
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
            if (typeFrom == null && !typeTo.IsConcrete())
            {
                container.Registrations.ForEach(c =>
                {
                    if (c.RegisteredType == typeTo)
                        container.RegisterType(c.MappedToType, c.Name, lifetimeManagerFunc(), injectionMembers);

                });
            }
            else
            {
                container.RegisterType(typeFrom, typeTo, name, lifetimeManagerFunc(), injectionMembers);
            }
        }
    }
}