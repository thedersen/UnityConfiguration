using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    /// <summary>
    /// Convention that looks for a writable property of a specified type
    /// and configures injection for it. Mostly useful for optional dependencies
    /// like loggers etc.
    /// </summary>
    public class SetAllPropertiesConvention : IAssemblyScannerConvention
    {
        private Type interfaceType;

        /// <summary>
        /// Specify the type of the property to inject.
        /// </summary>
        /// <typeparam name="T">The type of the property to inject.</typeparam>
        public void OfType<T>()
        {
            interfaceType = typeof (T);
        }

        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            IEnumerable<PropertyInfo> properties =
                type.GetProperties().Where(p => p.CanWrite && p.PropertyType == interfaceType);

            foreach (PropertyInfo property in properties)
            {
                registry.Register(null, type).WithInjectionMembers(new InjectionProperty(property.Name));
            }
        }
    }
}