using MagicVilla_VillaAPI.Logging;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddNewtonsoftJson();

// create an instance of LoggerConfiguration class belong to Serilog
// Log.Logger = new LoggerConfiguration()
//                 .MinimumLevel
//                 .Debug()
//                 .WriteTo.File("log/villalogs.txt", rollingInterval: RollingInterval.Day)
//                 .CreateLogger();
// builder.Host.UseSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ILogging,Logging>();

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
