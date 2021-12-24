using AppStory.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AppStory.Middlewares
{
	public class ExceptionHandlerMiddleware
	{
		private readonly IWebHostEnvironment _env;
		private readonly RequestDelegate _next;

		public ExceptionHandlerMiddleware(IWebHostEnvironment env,
			RequestDelegate next)
		{
			_env = env;
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception e)
			{
				await ProcessError(context, e);
			}
		}

		private async Task ProcessError(HttpContext context, Exception e)
		{
			var response = GetProblemDetails(context, e);

			var json = Newtonsoft.Json.JsonConvert.SerializeObject(response);

			context.Response.Clear();

			context.Response.StatusCode = response.Status ?? StatusCodes.Status500InternalServerError;
			context.Response.ContentType = MediaTypeNames.Application.Json;
			await context.Response.Body.WriteAsync(Encoding.UTF8.GetBytes(json));
		}

		[MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
		private ApiExceptionResultDto GetProblemDetails(HttpContext context, Exception e)
		{
			var statusCode = StatusCodes.Status500InternalServerError;
			var title = "Internal Error";
			if (e.GetType() == typeof(UnauthorizedAccessException))
			{
				statusCode = StatusCodes.Status401Unauthorized;
				title = "Unauthorized access";
			}

			var result = new ApiExceptionResultDto
			{
				Type = "about:blank",
				Status = statusCode,
				Title = title,
				Detail = e.Message,
				Instance = context.Request.Path,
			};

			result.Extensions[nameof(context.TraceIdentifier)] = context.TraceIdentifier;

			// if dev only!!!
			result.Extensions[nameof(e.StackTrace)] = e.StackTrace;

			return result;
		}
	}
}
