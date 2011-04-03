using System;

namespace UnityConfiguration
{
    /// <summary>
    /// Interface implemented by all scanner conventions. 
    /// Defines a method to process each type the scanner finds.
    /// </summary>
    public interface IAssemblyScannerConvention : IHideObjectMembers
    {
        /// <summary>
        /// Gets called for each type the <see cref="IAssemblyScanner"/> finds.
        /// </summary>
        /// <param name="type">The type to process.</param>
        /// <param name="registry">
        /// The instance of the <see cref="IUnityRegistry"/> 
        /// that can be used to register the type.
        /// </param>
        void Process(Type type, IUnityRegistry registry);
    }
}