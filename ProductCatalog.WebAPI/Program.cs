using Asp.Versioning;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Interfaces;
using ProductCatalog.Application.Services;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Infrastructure.Data;
using ProductCatalog.Infrastructure.Repositories;
using ProductCatalog.WebAPI.Middlewares;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// --- SETUP SERILOG ---
builder.Host.UseSerilog((context, configuration) =>
{
    configuration
        .MinimumLevel.Information() 
        .WriteTo.Console() 
        .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day);
});

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true; 
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(), 
        new HeaderApiVersionReader("X-Api-Version")); 
})
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddValidatorsFromAssembly(typeof(ProductCatalog.Application.Validators.Products.CreateProductRequestValidator).Assembly);
builder.Services.AddValidatorsFromAssembly(typeof(ProductCatalog.Application.Validators.Products.UpdateProductRequestValidator).Assembly);

//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("SuperAdminOnly", policy => policy.RequireRole("SuperAdmin"));
//    options.AddPolicy("AdminAccess", policy => policy.RequireRole("SuperAdmin", "Admin"));
//});
var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();