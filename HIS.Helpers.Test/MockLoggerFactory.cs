using Microsoft.Extensions.Logging;

namespace HIS.Helpers.Test
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
