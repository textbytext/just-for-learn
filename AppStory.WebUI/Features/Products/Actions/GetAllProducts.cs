using AppStory.DataBase;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace AppStory.Features.Products
{
    using TRequest = GetAllProducts;
    using TResponse = GetAllProducts.Result;

    public class GetAllProducts : IRequest<TResponse>
    {
        public class Handler : IRequestHandler<TRequest, TResponse>
        {
            private readonly IProductsDbContext _context;

            public Handler(IProductsDbContext context)
            {
                _context = context;
            }

            public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
            {
                var items = await _context.Products.AsNoTracking().ToArrayAsync();

                return new TResponse
                {
                    Products = items,
                };
            }
        }

        public class Result
        {
            public Product[] Products { get; set; }
        }
    }
}
