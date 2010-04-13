using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnityConfiguration
{
    public class AddAllAsSingletonConvention : RegistrationConvention
    {
        private Type interfaceType;

        public void TypesImplementing<T>()
        {
            interfaceType = typeof(T);
        }
    
        internal override void Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(interfaceType) && type.CanBeCreated())
            {
                registry.Register(interfaceType, type).WithName(type.Name).AsSingleton();
            }
        }
    }
}
