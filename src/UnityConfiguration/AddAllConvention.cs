using System;

namespace UnityConfiguration
{
    public class AddAllConvention : RegistrationConvention
    {
        private Type interfaceType;

        public void TypesImplementing<T>()
        {
            interfaceType = typeof (T);
        }
        
        internal override void Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(interfaceType) && type.CanBeCreated())
            {
                registry.Register(interfaceType, type).WithName(type.Name);
            }
        }
    }
}