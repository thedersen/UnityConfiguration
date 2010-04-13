using System;
using System.Linq;

namespace UnityConfiguration
{
    public class FirstInterfaceConvention : RegistrationConvention
    {
        internal override void Process(Type type, IUnityRegistry registry)
        {
            if (!type.IsConcrete() || !type.CanBeCreated()) 
                return;
            
            Type interfaceType = type.GetInterfaces().FirstOrDefault();
            if (interfaceType != null)
            {
                registry.Register(interfaceType, type);
            }
        }
    }
}