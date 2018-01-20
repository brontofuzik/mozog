using System;
using System.Collections.Generic;

namespace Mozog.Search
{
    public class Metrics
    {
        private Dictionary<string, object> hash = new Dictionary<string, object>();

        public void Set<T>(string name, T m) => hash[name] = m;

        public T Get<T>(string name) => (T)hash[name];

        public void IncrementInt(string name) => Set(name, Get<int>(name) + 1);

        public ISet<string> Keys => new HashSet<string>(hash.Keys);
    }
}
