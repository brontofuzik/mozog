using System.Threading;

namespace Mozog.Search
{
    public class ThreadingUtils
    {
        public static Thread RunThreadInBackground(ThreadStart task)
        {
            var result = new CancellableThread(task);
            result.SetDaemon(true);
            result.Start();
            return result;
        }

        public static void CancelThread(Thread thread)
        {
            if (thread is CancellableThread)
                ((CancellableThread)thread).Cancel();
        }

        public static bool IsCurrentThreadCancelled()
        {
            return (Thread.currentThread() is CancellableThread)
                ? ((CancellableThread)Thread.currentThread()).IsCancelled
                : false;
        }
    }
}
