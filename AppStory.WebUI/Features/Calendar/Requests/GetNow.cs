using AppStory.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory.Calendar
{
    using TRequest = GetNow;
    using TResponse = GetNow.Response;

    public class GetNow : IRequest<TResponse>
    {
        public class Handler : IRequestHandler<TRequest, TResponse>
        {
            private readonly IMediator _mediator;
            private readonly ILogger _logger;

            public Handler(
                IMediator mediator,
                ILogger<TRequest> logger)
            {
                _mediator = mediator;
                _logger = logger;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
            {
                var now = DateTime.Now;

                _logger.LogDebug($"now: {now}");

                var tomorrow = await _mediator.Send(new GetTomorrow
                {
                    Time = now,
                });

                _logger.LogDebug($"tomorrow: {tomorrow.Time}");

                var yesterday = await _mediator.Send(new GetYesterday
                {
                    Time = tomorrow.Time,
                });

                _logger.LogDebug($"yesterday: {yesterday.Time}");

                return new TResponse
                {
                    Time = yesterday.Time,
                };
            }
        }

        public class Response : TimeDto
        {
        }
    }
}
