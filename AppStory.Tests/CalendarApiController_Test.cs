using AppStory.Calendar;
using AppStory.DataBase;
using AppStory.Features.Products;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppStory.Tests
{
    public class CalendarApiController_Test : BaseTest
    {
        private readonly ITestOutputHelper _output;

        public CalendarApiController_Test(ITestOutputHelper output)
        {
            _output = output;
            Init();
        }

        protected override void ConfigureServices(IServiceCollection services)
        {
            var Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Test.json")
                .Build();

            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMediatR(typeof(CalendarApiController).Assembly);
            services.AddScoped<CalendarApiController>();
            services.AddScoped<ProductsApiController>();            
            services.AddScoped<IHttpContextAccessor, HttpContextAccessor>(sp =>
            {
               return new HttpContextAccessor
               {
                   HttpContext = new DefaultHttpContext(),
               };
            });
            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddProvider(new XUnitLogProvider(_output));
                builder.SetMinimumLevel(LogLevel.Trace);
            });

            services.AddDbContext<StoryDbContext>(option =>
            {
                //option.UseInMemoryDatabase(Guid.NewGuid().ToString());
                option.UseSqlServer(Configuration.GetConnectionString(nameof(StoryDbContext)));
            });
            services.AddScoped<IOrdersDbContext>(sp => sp.GetService<StoryDbContext>());
            services.AddScoped<IProductsDbContext>(sp => sp.GetService<StoryDbContext>());
        }

        [Fact]
        public async Task Now()
        {
            var controller = this.GetService<CalendarApiController>();

            var okResult = await controller.Now() as OkObjectResult;

            Assert.NotNull(okResult);

            var response = okResult.Value as GetNow.Response;

            Assert.NotNull(response);

            var delta = DateTime.Now - response.Time;

            Assert.True(delta.Seconds < 3); // almost now ...
        }

        [Fact]
        public void DeleteProducts()
        {
            var logger = GetService<ILogger<CalendarApiController_Test>>();

            var context = this.GetService<StoryDbContext>();

            var count = context.Products.Count();
            logger.LogDebug($"Count of product: {count}");


            using var transaction = context.Database.BeginTransaction();
            try
            {
                context.Products.RemoveRange(context.Products);
                context.SaveChanges();

                Assert.True(context.Products.Count() == 0);
            }
            finally
            {
                transaction.Rollback();
            }

            Assert.True(context.Products.Count() > 0);
            Assert.True(context.Products.Count() == count);
        }

        [Fact]
        public async Task GetAllProducts()
        {
            var controller = this.GetService<ProductsApiController>();
            var products = await controller.GetAllProducts();
            var result = (products as OkObjectResult).Value as GetAllProducts.Result;
            Assert.True(result.Products.Length > 0);
        }
    }
}
