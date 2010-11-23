namespace UnityConfiguration
{
    public interface IInitializationExpression : IUnityRegistry
    {
        void AddRegistry<T>() where T : UnityRegistry, new();
    }
}