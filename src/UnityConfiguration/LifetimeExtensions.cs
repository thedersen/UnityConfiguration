using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public static class LifetimeExtensions
    {
        /// <summary>
        /// Indicates that only a single instance should be created, and then
        /// should be re-used for all subsequent requests.
        /// </summary>
        public static void AsSingleton(this ILifetimePolicyExpression o)
        {
            o.Using<ContainerControlledLifetimeManager>();
        }

        /// <summary>
        /// Indicates that instances should not be re-used, nor have
        /// their lifecycle managed by Unity.
        /// </summary>
        public static void AsTransient(this ILifetimePolicyExpression o)
        {
            o.Using<TransientLifetimeManager>();
        }
        
        /// <summary>
        /// Indicates that instances should be re-used within the same thread.
        /// </summary>
        public static void AsPerThread(this ILifetimePolicyExpression o)
        {
            o.Using<PerThreadLifetimeManager>();
        }
        
        /// <summary>
        /// Indicates that instances should be re-used within the same build up object graph.
        /// </summary>
        public static void AsPerResolve(this ILifetimePolicyExpression o)
        {
            o.Using<PerResolveLifetimeManager>();
        }
        
        /// <summary>
        /// Indicates that Unity maintains only a weak reference to the objects it creates.
        /// Instances are re-used as long as the weak reference is alive.
        /// </summary>
        public static void AsExternallyControlled(this ILifetimePolicyExpression o)
        {
            o.Using<ExternallyControlledLifetimeManager>();
        }
        
        /// <summary>
        /// Acts like a singleton, except that in the presence of child containers, 
        /// each child gets it's own instance of the object, instead of sharing one in the common parent.
        /// </summary>
        public static void AsHierarchicalControlled(this ILifetimePolicyExpression o)
        {
            o.Using<HierarchicalLifetimeManager>();
        }
    }
}