using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mozog.Utils.Threading
{
    public interface IParallelizer
    {
        void Parallelize(IEnumerable<Action> tasks);
    }
}
