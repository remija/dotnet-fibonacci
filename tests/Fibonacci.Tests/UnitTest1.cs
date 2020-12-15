using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fibonacci.Tests
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var builder = new DbContextOptionsBuilder<FibonacciDataContext>();
            var dataBaseName = Guid.NewGuid().ToString();
            builder.UseInMemoryDatabase(dataBaseName);
            var options = builder.Options;
            using (var fibonacciDataContext = new FibonacciDataContext(options))
            {
                await fibonacciDataContext.Database.EnsureCreatedAsync();
                
                
                
                var result = new Compute(fibonacciDataContext).ExecuteAsync(new[] {"44"});
                Assert.Equal(701408733, result.Result[0]);
            }
        }
    }
}
