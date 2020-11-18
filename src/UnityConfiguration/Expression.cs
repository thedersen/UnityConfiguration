using Unity;

namespace UnityConfiguration
{
    public abstract class Expression : IHideObjectMembers
    {
        internal abstract void Execute(IUnityContainer container);
    }
}