using System;

namespace UnityConfiguration
{
    /// <summary>
    /// Convention for registering all types by a naming convention.
    /// By default it tries to find an interface with the same name as the
    /// service, prefixed with an I, but this can be overridden.
    /// </summary>
    public class NamingConvention : IAssemblyScannerConvention
    {
        private Func<Type, string> getInterfaceName = t => "I" + t.Name;

        /// <summary>
        /// Specify how to resolve the name of the interface.
        /// </summary>
        /// <param name="func">The function to create the name of the interface.</param>
        public void WithInterfaceName(Func<Type, string> func)
        {
            getInterfaceName = func;
        }

        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            Type @interface = FindInterface(type);

            if(@interface != null)
                registry.Register(@interface, type);
        }

        private Type FindInterface(Type type)
        {
            string interfaceName = getInterfaceName(type);

            return type.GetInterface(interfaceName);
        }
    }
}