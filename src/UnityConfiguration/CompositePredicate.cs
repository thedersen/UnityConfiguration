using System;
using System.Collections.Generic;
using System.Linq;

namespace UnityConfiguration
{
    public class CompositePredicate<T>
    {
        private readonly List<Func<T, bool>> filters = new List<Func<T, bool>>();
        private Func<T, bool> matchesAll = x => true;
        private Func<T, bool> matchesAny = x => true;
        private Func<T, bool> matchesNone = x => false;

        public void Add(Func<T, bool> filter)
        {
            matchesAll = x => filters.All(predicate => predicate(x));
            matchesAny = x => filters.Any(predicate => predicate(x));
            matchesNone = x => !MatchesAny(x);

            filters.Add(filter);
        }

        public static CompositePredicate<T> operator +(CompositePredicate<T> invokes, Func<T, bool> filter)
        {
            invokes.Add(filter);
            return invokes;
        }

        public bool MatchesAll(T target)
        {
            return matchesAll(target);
        }

        public bool MatchesAny(T target)
        {
            return matchesAny(target);
        }

        public bool MatchesNone(T target)
        {
            return matchesNone(target);
        }

        public bool DoesNotMatcheAny(T target)
        {
            return filters.Count == 0 ? true : !MatchesAny(target);
        }
    }
}