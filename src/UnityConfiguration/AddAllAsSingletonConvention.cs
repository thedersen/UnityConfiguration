using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityConfiguration
{
    public class AddAllAsSingletonConvention : IRegistrationConvention
    {
        private readonly Type interfaceType;

        public AddAllAsSingletonConvention(Type interfaceType)
        {
            this.interfaceType = interfaceType;
        }

        public void Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(interfaceType) && type.CanBeCreated())
            {
                registry.Register(interfaceType, type).WithName(type.Name).AsSingleton();
            }
        }
    }
}
