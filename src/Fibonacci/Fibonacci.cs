using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Fibonacci
{
    public class Compute
    {
        private readonly FibonacciDataContext _fibonacciDataContext;

        public Compute(FibonacciDataContext fibonacciDataContext)
        {
            _fibonacciDataContext = fibonacciDataContext;
        }
        public static int Fib(int i)
        {
            if (i <= 2) return 1;
            return Fib(i - 2) + Fib(i - 1);
        }

        public async Task<IList<int>> ExecuteAsync(string[] args)
        {
            var results = new List<int>();
           // using (var fibonacciDataContext = new FibonacciDataContext())
            {
                IList<Task<int>> tasks = new List<Task<int>>();
                foreach (var arg in args)
                {
                    var i = int.Parse(arg);
                    var output =  await _fibonacciDataContext.TFibonacci.Where(f => f.FibInput == i).Select(f => f.FibOutput).FirstOrDefaultAsync();
                    Task<int> task;
                    if (output == default)
                    {
                        task = Task.Run(() => Fib(i));
                    }
                    else
                    {
                        task = Task.FromResult((int)output);
                    }
                    tasks.Add(task);
                };

                foreach (var task in tasks)
                {
                    var indexOf = tasks.IndexOf(task);
                    var arg = args[indexOf];
                    results.Add(task.Result);
                    _fibonacciDataContext.TFibonacci.Add(new TFibonacci()
                    {
                        FibInput = int.Parse(arg),
                        FibOutput = task.Result,
                        FibCreatedTimestamp = DateTime.Now
                    });
                }

                await _fibonacciDataContext.SaveChangesAsync();
                
                return results;
            }
        }
    }
}