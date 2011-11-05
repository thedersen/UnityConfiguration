using System;
using System.Linq;

namespace UnityConfiguration
{
    public static class TypeExtensions
    {
        public static bool IsConcrete(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        public static bool CanBeCreated(this Type type)
        {
            return type.IsConcrete() && type.GetConstructors().Length > 0;
        }

        public static bool CanBeCastTo(this Type typeFrom, Type typeTo)
        {
            if (typeFrom == null)
                return false;

            if (typeFrom.IsInterface || typeFrom.IsAbstract)
                return false;

            if (IsOpenGeneric(typeFrom))
                return false;

            return typeTo.IsAssignableFrom(typeFrom);
        }

        public static bool IsOpenGeneric(this Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

        public static bool IsInNamespace(this Type type, string @namespace)
        {
            if (type.Namespace != null)
                return type.Namespace.StartsWith(@namespace);

            return false;
        }

        public static bool ImplementsInterface(this Type type, Type implements)
        {
            return implements.IsInterface && type.GetInterface(implements.Name) != null;
        }

        public static bool ImplementsInterfaceTemplate(this Type type, Type templateType)
        {
            if (!type.IsConcrete()) 
                return false;

            return type.GetInterfaces().Any(interfaceType => interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == templateType);
        }
    }
}