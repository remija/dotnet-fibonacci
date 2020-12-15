using System;

namespace DemoFromNuget
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = Fibonacci.Compute.Execute(args);
            foreach (var i in result)
            {
                    Console.WriteLine($"Result: {i}");
            }
        }
    }
}
