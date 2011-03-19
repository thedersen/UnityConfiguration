using System;

namespace UnityConfiguration
{
    /// <summary>
    /// Convention that looks for <see cref="UnityRegistry"/> and imports it
    /// into the current <see cref="UnityRegistry"/>.
    /// </summary>
    public class ScanForRegistriesConvention : IAssemblyScannerConvention
    {
        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(typeof(UnityRegistry)) && type.CanBeCreated())
                registry.AddRegistry((UnityRegistry) type.GetConstructor(Type.EmptyTypes).Invoke(null));
        }
    }
}