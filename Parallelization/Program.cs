using Parallelization.DynamicPartitioning;
using Parallelization.StaticPartitioning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Parallelization.ForEach;

namespace Parallelization
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const int waitTimeBetweenFunctionCalls = 1500; // ms
            const int lowerBound = 2;
            const int upperBound = 100;
            
            var range = Enumerable.Range(lowerBound,upperBound);

            var noThreadPools = new MyParallelForWithoutThreadPools();
            var threadPools = new MyParallelForWithThreadPool();
            var loadBalancer = new LoadBalancingMyParallelFor();
            var balancedLoadBalancer = new LoadBalancingBothStaticAndDynamic();

            var parallelForEach = new MyParallelForEach();

            var before = DateTime.Now;
            var after = DateTime.Now;


            //before = DateTime.Now;
            //noThreadPools.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            //after = DateTime.Now;
            //Debug.WriteLine("No thread pools: Elapsed time: " + (after - before).TotalMilliseconds);

            //Thread.Sleep(waitTimeBetweenFunctionCalls);

            //before = DateTime.Now;
            //threadPools.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            //after = DateTime.Now;
            //Debug.WriteLine("With thread pools: Elapsed time: " + (after - before).TotalMilliseconds);

            //Thread.Sleep(waitTimeBetweenFunctionCalls);

            //before = DateTime.Now;
            //loadBalancer.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            //after = DateTime.Now;
            //Debug.WriteLine("Dynamic load balancing: Elapsed time: " + (after - before).TotalMilliseconds);

            //Thread.Sleep(waitTimeBetweenFunctionCalls);

            //before = DateTime.Now;
            //balancedLoadBalancer.MyParallelFor(lowerBound, upperBound, Console.WriteLine);
            //after = DateTime.Now;
            //Debug.WriteLine("Balanced load balancing: Elapsed time: " + (after - before).TotalMilliseconds);

            //Thread.Sleep(waitTimeBetweenFunctionCalls);

            //before = DateTime.Now;
            //parallelForEach.MyParallelForEachFunction(range, Console.WriteLine);
            //after = DateTime.Now;
            //Debug.WriteLine("For each: Elapsed time: " + (after - before).TotalMilliseconds);


            var students = new List<Student>();
            for (var i = 0; i < 100000; i++)
            {
                students.Add(new Student(i, i*10));
            }

            before = DateTime.Now;
            foreach (var student in students)
            {
                student.Sum = student.Grade * student.Weight;
            }
            after = DateTime.Now;
            Debug.WriteLine("Students sequentially: Elapsed time: " + (after - before).TotalMilliseconds);

            Thread.Sleep(250);


            before = DateTime.Now;
            Parallel.ForEach(students, student => { student.Sum = student.Grade * student.Weight; });
            after = DateTime.Now;
            Debug.WriteLine("Students parallel: Elapsed time: " + (after - before).TotalMilliseconds);



            Console.ReadKey();
        }
    }

    class Student
    {
        public Student(int grade, int weight)
        {
            Grade = grade;
            Weight = weight;

            Sum = 0;
        }
        public int Grade;
        public int Weight;

        public int Sum;
    }
}