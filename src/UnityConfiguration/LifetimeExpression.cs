using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class LifetimeExpression<T> : Expression
    {
        private readonly LifetimeManager lifetimeManager;

        public LifetimeExpression(LifetimeManager lifetimeManager)
        {
            this.lifetimeManager = lifetimeManager;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterType<T>(lifetimeManager);
        }
    }
}