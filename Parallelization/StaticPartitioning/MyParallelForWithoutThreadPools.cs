using System;
using System.Collections.Generic;
using System.Threading;

namespace Parallelization.StaticPartitioning
{
    public class MyParallelForWithoutThreadPools
    {
        public void MyParallelFor(int inclusiveLowerBound, int exclusiveUpperBound, Action<int> body)
        {
            // Get number of iterations to be processed, number of cores to use, and a number of iterations
            // to be processed in each thread
            var size = exclusiveUpperBound - inclusiveLowerBound;
            var numberOfProcessors = Environment.ProcessorCount;
            Console.WriteLine($"Number of processors: {numberOfProcessors}");
            var range = size / numberOfProcessors;

            // Using a thread for each partition. 1. Create them all. 2. Start them all. 3. Wait for them all.
            var threads = new List<Thread>(numberOfProcessors);
            for (var p = 0; p < numberOfProcessors; p++)
            {
                var start = p * range + inclusiveLowerBound;
                var end = (p == numberOfProcessors - 1) ? exclusiveUpperBound : start + range;

                threads.Add(new Thread(() =>
                {
                    for (var i = start; i < end; i++)
                    {
                        body(i);
                    }
                }));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}
