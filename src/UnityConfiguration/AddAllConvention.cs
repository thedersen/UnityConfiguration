using System;

namespace UnityConfiguration
{
    public class AddAllConvention : RegistrationConvention, ILifetimePolicyExpression
    {
        private bool asSingleton;
        private Func<Type, string> getName = t => t.Name;
        private Type interfaceType;

        public void AsSingleton()
        {
            asSingleton = true;
        }

        public AddAllConvention TypesImplementing<T>()
        {
            interfaceType = typeof (T);
            return this;
        }

        public AddAllConvention WithName(Func<Type, string> func)
        {
            getName = func;
            return this;
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