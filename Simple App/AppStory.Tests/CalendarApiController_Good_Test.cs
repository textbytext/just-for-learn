using AppStory.Calendar;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Threading.Tasks;
using Xunit;

namespace AppStory.Tests
{
    public class CalendarApiController_Good_Test : IClassFixture<WebApplicationFactory<AppStory.WebUI.Startup>>
    {
        WebApplicationFactory<AppStory.WebUI.Startup> _factory;

        public CalendarApiController_Good_Test(WebApplicationFactory<AppStory.WebUI.Startup> factory)
        {
            _factory = factory;
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
    }    
}
