using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppStory.Tests
{
    public class BaseTest
    {
        private IServiceProvider _serviceProvider;

        protected void Init()
        {
            var services = new ServiceCollection();

            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();
        }

        protected virtual void ConfigureServices(IServiceCollection services)
        { 
        }

        private IServiceScope _scope;
        protected T GetService<T>()
        {
            _scope ??= _serviceProvider.CreateScope();
            
            return _scope.ServiceProvider.GetService<T>();
        }
    }
}
