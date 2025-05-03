using APIHub.Interfaces;
using APIHubCore.Interfaces;
using APIHubCore.Services;
using APIHubRepository.Repositories;
using Hangfire;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog.Extensions.Logging;
using NLog.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSingleton<OtpService>(); // 

var config = new ConfigurationBuilder()
    .SetBasePath(System.IO.Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .Build();
NLog.LogManager.Configuration = new NLogLoggingConfiguration(config.GetSection("NLog"));

builder.Logging.ClearProviders();

builder.Host.UseNLog();


builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = config["Redis:Url"];
    options.InstanceName = "RedisCacheInstance";
});

// Register the RedisCacheService for DI
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();


// Configure Hangfire with SQL Server storage
var hangfireConnectionString = config.GetConnectionString("HangFireConnString");
builder.Services.AddHangfire(configuration => configuration.UseSqlServerStorage(hangfireConnectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IUserRepo, UserRepository>();
builder.Services.AddScoped<ILoggerService, LoggerService>();
builder.Services.AddSingleton<WindowsAuthService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

// Start Hangfire server
app.UseHangfireServer();

app.UseHangfireDashboard();


app.Run();
