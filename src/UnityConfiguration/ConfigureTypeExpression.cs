using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class ConfigureTypeExpression<T> : Expression
    {
        private readonly InjectionMember[] injectionMembers;

        public ConfigureTypeExpression(params InjectionMember[] injectionMembers)
        {
            this.injectionMembers = injectionMembers;
        }

        internal override void Execute(IUnityContainer container)
        {
            container.RegisterType<T>(injectionMembers);
        }
    }
}