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
        {
            this.typeFrom = typeFrom;
            this.typeTo = typeTo;
            lifetimeManagerFunc = () => new TransientLifetimeManager();
            injectionMembers = new InjectionMember[0];
        }

        /// <summary>
        /// Specify how lifetime should be managed by the controller, by specifying a <see cref="LifetimeManager"/>
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="LifetimeManager"/> to use.</typeparam>
        public void Using<T>() where T : LifetimeManager, new()
        {
            lifetimeManagerFunc = () => new T();
        }

        /// <summary>
        /// Specify a name for this registration mapping.
        /// </summary>
        /// <param name="name">The name for this registration mapping.</param>
        public RegistrationExpression WithName(string name)
        {
            this.name = name;
            return this;
        }

        /// <summary>
        /// Specify parameters that will be passed to the constructor when constructing the type.
        /// If some of the parameters should be resolved from the container, specify its type.
        /// </summary>
        /// <param name="args">Value or type of the parameters.</param>
        /// <example>
        /// WithConstructorArguments(42, "some string", typeof(IBar));
        /// </example>
        public ILifetimePolicyExpression WithConstructorArguments(params object[] args)
        {
            WithInjectionMembers(new InjectionConstructor(args));
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