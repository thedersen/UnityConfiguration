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
            set { }
        }

        public bool Matches(T target)
        {
            return Includes.MatchesAny(target) && Excludes.DoesNotMatcheAny(target);
        }
    }
}