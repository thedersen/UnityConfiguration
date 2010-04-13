using System;

namespace UnityConfiguration
{
    public abstract class RegistrationConvention
    {
        internal abstract void Process(Type type, IUnityRegistry registry);
    }
}