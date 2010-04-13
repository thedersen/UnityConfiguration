using System;

namespace UnityConfiguration
{
    public class AddAllConvention : RegistrationConvention, ILifetimePolicyExpression
    {
        private Type interfaceType;
        private bool asSingleton;
        private Func<Type, string> getName = t => t.Name;

        public AddAllConvention TypesImplementing<T>()
        {
            interfaceType = typeof(T);
            return this;
        }

        public AddAllConvention WithName(Func<Type, string> func)
        {
            getName = func;
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
                    registry.Register(interfaceType, type).WithName(getName(type)).AsSingleton();
                }
                else
                {
                    registry.Register(interfaceType, type).WithName(getName(type));
                }

            }
        }
    }
}