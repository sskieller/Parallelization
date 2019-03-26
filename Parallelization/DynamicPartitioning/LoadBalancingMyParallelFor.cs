using System;
using System.Threading;

namespace Parallelization.DynamicPartitioning
{
    internal class LoadBalancingMyParallelFor
    {
        public void MyParallelFor(int inclusiveLowerBound, int exclusiveUpperBound, Action<int> body)
        {
            var numProcs = Environment.ProcessorCount;
            var remainingWorkItems = numProcs;
            var nextIteration = inclusiveLowerBound;

            using (var mre = new ManualResetEvent(false))
            {
                for (int p = 0; p < numProcs; p++)
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        int index;
                        while ((index = Interlocked.Increment(
                                            ref nextIteration) - 1) < exclusiveUpperBound)
                        {
                            body(index);
                        }

                        if (Interlocked.Decrement(ref remainingWorkItems) == 0)
                        {
                            mre.Set();
                        }
                    });
                }

                mre.WaitOne();
            }
        }
    }
}
