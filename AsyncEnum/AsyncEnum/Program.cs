var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSwaggerGen(swg =>
{
    /*swg.UseAllOfForInheritance();
    swg.SelectSubTypesUsing(baseType =>
    {
        return baseType.Assembly.GetTypes().Where(type => type.IsSubclassOf(baseType));
    });*/
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseEndpoints(conf =>
{
    conf.MapControllers();
});

app.Run();