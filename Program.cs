using DatingApi.Data;
using DatingApi.Repository;
using DatingApi.Repository.IRepository;
using DatingApi.Service;
using DatingApi.Service.IService;
using DatingApi.Utility;
using DatingApi.Utility.Middleware;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuration for the Database Application
builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultSqlConnection")));

//configuration for the Cors
builder.Services.AddCors();

//DI of Iunit of Work
builder.Services.AddScoped<IUOWService,UOWService>();
builder.Services.AddScoped<IJwtToken, JwtToken>();

var app = builder.Build();

//adding you exceptionMiddleware
app.UseMiddleware<ExceptionMiddelware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()||app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Adding the middelware of cors
app.UseCors(builder=>builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200"));

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
