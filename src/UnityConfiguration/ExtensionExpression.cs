using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class ExtensionExpression<TExtension> : Expression where TExtension : UnityContainerExtension, new()
    {
        internal override void Execute(IUnityContainer container)
        {
            container.AddNewExtension<TExtension>();
        }
    }
}