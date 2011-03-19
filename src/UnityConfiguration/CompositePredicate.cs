/*
 * Copyright 2004-2009 Jeremy D. Miller
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

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

        private void Add(Func<T, bool> filter)
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