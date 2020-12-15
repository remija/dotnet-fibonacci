using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fibonacci;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FibonacciController : ControllerBase
    {
        private readonly Compute _compute;

        public FibonacciController(Compute compute)
        {
            _compute = compute;
        }

        [HttpPost]
        public async Task<IList<int>> Get(IList<string> args)
        {
            return await _compute.ExecuteAsync(args.ToArray());
        }
    }
}
