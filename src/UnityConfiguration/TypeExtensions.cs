using System;

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
            return type.Namespace.StartsWith(@namespace);
        }
    }
}