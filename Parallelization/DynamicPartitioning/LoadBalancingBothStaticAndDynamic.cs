using System;
using System.Threading;

namespace Parallelization.DynamicPartitioning
{
    internal class LoadBalancingBothStaticAndDynamic
    {
        public void MyParallelFor(
            int inclusiveLowerBound, int exclusiveUpperBound, Action<int> body)
        {
            int numprocs = Environment.ProcessorCount;
            int remainingWorkItems = numprocs;
            int nextIteration = inclusiveLowerBound;
            const int batchSize = 3;

            using (var mre = new ManualResetEvent(false))
            {
                // Create each of the work items
                for (int p = 0; p < numprocs; p++)
                {
                    ThreadPool.QueueUserWorkItem(delegate
                    {
                        int index;
                        while ((index = Interlocked.Add(
                                            ref nextIteration, batchSize) - batchSize) < exclusiveUpperBound)
                        {
                            // In real implementation remember to handle overflow on this arithmetic
                            int end = index + batchSize;
                            if (end >= exclusiveUpperBound)
                            {
                                end = exclusiveUpperBound;
                            }

                            for (int i = index; i < end; i++)
                            {
                                body(i);
                            }
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
