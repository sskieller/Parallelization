using System;
using System.Collections.Generic;
using System.Threading;

namespace Parallelization.ForEach
{
    internal class MyParallelForEach
    {
        public void MyParallelForEachFunction<T>(IEnumerable<T> source, Action<T> body)
        {
            int numprocs = Environment.ProcessorCount;
            int remainingWorkItems = numprocs;

            using (var enumerator = source.GetEnumerator())
            {
                using (var mre = new ManualResetEvent(false))
                {
                    for (int p = 0; p < numprocs; p++)
                    {
                        ThreadPool.QueueUserWorkItem(delegate
                        {
                            // Iterates through until there is no more work
                            while (true)
                            {
                                // Get next item under a lock, and then process item
                                T nextItem;
                                lock (enumerator)
                                {
                                    if (!enumerator.MoveNext())
                                    {
                                        break;
                                    }

                                    nextItem = enumerator.Current;
                                }

                                body(nextItem);
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
}
