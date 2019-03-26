using System;
using System.Threading;

namespace Parallelization.StaticPartitioning
{
    public class MyParallelForWithThreadPool
    {
        public void MyParallelFor(int inclusiveLowerBound, int exclusiveUpperBound, Action<int> body)
        {
            var size = exclusiveUpperBound - inclusiveLowerBound;
            var numProcs = Environment.ProcessorCount;
            var range = size / numProcs;

            // Keep track of threads needing to complete
            var remaining = numProcs;

            using (var mre = new ManualResetEvent(false))
            {
                for (var p = 0; p < numProcs; p++)
                {
                    var start = p * range + inclusiveLowerBound;
                    var end = (p == numProcs - 1) ? exclusiveUpperBound : start + range;
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        for (var i = start; i < end; i++)
                        {
                            body(i);
                            if (Interlocked.Decrement(ref remaining) == 0)
                            {
                                mre.Set();
                            }
                        }
                    });
                }

                mre.WaitOne();
            }
        }
    }
}
