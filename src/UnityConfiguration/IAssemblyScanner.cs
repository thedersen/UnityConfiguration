using System;

namespace UnityConfiguration
{
    public interface IAssemblyScanner
    {
        void AssemblyContaining<T>();
        TConvention With<TConvention>() where TConvention : RegistrationConvention, new();
        void ExcludeType<T>();
        void Exclude(Func<Type, bool> exclude);
        void ExcludeNamespaceContaining<T>();
        void ExcludeNamespace(string @namespace);
        void Include(Func<Type, bool> include);
        void IncludeNamespaceContaining<T>();
        void IncludeNamespace(string @namespace);
    }
}