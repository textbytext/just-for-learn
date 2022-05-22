using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory
{
	public class MediatorRequestTimer<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly MediatrRequestTimerInfo _mediatrRequestTimerInfo;
        private readonly ILogger _logger;

        public MediatorRequestTimer(
            MediatrRequestTimerInfo mediatrRequestTimerInfo,
            ILogger<MediatorRequestTimer<TRequest, TResponse>> logger)
        {
            _mediatrRequestTimerInfo = mediatrRequestTimerInfo;
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            if (_mediatrRequestTimerInfo.FirstRequestReference is null)
            {
                _mediatrRequestTimerInfo.FirstRequestReference = request;
                _mediatrRequestTimerInfo.Timer = Stopwatch.StartNew();
            }

            _mediatrRequestTimerInfo.RequestChain.Add(request.GetType().Name);

            try
            {
                return await next();
            }
            finally
            {
                if (object.ReferenceEquals(_mediatrRequestTimerInfo.FirstRequestReference, request))
                {
                    _logger.LogDebug($"Request ${typeof(TRequest).Name}. Time: {_mediatrRequestTimerInfo.Timer.Elapsed}. Stack: {string.Join("->", _mediatrRequestTimerInfo.RequestChain)}");
                }
            }
        }
    }

    public class MediatrRequestTimerInfo
    {
        public object FirstRequestReference { get; set; }
        public Stopwatch Timer { get; set; }
        public List<string> RequestChain { get; set; } = new List<string>();
    }
}
