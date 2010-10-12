using System;
using System.Linq;

namespace UnityConfiguration
{
    public class FirstInterfaceConvention : RegistrationConvention
    {
        private bool ignoreBaseTypes;

        public void IgnoreInterfacesOnBaseTypes()
        {
            ignoreBaseTypes = true;
        }

        internal override void Process(Type type, IUnityRegistry registry)
        {
            if (!type.IsConcrete() || !type.CanBeCreated()) 
                return;

            Type interfaceType = GetInterfaceType(type);

            if (interfaceType != null)
                registry.Register(interfaceType, type);
        }

        private Type GetInterfaceType(Type type)
        {
            Type[] interfaces = type.GetInterfaces();
            Type interfaceType = interfaces.FirstOrDefault();

            if (!ignoreBaseTypes || type.BaseType == null)
                return interfaceType;

            foreach (var @interface in interfaces)
            {
                if (!type.BaseType.ImplementsInterface(@interface))
                   return @interface;
            }

            return interfaceType;
        }
    }
}