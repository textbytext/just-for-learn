using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AppStory.Authorizations
{
	public class AuthorizationMiddleware
	{
		private readonly IWebHostEnvironment _env;
		private readonly RequestDelegate _next;
		private readonly AuthService _authService;
		private readonly ILogger<AuthorizationMiddleware> _logger;

		public AuthorizationMiddleware(
			IWebHostEnvironment env,
			RequestDelegate next,
			AuthService authService,
			ILogger<AuthorizationMiddleware> logger)
		{
			_env = env;
			_next = next;
			_authService = authService;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			var authorization = context.Request.Headers[HeaderNames.Authorization];

			_logger.LogDebug(authorization);

			if (authorization.Count > 0)
			{
				var bearer = authorization.First();
				_logger.LogDebug(bearer);

				var jwt = bearer.Split(' ').Last();
				
				var dto = _authService.DecodeJwtToken(jwt);
				if (dto is not null)
				{
					var identity = new ClaimsIdentity(dto.Roles
						.Select(role => new Claim(ClaimTypes.Role, role))
						.ToArray());

					context.User = new ClaimsPrincipal(identity);
					_logger.LogDebug("User Authorized!");
				}
			}

			await _next(context);
		}
	}
}
