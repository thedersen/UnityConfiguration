using System;

namespace UnityConfiguration
{
    public class AddAllConvention : IRegistrationConvention
    {
        private readonly Type interfaceType;

        public AddAllConvention(Type interfaceType)
        {
            this.interfaceType = interfaceType;
        }

        public void Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(interfaceType) && type.CanBeCreated())
            {
                registry.Register(interfaceType, type).WithName(type.Name);
            }
        }
    }
}