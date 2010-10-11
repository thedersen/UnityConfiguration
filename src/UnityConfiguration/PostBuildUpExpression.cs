using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class PostBuildUpExpression<T> : Expression where T : class
    {
        private Action<IUnityContainer, T> action;
        private Func<IUnityContainer, T, object> decoratorFunc;

        public void Call(Action<IUnityContainer, T> action)
        {
            this.action = action;
        }

        public void DecorateWith(Func<IUnityContainer, T, object> func)
        {
            decoratorFunc = func;
        }

        internal override void Execute(IUnityContainer container)
        {
            if (decoratorFunc != null)
                container.AddExtension(new DecoratorExtension<T>(decoratorFunc));

            if(action != null)
                container.AddExtension(new PostBuildUpActionExtension<T>(action));
        }
    }
}