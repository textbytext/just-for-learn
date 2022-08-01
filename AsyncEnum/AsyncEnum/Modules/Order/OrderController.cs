
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsyncEnum.Order
{
	[Route("api/order")]
	[ApiController]
	public class OrderController : ControllerBase
	{
		[HttpPost("create")]
		public IAsyncEnumerable<OrderStateDto> PostCreateOrder()
		{
			return ProcessOrder();
		}

		[HttpGet("create")]
		public IAsyncEnumerable<OrderStateDto> GetCreateOrder()
		{
			return ProcessOrder();
		}

		private async IAsyncEnumerable<OrderStateDto> ProcessOrder()
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
			orderState.Message = $"Order sending ...";
			orderState.Time = DateTime.Now;
			yield return orderState;

			await Task.Delay(2000);
			orderState.Message = $"Order sent!";
			orderState.Time = DateTime.Now;
			yield return orderState;
		}
	}
}
