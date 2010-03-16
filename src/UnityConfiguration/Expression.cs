using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public abstract class Expression
    {
        internal abstract void Execute(IUnityContainer container);
    }
}