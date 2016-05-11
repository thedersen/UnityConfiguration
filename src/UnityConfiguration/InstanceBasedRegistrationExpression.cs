using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class InstanceBasedRegistrationExpression : RegistrationExpression
    {
        private object instanceTo;

        public InstanceBasedRegistrationExpression(Type typeFrom, object instanceTo)
            :base (typeFrom,null)
        {
            lifetimeManagerFunc = () => new ContainerControlledLifetimeManager();
            this.instanceTo = instanceTo;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterInstance(typeFrom, name, instanceTo,  lifetimeManagerFunc());
        }
    }
}