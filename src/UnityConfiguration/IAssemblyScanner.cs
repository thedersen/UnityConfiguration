using System;

namespace UnityConfiguration
{
    public interface IAssemblyScanner
    {
        void AssemblyContaining<T>();
        void With<TConvention>() where TConvention : IRegistrationConvention, new();
        void With(IRegistrationConvention convention);
        void ExcludeType<T>();
        void Exclude(Func<Type, bool> exclude);
        void ExcludeNamespaceContaining<T>();
        void ExcludeNamespace(string @namespace);
        void Include(Func<Type, bool> include);
        void IncludeNamespaceContaining<T>();
        void IncludeNamespace(string @namespace);
    }
}