using Unity;
using Unity.Extension;

namespace UnityConfiguration
{
    public class AddNewExtensionExpression<T> : Expression where T : UnityContainerExtension, new()
    {
        internal override void Execute(IUnityContainer container)
        {
            container.AddNewExtension<T>();
        }
    }
}