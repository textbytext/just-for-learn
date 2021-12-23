using AppStory.Models;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory.Calendar
{
    using TRequest = GetYesterday;
    using TResponse = GetYesterday.Response;

    public class GetYesterday : TimeDto, IRequest<TResponse>
    {
        public class Handler : IRequestHandler<TRequest, TResponse>
        {
            public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
            {
                var result = new TResponse
                {
                    Time = request.Time.AddDays(-1),
                };

                return Task.FromResult(result);
            }
        }

        public class Response : TimeDto
        {
        }
    }
}
