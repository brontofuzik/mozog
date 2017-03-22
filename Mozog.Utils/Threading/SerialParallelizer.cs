using System;
using System.Collections.Generic;

namespace Mozog.Utils.Threading
{
    public class SerialParallelizer : IParallelizer
    {
        public void Parallelize(IEnumerable<Action> tasks)
        {
            foreach (var task in tasks)
            {
                task();
            }
        }
    }
}
