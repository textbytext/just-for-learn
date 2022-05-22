using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace AppStory.Infrastructure.Auth;

public static class KeycloakExtensions
{
	public static IServiceCollection AddKeycloakWebApp(this IServiceCollection services,
		IConfiguration configuration)
	{
		var authority = configuration["Authentication:oidc:Authority"];
		var clientId = configuration["Authentication:oidc:ClientId"];
		var clientSecret = configuration["Authentication:oidc:ClientSecret"];

		services.AddAuthentication(options =>
		{
			// Store the session to cookies
			options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
			// OpenId authentication
			options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
		})
		.AddCookie("Cookies")
		.AddOpenIdConnect(options =>
		{			
			// URL of the Keycloak server
			options.Authority = authority;
			// Client configured in the Keycloak
			options.ClientId = clientId;

			// For testing we disable https (should be true for production)
			options.RequireHttpsMetadata = false;
			options.SaveTokens = true;

			// Client secret shared with Keycloak
			options.ClientSecret = clientSecret;
			options.GetClaimsFromUserInfoEndpoint = true;

			// OpenID flow to use
			options.ResponseType = OpenIdConnectResponseType.CodeIdToken;
			//options.ResponseType = OpenIdConnectResponseType.Token;
		});

		return services;
	}
}