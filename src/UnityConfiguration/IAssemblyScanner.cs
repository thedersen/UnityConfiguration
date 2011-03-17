using System;
using System.Reflection;

namespace UnityConfiguration
{
    public interface IAssemblyScanner
    {
        void Assembly(Assembly assembly);
        void Assembly(string assemblyName);
        void AssemblyContaining<T>();
        void AssembliesInBaseDirectory();
        void AssembliesInDirectory(string path);

        TConvention With<TConvention>() where TConvention : IAssemblyScannerConvention, new();

        void ExcludeType<T>();
        void Exclude(Func<Type, bool> exclude);
        void ExcludeNamespaceContaining<T>();
        void ExcludeNamespace(string @namespace);
        void Include(Func<Type, bool> include);
        void IncludeNamespaceContaining<T>();
        void IncludeNamespace(string @namespace);
    }
}