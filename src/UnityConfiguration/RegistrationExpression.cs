using System;
using Unity;
using Unity.Injection;
using Unity.Lifetime;

namespace UnityConfiguration
{
    public class RegistrationExpression : Expression, INamedRegistrationExpression
    {
        private readonly Type typeFrom;
        private Type typeTo;
        private InjectionMember[] injectionMembers;
        private Func<ITypeLifetimeManager> typeLifetimeManagerFunc;
        private string name;
        
        public RegistrationExpression(Type typeFrom, Type typeTo)
        {
            this.typeFrom = typeFrom;
            this.typeTo = typeTo;
            typeLifetimeManagerFunc = () => new TransientLifetimeManager();
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
            typeLifetimeManagerFunc = () => (ITypeLifetimeManager) new T();
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
                        container.RegisterType(c.MappedToType, c.Name, typeLifetimeManagerFunc(), injectionMembers);

                });
            }
            else if (container.IsRegistered(typeFrom, name) == false)
            {
                container.RegisterType(typeFrom, typeTo, name, typeLifetimeManagerFunc(), injectionMembers);
            }
        }
    }
}
