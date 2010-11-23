namespace UnityConfiguration
{
    public static class Extensions
    {
        public static T As<T>(this object obj) where T : class
        {
            return obj as T;
        }
    }
}