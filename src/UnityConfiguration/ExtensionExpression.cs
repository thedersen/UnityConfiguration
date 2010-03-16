using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class ExtensionExpression<T> : Expression where T : UnityContainerExtension, new()
    {
        internal override void Execute(IUnityContainer container)
        {
            container.AddNewExtension<T>();
        }
    }
}