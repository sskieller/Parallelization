using Parallelization.DynamicPartitioning;
using Parallelization.StaticPartitioning;
using System;
using System.Diagnostics;
using System.Threading;

namespace Parallelization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const int waitTimeBetweenFunctionCalls = 1500; // ms
            const int lowerBound = 2;
            const int upperBound = 500;


            var noThreadPools = new MyParallelForWithoutThreadPools();
            var threadPools = new MyParallelForWithThreadPool();
            var loadBalancer = new LoadBalancingMyParallelFor();
            var balancedLoadBalancer = new LoadBalancingBothStaticAndDynamic();

            var before = DateTime.Now;
            noThreadPools.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            var after = DateTime.Now;
            Debug.WriteLine("No thread pools: Elapsed time: " + (after - before).TotalMilliseconds);

            Thread.Sleep(waitTimeBetweenFunctionCalls);

            before = DateTime.Now;
            threadPools.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            after = DateTime.Now;
            Debug.WriteLine("With thread pools: Elapsed time: " + (after - before).TotalMilliseconds);

            Thread.Sleep(waitTimeBetweenFunctionCalls);

            before = DateTime.Now;
            loadBalancer.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            after = DateTime.Now;
            Debug.WriteLine("Dynamic load balancing: Elapsed time: " + (after - before).TotalMilliseconds);

            Thread.Sleep(waitTimeBetweenFunctionCalls);

            before = DateTime.Now;
            balancedLoadBalancer.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            after = DateTime.Now;
            Debug.WriteLine("Dynamic load balancing: Elapsed time: " + (after - before).TotalMilliseconds);

            Console.ReadKey();
        }
    }
}