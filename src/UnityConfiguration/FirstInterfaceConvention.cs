using System;
using System.Linq;

namespace UnityConfiguration
{
    /// <summary>
    /// Convention for registering all types by the first interface defined for the type.
    /// </summary>
    public class FirstInterfaceConvention : IAssemblyScannerConvention
    {
        private bool ignoreBaseTypes;

        /// <summary>
        /// Determines whether or not to ignore interfaces on base types. Default false.
        /// </summary>
        public void IgnoreInterfacesOnBaseTypes()
        {
            ignoreBaseTypes = true;
        }

        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
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

            foreach (Type @interface in interfaces)
            {
                if (!type.BaseType.ImplementsInterface(@interface))
                    return @interface;
            }

            return interfaceType;
        }
    }
}