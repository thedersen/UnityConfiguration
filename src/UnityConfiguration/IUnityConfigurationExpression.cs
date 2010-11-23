namespace UnityConfiguration
{
    public interface IUnityConfigurationExpression : IUnityRegistry
    {
        void AddRegistry<T>() where T : UnityRegistry, new();
    }
}