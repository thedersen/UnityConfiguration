using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class RegistrationExpression : Expression, INamedRegistrationExpression
    {
        protected readonly Type typeFrom;
        private Type typeTo;
        private InjectionMember[] injectionMembers;
        protected Func<LifetimeManager> lifetimeManagerFunc;
        protected string name;
        
        public RegistrationExpression(Type typeFrom, Type typeTo)
        {
            this.typeFrom = typeFrom;
            this.typeTo = typeTo;
            lifetimeManagerFunc = () => new TransientLifetimeManager();
            injectionMembers = new InjectionMember[0];
        }

        public IRegistrationExpression WithName(string name)
        {
            this.name = name;
            return this;
        }

        public ILifetimePolicyExpression WithConstructorArguments(params object[] args)
        {
            WithInjectionMembers(new InjectionConstructor(args));
            return this;
        }

        public void Using<T>() where T : LifetimeManager, new()
        {
            lifetimeManagerFunc = () => new T();
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