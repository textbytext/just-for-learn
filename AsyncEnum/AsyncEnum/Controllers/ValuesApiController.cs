using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AsyncEnum.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ValuesApiController : ControllerBase
	{
		[HttpPost]
		[ProducesResponseType(type: typeof(Model2), statusCode: StatusCodes.Status200OK)]
		public Model2 GetModel([FromBody] Model2 model)
		{
			return model;
		}
	}

	public class Model1
	{
		public string value { get; set; }
	}

	public class Model2: Model1
	{
		public string value2 { get; set; }
	}
}
