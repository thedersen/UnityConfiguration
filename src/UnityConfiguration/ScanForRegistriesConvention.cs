using System;

namespace UnityConfiguration
{
    public class ScanForRegistriesConvention : IAssemblyScannerConvention
    {

        void IAssemblyScannerConvention.Process(Type type, IUnityRegistry registry)
        {
            if (type.CanBeCastTo(typeof(UnityRegistry)) && type.CanBeCreated())
                registry.AddRegistry((UnityRegistry) type.GetConstructor(Type.EmptyTypes).Invoke(null));
        }
    }
}