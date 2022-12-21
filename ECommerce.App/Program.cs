using ECommerce.App.Data;
using ECommerce.App.Helpers.Interfaces;
using ECommerce.App.Helpers.Repositories;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Application.Interfaces;
using ECommerce.Common.DataBase;
using ECommerce.Common.SExplMappers;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Vereyon.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddRazorRuntimeCompilation();

builder.Services.AddDbContext<ECommerceDbContext>(o =>
{
    o.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddAutoMapper(typeof(SpExplorationMapper));
builder.Services.AddTransient<SeedDb>();
builder.Services.AddApplication();
builder.Services.AddFlashMessage();

//Register dapper in scope    
builder.Services.AddScoped<IDapperRepository, DapperRepository>();
builder.Services.AddScoped<ICombosHelper, CombosHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelper>();

// Set the JSON serializer options
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = null;
    options.SerializerOptions.WriteIndented = true;
});
var app = builder.Build();

SeedData();

void SeedData()
{
    IServiceScopeFactory? scopedFactory = app.Services.GetService<IServiceScopeFactory>();

    using (IServiceScope? scope = scopedFactory.CreateScope())
    {
        SeedDb? service = scope.ServiceProvider.GetService<SeedDb>();
        service.SeedAsync().Wait();
    }
}
var supportedCultures = new[] { "en-US", "es-MX" };
var localizationOptions = new RequestLocalizationOptions().SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
