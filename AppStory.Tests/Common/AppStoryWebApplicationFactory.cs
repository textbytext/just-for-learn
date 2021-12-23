using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace AppStory.Tests
{
    public class AppStoryWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private ILoggerProvider[] _loggers = new ILoggerProvider[] { };

        public void AddLoggerProvider(ILoggerProvider loggerProvider)
        {
            _loggers = _loggers.Append(loggerProvider).ToArray();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddLogging(option =>
                {
                    option.ClearProviders();
                    foreach (var provider in _loggers)
                    {
                        option.AddProvider(provider);
                    }
                    option.SetMinimumLevel(LogLevel.Trace);
                });
            });
        }
    }
}
