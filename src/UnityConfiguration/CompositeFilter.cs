namespace UnityConfiguration
{
    public class CompositeFilter<T>
    {
        private readonly CompositePredicate<T> excludes = new CompositePredicate<T>();
        private readonly CompositePredicate<T> includes = new CompositePredicate<T>();

        public CompositePredicate<T> Includes
        {
            get { return includes; }
            set { }
        }

        public CompositePredicate<T> Excludes
        {
            get { return excludes; }
            set {  }
        }

        public bool Matches(T target)
        {
            return Includes.MatchesAny(target) && Excludes.DoesNotMatcheAny(target);
        }
    }
}