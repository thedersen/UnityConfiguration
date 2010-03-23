using System;
using Microsoft.Practices.Unity;

namespace UnityConfiguration
{
    public class PostBuildUpExpression<T> : Expression where T : class
    {
        private Action<T> action;
        private Func<T, object> decoratorFunc;

        public void Call(Action<T> action)
        {
            this.action = action;
        }

        public void DecorateWith(Func<T, object> func)
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