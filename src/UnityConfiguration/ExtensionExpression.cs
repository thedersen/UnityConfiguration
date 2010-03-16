using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class ExtensionExpression : Expression
    {
        private readonly UnityContainerExtension extension;

        public ExtensionExpression(UnityContainerExtension extension)
        {
            this.extension = extension;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.AddExtension(extension);
        }
    }
}