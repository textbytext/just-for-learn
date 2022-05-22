using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AppStory.Features.Products
{
    [Route("api/products")]
    [ApiController]
    public class ProductsApiController : ControllerBase
    {
        private readonly ISender _mediator;

        public ProductsApiController(ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(type: typeof(GetAllProducts.Result), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProducts() => Ok(await _mediator.Send(new GetAllProducts()));
    }
}
