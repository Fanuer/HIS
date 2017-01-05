using System;
using Microsoft.Extensions.Logging;

namespace HIS.Helpers.Test
{
    public class ConsoleLoggerProvider : ILoggerProvider
    {
        public ILogger CreateLogger(string categoryName)
        {
            return new MyLogger();
        }

        public void Dispose()
        { }

        private class MyLogger : ILogger
        {
            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                /*if (!File.Exists(@"C:\temp\ef.log.txt"))
                {
                    File.Create(@"C:\temp\ef.log.txt");
                }
                File.AppendAllText(@"C:\temp\ef.log.txt", formatter(state, exception));*/
                Console.WriteLine(formatter(state, exception));
            }

            public IDisposable BeginScope<TState>(TState state)
            {
                return null;
            }
        }
    }
}
