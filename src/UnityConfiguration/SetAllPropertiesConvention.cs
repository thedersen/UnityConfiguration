using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class SetAllPropertiesConvention : IAssemblyScannerConvention
    {
        private Type interfaceType;

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