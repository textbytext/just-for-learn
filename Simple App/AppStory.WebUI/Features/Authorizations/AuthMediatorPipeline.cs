using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory.Authorizations
{
	public class AuthMediatorPipeline<TRequest> : IRequestPreProcessor<TRequest>
	{
		private readonly AuthService _policyService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly ILogger<AuthMediatorPipeline<TRequest>> _logger;

		public AuthMediatorPipeline(
			AuthService policyService,
			IHttpContextAccessor httpContextAccessor,
			ILogger<AuthMediatorPipeline<TRequest>> logger)
		{
			_policyService = policyService;
			_httpContextAccessor = httpContextAccessor;
			_logger = logger;
		}

		public Task Process(TRequest request, CancellationToken cancellationToken)
		{
			var requestName = request.GetType().Name;
			var policyName = request.GetType().Name;

			var roles = _policyService.GetRolesByPolicyName(policyName);

			if (roles is null || roles.Length == 0)
			{
				return Task.CompletedTask;
			}

			var unauthorizedAccessMessage = $"Unauthorized access to {requestName}!";

			var user = _httpContextAccessor.HttpContext.User;
			if (user.Identity is null)
			{
				throw new UnauthorizedAccessException(unauthorizedAccessMessage);
			}

			var hasRole = roles.Any(r => user.IsInRole(r));
			if (!hasRole)
			{
				throw new UnauthorizedAccessException(unauthorizedAccessMessage);
			}

			return Task.CompletedTask;
		}
	}
}
