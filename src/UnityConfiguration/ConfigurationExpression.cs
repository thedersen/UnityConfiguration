using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class ConfigurationExpression<T> : Expression
    {
        private readonly InjectionMember[] injectionMembers;

        public ConfigurationExpression(params InjectionMember[] injectionMembers)
        {
            this.injectionMembers = injectionMembers;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterType<T>(injectionMembers);
        }
    }
}