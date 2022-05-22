/*using AppStory.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace AppStory.Controllers
{
    //[Route("api/oldtime")]
    //[ApiController]
    //[AllowAnonymous]
    public class OldTimeApiController // : ControllerBase
    {
        private readonly ITimeService _oldService;
        private readonly IConfiguration _configuration;

        public OldTimeApiController(ITimeService oldService,
            IConfiguration configuration)
        {
            _oldService = oldService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("now")]
        //[ProducesResponseType(type: typeof(TimeDto), statusCode: StatusCodes.Status200OK)]
        public IActionResult Now()
        {
            var result = _oldService.Now();

            return Ok(result);
        }

        [HttpGet]
        [Route("now2")]
        //[ProducesResponseType(type: typeof(TimeDto), statusCode: StatusCodes.Status200OK)]
        public ActionResult<TimeDto> Now2()
        {
            var result = _oldService.Now();

            return result;
        }

        [HttpPost]
        [Route("yesterday")]
        [ProducesResponseType(type: typeof(TimeDto), statusCode: StatusCodes.Status200OK)]
        public IActionResult Yesterday([FromBody] TimeDto dto)
        {
            var result = _oldService.Yesterday(dto.Time);

            return Ok(result);
        }

        [HttpPost]
        [Route("tomorrow")]
        [ProducesResponseType(type: typeof(TimeDto), statusCode: StatusCodes.Status200OK)]
        public IActionResult Tomorrow([FromBody] TimeDto dto)
        {
            var result = _oldService.Tomorrow(dto.Time);

            return Ok(result);
        }

        [HttpGet]
        [Route("reload")]
        public IActionResult Reload()
        {
            var root = (IConfigurationRoot)_configuration;
            root.Reload();

            return Ok();
        }
    }
}
*/