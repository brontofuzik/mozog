using System;
using System.Collections.Generic;

namespace Mozog.Search
{
    public class Metrics
    {
        private Dictionary<string, string> hash = new Dictionary<string, string>();

        public void Set(string name, int i)
        {
            hash[name] = i.ToString();
        }

        public void Set(string name, long l)
        {
            hash[name] = l.ToString();
        }

        public void Set(string name, double d)
        {
            hash[name] = d.ToString();
        }

        public String Get(string name) => hash[name];

        public int GetInt(string name)
        {
            string value = hash[name];
            return value != null ? Int32.Parse(value) : 0;
        }

        public long GetLong(string name)
        {
            string value = hash[name];
            return value != null ? Int64.Parse(value) : 0L;
        }

        public double GetDouble(string name)
        {
            string value = hash[name];
            return value != null ? Double.Parse(value) : Double.NaN;
        }

        public void IncrementInt(string name)
        {
            Set(name, GetInt(name) + 1);
        }

        public ISet<string> Keys => new HashSet<string>(hash.Keys);
    }
}
