using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory
{
    public class MediatorRequestLogger<T> : IRequestPreProcessor<T>
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MediatorRequestLogger(
            IMediator mediator,
            ILogger<MediatorRequestLogger<T>> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public Task Process(T request, CancellationToken cancellationToken)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(request,
                new JsonSerializerSettings
                {
                    Formatting = Formatting.None,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                });

            _logger.LogDebug($@"{typeof(T).Name}
Request ID: {_httpContextAccessor.HttpContext.TraceIdentifier}
result: {json}");

            return Task.CompletedTask;
        }
    }
}
