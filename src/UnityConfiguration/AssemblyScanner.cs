using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace UnityConfiguration
{
    public class AssemblyScanner : IAssemblyScanner
    {
        private readonly List<Assembly> assemblies = new List<Assembly>();
        private readonly List<IAssemblyScannerConvention> conventions = new List<IAssemblyScannerConvention>();
        private readonly CompositeFilter<Type> filter = new CompositeFilter<Type>();
        private Func<Assembly, IEnumerable<Type>> getTypes = a => a.GetExportedTypes();

        public void Assembly(Assembly assembly)
        {
            if (!assemblies.Contains(assembly))
                assemblies.Add(assembly);
        }

        public void Assembly(string assemblyName)
        {
            assemblyName = Regex.Replace(assemblyName, ".dll$", string.Empty);
            Assembly(AppDomain.CurrentDomain.Load(assemblyName));
        }

        public void AssemblyContaining<T>()
        {
            Assembly(typeof(T).Assembly);
        }

        public void AssembliesInBaseDirectory()
        {
            AssembliesInBaseDirectory(x => true);
        }

        public void AssembliesInBaseDirectory(Predicate<Assembly> predicate)
        {
            AssembliesInDirectory(AppDomain.CurrentDomain.BaseDirectory, predicate);
            AssembliesInDirectory(AppDomain.CurrentDomain.SetupInformation.PrivateBinPath, predicate);
        }

        public void AssembliesInDirectory(string path)
        {
            AssembliesInDirectory(path, a => true);
        }

        public void AssembliesInDirectory(string path, Predicate<Assembly> predicate)
        {
            if (!Directory.Exists(path))
                return;

            var assemblyPaths = from file in Directory.GetFiles(path)
                                let extension = Path.GetExtension(file)
                                where extension.Equals(".exe", StringComparison.OrdinalIgnoreCase) ||
                                      extension.Equals(".dll", StringComparison.OrdinalIgnoreCase)
                                select file;

            foreach (string assemblyPath in assemblyPaths)
            {
                Assembly assembly = null;
                try
                {
                    assembly = System.Reflection.Assembly.LoadFrom(assemblyPath);
                }
                catch
                {
                    // ignore
                }

                if (assembly != null && predicate(assembly))
                    Assembly(assembly);
            }
        }

        public void InternalTypes()
        {
            getTypes = a => a.GetTypes();
        }

        public TConvention With<TConvention>() where TConvention : IAssemblyScannerConvention, new()
        {
            var convention = new TConvention();
            conventions.Add(convention);

            return convention;
        }

        public void Exclude(Predicate<Type> exclude)
        {
            filter.Excludes += exclude;
        }

        public void ExcludeNamespace(string @namespace)
        {
            Exclude(type => type.IsInNamespace(@namespace));
        }

        public void ExcludeNamespaceContaining<T>()
        {
            ExcludeNamespace(typeof(T).Namespace);
        }

        public void ExcludeType<T>()
        {
            Exclude(type => type == typeof(T));
        }

        public void Include(Predicate<Type> include)
        {
            filter.Includes += include;
        }

        public void IncludeNamespace(string @namespace)
        {
            Include(type => type.IsInNamespace(@namespace));
        }

        public void IncludeNamespaceContaining<T>()
        {
            IncludeNamespace(typeof(T).Namespace);
        }

        public void Scan(IUnityRegistry registry)
        {
            GetExportedTypes().ForEach(type => ApplyConventions(type, registry));
        }

        private IEnumerable<Type> GetExportedTypes()
        {
            return assemblies.SelectMany(getTypes).Where(t => filter.Matches(t));
        }

        private void ApplyConventions(Type type, IUnityRegistry registry)
        {
            conventions.ForEach(c => c.Process(type, registry));
        }
    }
}