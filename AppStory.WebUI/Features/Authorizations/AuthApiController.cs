using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppStory.Authorizations
{
	[Route("api/auth")]
	[ApiController]
	public class AuthApiController : ControllerBase
	{
		private readonly AuthService _policyService;

		public AuthApiController(AuthService policyService)
		{
			_policyService = policyService;
		}

		[HttpPost]
		[Route("GenerateToken")]
		public string GenerateJwtToken(JwtDto data)
		{
			return _policyService.GenerateJwtToken(data);
		}

		[HttpGet]
		[Route("DecodeToken")]
		public JwtDto DecodeJwtToken(string token)
		{
			return _policyService.DecodeJwtToken(token);
		}
	}
}
