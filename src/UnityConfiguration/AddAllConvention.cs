using System;

namespace UnityConfiguration
{
    public class AddAllConvention : RegistrationConvention, ILifetimePolicyExpression
    {
        private Type interfaceType;
        private bool asSingleton;

        public ILifetimePolicyExpression TypesImplementing<T>()
        {
            interfaceType = typeof(T);
            return this;
        }

        public void AsSingleton()
        {
            asSingleton = true;
        }

        internal override void Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(interfaceType) && type.CanBeCreated())
            {
                if (asSingleton)
                {
                    registry.Register(interfaceType, type).WithName(type.Name).AsSingleton();
                }
                else
                {
                    registry.Register(interfaceType, type).WithName(type.Name);
                }

            }
        }
    }
}