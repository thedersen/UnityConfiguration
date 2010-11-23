using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class LifetimeExpression : Expression
    {
        private readonly Func<LifetimeManager> lifetimeManager;
        private readonly string namedInstance;
        private readonly Type type;

        public LifetimeExpression(Type type, Func<LifetimeManager> lifetimeManager, string namedInstance)
        {
            this.type = type;
            this.lifetimeManager = lifetimeManager;
            this.namedInstance = namedInstance;
        }

        internal override void Execute(IUnityContainer container)
        {
            if (type.IsConcrete())
            {
                container.RegisterType(type, namedInstance, lifetimeManager());
            }
            else
            {
                container.Registrations.ForEach(c =>
                                                    {
                                                        if (c.RegisteredType == type)
                                                            container.RegisterType(c.MappedToType, c.Name,
                                                                                   lifetimeManager());
                                                    });
            }
        }
    }
}