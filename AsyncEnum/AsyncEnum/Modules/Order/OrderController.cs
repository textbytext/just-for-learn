
using Microsoft.AspNetCore.Mvc;

namespace AsyncEnum.Order
{
	[Route("api/order")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		[HttpPost("create")]
		public async IAsyncEnumerable<OrderStateDto> CreateOrder()
		{
			var orderState = new OrderStateDto
			{
				Id = Guid.NewGuid().ToString(),
				Time = DateTime.Now,
			};

			await Task.Delay(1000);
			orderState.Message = $"Order creating ...";
			yield return orderState;

			await Task.Delay(2000);
			orderState.Message = $"Order created!";
			orderState.Time = DateTime.Now;
			yield return orderState;

			await Task.Delay(2000);
			orderState.Message = $"Order sending!";
			orderState.Time = DateTime.Now;
			yield return orderState;

			await Task.Delay(2000);
			orderState.Message = $"Order sent!";
			orderState.Time = DateTime.Now;
			yield return orderState;
		}
	}
}
