using AppStory.Authorizations;
using AppStory.DataBase;
using AppStory.Middlewares;
using AppStory.Models;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AppStory.WebUI
{
	public class Startup
	{
		private readonly IWebHostEnvironment _env;

		public Startup(IConfiguration configuration,
			IWebHostEnvironment env)
		{
			Configuration = configuration;
			_env = env;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddLogging();

			services.AddDbContext<StoryDbContext>(option =>
			{
				option.UseSqlServer(Configuration.GetConnectionString(nameof(StoryDbContext)));
			});
			services.AddScoped<IOrdersDbContext>(sp => sp.GetService<StoryDbContext>());
			services.AddScoped<IProductsDbContext>(sp => sp.GetService<StoryDbContext>());

			/*services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddConsole();
                builder.SetMinimumLevel(LogLevel.Debug);
            });*/

			services.AddScoped<ITimeService, TimeService>();
			services.AddScoped<MediatrRequestTimerInfo>();
			services.AddSingleton<AuthService>();

			services.AddControllers();

			//services.AddSwaggerGen();
			services.AddSwaggerGen(options =>
			{
				//options.SwaggerDoc("v1", new OpenApiInfo { Title = "API WSVAP (WebSmartView)", Version = "v1" });
				//options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
				//options.CustomSchemaIds(type => Guid.NewGuid().ToString());// type.ToString());
				options.CustomSchemaIds(type => type.FullName);

				var securitySchema = new OpenApiSecurityScheme
				{
					Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
					Name = "Authorization",
					In = ParameterLocation.Header,
					Type = SecuritySchemeType.Http,
					Scheme = "bearer",
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id = "Bearer"
					}
				};

				options.AddSecurityDefinition("Bearer", securitySchema);

				var securityRequirement = new OpenApiSecurityRequirement
				{
					{ securitySchema, new[] { "Bearer" } }
				};

				options.AddSecurityRequirement(securityRequirement);
			});

			services.AddMediatR(this.GetType().Assembly);
			services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MediatorRequestTimer<,>));
			services.AddScoped(typeof(IRequestPreProcessor<>), typeof(AuthMediatorPipeline<>));

			services.AddHttpContextAccessor();

			services.AddMvcCore(options =>
			{
				options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError)
				{
					Type = typeof(ApiExceptionResultDto),
				});
				options.Filters.Add(new ProducesResponseTypeAttribute(StatusCodes.Status401Unauthorized)
				{
					Type = typeof(ApiExceptionResultDto),
				});
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, StoryDbContext storyDbContext)
		{
			Console.WriteLine($"EnvironmentName: {env.EnvironmentName}");

			app.UseMiddleware<ExceptionHandlerMiddleware>();
			app.UseMiddleware<AuthorizationMiddleware>();

			storyDbContext.Database.Migrate();

			storyDbContext.Database.EnsureCreated();
			StoryDbSeed.Seed(storyDbContext);

			if (env.IsDevelopment())
			{
				//app.UseDeveloperExceptionPage();
			}
			else
			{
				//app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
			app.UseSwaggerUI();

			//app.UseAuthorization();


			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
