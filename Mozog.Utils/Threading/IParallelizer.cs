using System;
using System.Collections.Generic;

namespace Mozog.Utils.Threading
{
    public interface IParallelizer
    {
        void Parallelize(IEnumerable<Action> tasks);
    }
}
