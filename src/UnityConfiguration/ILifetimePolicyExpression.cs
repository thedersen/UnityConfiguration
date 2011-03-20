namespace UnityConfiguration
{
    public interface ILifetimePolicyExpression
    {
        /// <summary>
        /// Indicates that only a single instance should be created, and then
        /// should be re-used for all subsequent requests.
        /// </summary>
        void AsSingleton();
        /// <summary>
        /// Indicates that instances should not be re-used, nor have
        /// their lifecycle managed by Unity.
        /// </summary>
        void AsTransient();
        /// <summary>
        /// Indicates that instances should be re-used within the same thread.
        /// </summary>
        void AsPerThread();
    }
}