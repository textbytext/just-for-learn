using AppStory;
using AppStory.Authorizations;
using AppStory.Calendar;
using AppStory.DataBase;
using AppStory.Infrastructure.Auth;
using AppStory.Middlewares;
using AppStory.Models;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var Configuration = builder.Configuration;

var services = builder.Services;

services.AddSingleton<IConfiguration>(Configuration);
services.AddRazorPages();

services.AddLogging();

services.AddDbContext<StoryDbContext>(option =>
{
	option.UseSqlite(Configuration.GetConnectionString(nameof(StoryDbContext)));
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
/*
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
*/

services.AddMediatR(typeof(CalendarApiController).Assembly);
services.AddScoped(typeof(IPipelineBehavior<,>), typeof(MediatorRequestTimer<,>));
services.AddScoped(typeof(IRequestPreProcessor<>), typeof(AuthMediatorPipeline<>));

services.AddHttpContextAccessor();

services.AddRazorPages(options =>
{
	options.Conventions.AuthorizePage("/Auth/Index");
	//options.Conventions.AuthorizeFolder("/Private");
	//options.Conventions.AllowAnonymousToPage("/Private/PublicPage");
	//options.Conventions.AllowAnonymousToFolder("/Private/PublicPages");
});


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

// // https://stackoverflow.com/questions/41721032/keycloak-client-for-asp-net-core

// // https://eksekki.medium.com/openid-connect-with-net-core-2-jboss-keycloak-3fd83c30564c

// https://github.com/dylanplecki/KeycloakOwinAuthentication/blob/master/samples/ASP.NET%20MVC%204.5%20Sample/Startup.cs

services.AddKeycloakWebApp(Configuration);

var app = builder.Build();

//app.UseMiddleware<ExceptionHandlerMiddleware>();

//app.UseMiddleware<AuthorizationMiddleware>();

var storyDbContext = app.Services.CreateScope().ServiceProvider.GetService<StoryDbContext>();
storyDbContext.Database.EnsureCreated();
storyDbContext.Database.Migrate();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

var options = new RewriteOptions()
	.AddRedirect("^/signin-oidc", "Auth/Index");
//app.UseRewriter(options);

app.UseRouting();

//app.UseSwagger();
//app.UseSwaggerUI();


//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapRazorPages();


app.Run();