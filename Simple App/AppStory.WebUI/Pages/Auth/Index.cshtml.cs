using AppStory.Authorizations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AppStory.Pages.Auth
{
	public class IndexModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;
		private readonly AuthService _authService;

		public IndexModel(ILogger<IndexModel> logger,
			AuthService authService)
		{
			_logger = logger;
			_authService = authService;
		}

		public void OnGet()
		{ 
		}

		private void OnGetOld()
		{
			using var httpClient = new HttpClient();
			var url = @"http://localhost:9090/auth/realms/appstory/protocol/openid-connect/token";

			/*
			// 
			var person = new
			{
				client_id = "Appstory-front-client",
				username = "user1",
				password = "user1",
				grant_type = "password",
				client_secret = "ZYFFyV3wmRIAslXfHXflm4hJu7VVARv3",
			};

			var json = Newtonsoft.Json.JsonConvert.SerializeObject(person);
			_logger.LogDebug(json);


			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(url),
				Content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json"),
			};
			//request.Headers.Add("Content-Type", "application/json");

			var response = httpClient.Send(request);
			*/

			/*
			 curl --location --request POST 'http://127.0.0.1:8180/auth/realms/heroes/protocol/openid-connect/token' \
--header 'Content-Type: application/x-www-form-urlencoded' \
--data-urlencode 'client_id=admin-cli' \
--data-urlencode 'username=thor' \
--data-urlencode 'password=thor' \
--data-urlencode 'grant_type=password'
			*/

			var dict = new Dictionary<string, string>()
			{
				{ "client_id", "appstory-front-client"},
				{ "username", "user1"},
				{ "password", "user1"},
				{ "grant_type", "password"},
				{ "client_secret", "ZYFFyV3wmRIAslXfHXflm4hJu7VVARv3"},
			};

			var json = Newtonsoft.Json.JsonConvert.SerializeObject(dict);
			_logger.LogDebug(json);

			var request = new HttpRequestMessage
			{
				Method = HttpMethod.Post,
				RequestUri = new Uri(url),
				//Content = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded"),
				Content = new FormUrlEncodedContent(dict)
			};

			var response = httpClient.Send(request);

			_logger.LogDebug("IsSuccessStatusCode: " + response.IsSuccessStatusCode);

			var str = response.Content.ReadAsStringAsync().Result;

			_logger.LogDebug(JsonConvert.SerializeObject(str));
		}

		public void OnPost()
		{
		}
	}
}
