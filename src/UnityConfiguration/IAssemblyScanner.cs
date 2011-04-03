using System;
using System.Reflection;

namespace UnityConfiguration
{
    public interface IAssemblyScanner : IHideObjectMembers
    {
        /// <summary>
        /// Add an assembly to scan.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        void Assembly(Assembly assembly);
        /// <summary>
        /// Add an assembly to scan given its display name.
        /// </summary>
        /// <param name="assemblyName">The display name of the assembly to scan (e.g. UnityConfiguration).</param>
        void Assembly(string assemblyName);
        /// <summary>
        /// Add an assembly to scan by specifying a type it contains.
        /// </summary>
        /// <typeparam name="T">A type that resides in the assembly to scan.</typeparam>
        void AssemblyContaining<T>();
        /// <summary>
        /// Add all assemblies in the application base directory of the current app domain to the scanner.
        /// </summary>
        void AssembliesInBaseDirectory();
        /// <summary>
        /// Add all assemblies in the application base directory of the current app domain to the scanner.
        /// </summary>
        /// <param name="predicate">A predicate used for filtering out assemblies.</param>
        void AssembliesInBaseDirectory(Predicate<Assembly> predicate);
        /// <summary>
        /// Add all assemblies in the specified path to the scanner.
        /// </summary>
        /// <param name="path">The path to scan for assemblies.</param>
        void AssembliesInDirectory(string path);
        /// <summary>
        /// Add all assemblies in the specified path to the scanner.
        /// </summary>
        /// <param name="path">The path to scan for assemblies.</param>
        /// <param name="predicate">A predicate used for filtering out assemblies.</param> 
        void AssembliesInDirectory(string path, Predicate<Assembly> predicate);
        /// <summary>
        /// Add a convention to use when scanning.
        /// </summary>
        /// <typeparam name="TConvention">The type of the convention.</typeparam>
        /// <returns>An instance of the convention that can be used for configuring the convention.</returns>
        TConvention With<TConvention>() where TConvention : IAssemblyScannerConvention, new();
        /// <summary>
        /// Adds a convention that will look for and include all <see cref="IUnityRegistry"/> that it finds.
        /// </summary>
        void ForRegistries();
        /// <summary>
        /// Exclude the specified type when scanning.
        /// </summary>
        /// <typeparam name="T">Type of which to exclude.</typeparam>
        void ExcludeType<T>();
        /// <summary>
        /// Exclude types that match the specified predicate when scanning. 
        /// </summary>
        /// <param name="exclude">The predicate to use for matching.</param>
        void Exclude(Func<Type, bool> exclude);
        /// <summary>
        /// Exclude all types in the same namespace as the specified type or its sub namespaces
        /// when scanning.
        /// </summary>
        /// <typeparam name="T">A type in the namespace to exclude.</typeparam>
        void ExcludeNamespaceContaining<T>();
        /// <summary>
        /// Exclude all types in the specified namespace or its sub namespaces when scanning.
        /// </summary>
        /// <param name="namespace">The namespace to exclude.</param>
        void ExcludeNamespace(string @namespace);
        /// <summary>
        /// Only include types that match the specified predicate when scanning.
        /// </summary>
        /// <param name="include">The predicate to use for matching.</param>
        void Include(Func<Type, bool> include);
        /// <summary>
        /// Only include types in the same namespace as the specified type or its sub namespaces
        /// when scanning.
        /// </summary>
        /// <typeparam name="T">A type in the namespace to include.</typeparam>
        void IncludeNamespaceContaining<T>();
        /// <summary>
        /// Only include types in the specified namespace or its sub namespaces when scanning.
        /// </summary>
        /// <param name="namespace">The namespace to include.</param>
        void IncludeNamespace(string @namespace);
    }
}