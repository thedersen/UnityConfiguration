using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class AfterBuildUpExpression<T> : Expression where T : class
    {
        private Action<T> action;

        public void Call(Action<T> action)
        {
            this.action = action;
        }

        internal override void Execute(IUnityContainer container)
        {
            if(action != null)
                container.AddExtension(new AfterBuildUpExtension<T>(action));
        }
    }
}