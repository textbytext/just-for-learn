using AppStory.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory.Calendar
{
    using TRequest = GetException;
    using TResponse = GetNow.Response;

    public class GetException : IRequest<TResponse>
    {
        public class Handler : IRequestHandler<TRequest, TResponse>
        {
            private readonly IMediator _mediator;

            public Handler(IMediator mediator)
            {
                _mediator = mediator;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
            {
                var now = DateTime.Now;

                var tomorrow = await _mediator.Send(new GetTomorrow
                {
                    Time = now,
                });

                _ = await _mediator.Send(new GetYesterday
                {
                    Time = tomorrow.Time,
                });

                throw new AppStoryException("Exception sample.");
            }
        }

        public class Response : TimeDto
        {
        }
    }
}
