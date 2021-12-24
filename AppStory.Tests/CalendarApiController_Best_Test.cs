using AppStory.Authorizations;
using AppStory.Calendar;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace AppStory.Tests
{
    public class CalendarApiController_Best_Test : IClassFixture<AppStoryWebApplicationFactory<AppStory.WebUI.Startup>>
    {
        private readonly AppStoryWebApplicationFactory<AppStory.WebUI.Startup> _factory;

        public CalendarApiController_Best_Test(ITestOutputHelper output, AppStoryWebApplicationFactory<AppStory.WebUI.Startup> factory)
        {
            _factory = factory;
            _factory.AddLoggerProvider(new XUnitLogProvider(output));
        }

        [Fact]
        public async Task Now()
        {
            // Arrange
            var client = _factory.CreateClient();
            var url = "/api/calendar/now";

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var json = await response.Content.ReadAsStringAsync();

            GetNow.Response result = json.FromJson<GetNow.Response>();

            Assert.NotNull(result);

            Assert.True(result.Time.Date == DateTime.Now.Date);
        }

        [Fact]
        public async Task AuthSuccess()
        {
            var authService = _factory.Services.GetService<AuthService>();
            var jwt = authService.GenerateJwtToken(new JwtDto
            {
                Roles = new string[] { "Customer" },
            });

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + jwt);
            var url = "/api/products";

            var response = await client.GetAsync(url);

            Assert.True(response.IsSuccessStatusCode);
        }

        [Fact]
        public async Task AuthFail()
        {
            var authService = _factory.Services.GetService<AuthService>();
            var jwt = authService.GenerateJwtToken(new JwtDto
            {
                Roles = new string[] { "Customer1" },
            });

            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer " + jwt);
            var url = "/api/products";

            var response = await client.GetAsync(url);

            Assert.False(response.IsSuccessStatusCode);
        }
    }
}
