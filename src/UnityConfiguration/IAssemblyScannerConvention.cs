using System;

namespace UnityConfiguration
{
    public interface IAssemblyScannerConvention
    {
        void Process(Type type, IUnityRegistry registry);
    }
}