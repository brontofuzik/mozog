using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Mozog.Utils.Threading
{
    public class ThreadPoolParallelizer : IParallelizer
    {
        public void Parallelize(IEnumerable<Action> tasks)
        {
            int taskCount = tasks.Count();
            using (ManualResetEvent resetEvent = new ManualResetEvent(false))
            {
                foreach (var task in tasks)
                {
                    var newTask = task;
                    ThreadPool.QueueUserWorkItem(s =>
                    {
                        newTask();
                        if (Interlocked.Decrement(ref taskCount) == 0)
                            resetEvent.Set();
                    }, null);
                }
                resetEvent.WaitOne();
            }
        }
    }
}
