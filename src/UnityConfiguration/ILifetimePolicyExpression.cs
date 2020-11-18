using Unity.Lifetime;

namespace UnityConfiguration
{
    public interface ILifetimePolicyExpression : IHideObjectMembers
    {
        /// <summary>
        /// Specify how lifetime should be managed by the container, by specifying a <see cref="LifetimeManager"/>.
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="LifetimeManager"/> to use.</typeparam>
        void Using<T>() where T : LifetimeManager, new();
    }
}