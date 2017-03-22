using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mozog.Utils.Threading
{
    public class ForLoopParallelizer : IParallelizer
    {
        public void Parallelize(IEnumerable<Action> tasks)
        {
            Parallel.ForEach(tasks, t => t());
        }
    }
}
