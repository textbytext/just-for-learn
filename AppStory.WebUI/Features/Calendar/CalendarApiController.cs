using AppStory.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading.Tasks;

namespace AppStory.Calendar
{
    [Route("api/calendar")]
    [ApiController]
    [AllowAnonymous]
    public class CalendarApiController : ControllerBase
    {
        private readonly ISender _mediator;

        public CalendarApiController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("now")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(type: typeof(GetNow.Response), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> Now()
        {
            var cmd = new GetNow();

            return new OkObjectResult(await _mediator.Send(cmd));
        }

        [HttpPost]
        [Route("yesterday")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(type: typeof(GetYesterday.Response), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> Yesterday([FromBody, Required] GetYesterday cmd)
        {
            return new OkObjectResult(await _mediator.Send(cmd));
        }

        [HttpPost]
        [Route("tomorrow")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(type: typeof(GetTomorrow.Response), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> Tomorrow([FromBody, Required] GetTomorrow cmd)
        {
            return new OkObjectResult(await _mediator.Send(cmd));
        }

        [HttpPost]
        [Route("exception")]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(type: typeof(GetException.Response), statusCode: StatusCodes.Status200OK)]
        public async Task<IActionResult> Exception([FromBody, Required] GetException cmd)
        {
            return new OkObjectResult(await _mediator.Send(cmd));
        }
    }
}
