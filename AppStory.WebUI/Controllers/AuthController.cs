using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AppStory.Controllers
{
	[AllowAnonymous]
	public class AuthController : Controller
	{
		private readonly ILogger<AuthController> _logger;
		private readonly IConfiguration _configuration;

		public AuthController(ILogger<AuthController> logger,
			IConfiguration configuration)
		{
			_logger = logger;
			_configuration = configuration;
		}

		[HttpPost]
		[Route("/signin-oidc")]
		public IActionResult SigninOidc([FromForm] KeycloakSigninOidcDto dto)
		{
			_logger.LogDebug(dto.ToJsonIndent());

			var clientSecret = "932feb69-bc83-47c5-83c8-003f52409310";

			var data = new JwtBuilder()
				.WithSecret(clientSecret)
				.WithAlgorithm(new HMACSHA256Algorithm())				
				//.MustVerifySignature()
				.Decode<Dictionary<string, string>>(dto.id_token);

			_logger.LogDebug(data.ToJsonIndent());

			return new EmptyResult(); // Redirect("/");
		}
	}

	public class KeycloakSigninOidcDto
	{
		[JsonProperty("code")]
		public string code { get; set; }

		[JsonProperty("id_token")]
		public string id_token { get; set; }

		[JsonProperty("state")]
		public string state { get; set; }

		[JsonProperty("session_state")]
		public string session_state { get; set; }
	}
}
