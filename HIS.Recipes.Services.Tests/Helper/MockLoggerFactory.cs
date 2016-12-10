using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.Services.Tests.Helper
{
    public class MockLoggerFactory<T>:ILoggerFactory
    {
        public void Dispose()
        {
            
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new MockLogger<T>();
        }

        public void AddProvider(ILoggerProvider provider)
        {
            
        }
    }
}
