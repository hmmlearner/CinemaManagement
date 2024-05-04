global using Microsoft.EntityFrameworkCore;
using CinemaManagement.Data;
using CinemaManagement.Filters;
using CinemaManagement.Interfaces;
using CinemaManagement.Services;
using Microsoft.OpenApi.Models;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddDbContext<CinemaContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("CinemaConnection")));

builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheaterRepository, TheaterRepository>();
builder.Services.AddScoped<IShowTimeRepository, ShowTimeRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ModelStateValidationFilter>();

builder.Services.AddControllers(options =>
{
    options.Filters.Add<ModelStateValidationFilter>();
});

builder.Services.AddEndpointsApiExplorer();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API",
        Version = "v1",
        Description = "Cinema Booking System APIs",

    });

    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
