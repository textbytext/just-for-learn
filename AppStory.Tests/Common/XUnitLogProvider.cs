using Microsoft.Extensions.Logging;
using System;
using Xunit.Abstractions;

namespace AppStory.Tests
{
    public class XUnitLogProvider : ILoggerProvider
    {
        private readonly ITestOutputHelper _output;

        public XUnitLogProvider(ITestOutputHelper output)
        {
            _output = output;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new XUnitLogger(categoryName, _output);
        }

        public void Dispose()
        {
            // empty
        }
    }

    internal class XUnitLogger : ILogger
    {
        private readonly ITestOutputHelper _output;
        private readonly string _categoryName;

        public XUnitLogger(string categoryName, ITestOutputHelper output)
        {
            _output = output;
            _categoryName = categoryName;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return new MoqDispose();
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            _output.WriteLine($@"{DateTime.Now} [{logLevel}] {_categoryName}:
{formatter(state, exception)}
");
        }
    }

    internal class MoqDispose : IDisposable
    {
        public void Dispose()
        {
            // Method intentionally left empty.
        }
    }
}
