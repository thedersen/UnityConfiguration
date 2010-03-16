using System;

namespace UnityConfiguration
{
    public interface IRegistrationConvention
    {
        void Process(Type type, IUnityRegistry registry);
    }
}