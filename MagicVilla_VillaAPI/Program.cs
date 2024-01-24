using System.Text;
using MagicVilla_VillaAPI;
using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Extensions;
using MagicVilla_VillaAPI.Filters;
using MagicVilla_VillaAPI.MiddleWares;
using MagicVilla_VillaAPI.Model;
using MagicVilla_VillaAPI.Repository;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// set up ASP.NET Core Identity with custom class
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>();

// adding the response cache to the exist controller
builder.Services.AddResponseCaching();

builder.Services.AddControllers(option =>
{
    option.CacheProfiles.Add("Default30sec",
        new CacheProfile
        {
            Duration = 30
        }
    );

    // register custom error filter
    option.Filters.Add<CustomExceptionFilter>();
}).AddNewtonsoftJson()
// congif this to use custom error link 
.ConfigureApiBehaviorOptions(option =>{
    option.ClientErrorMapping[StatusCodes.Status500InternalServerError] = new ClientErrorData(){
        Link = "https://www.youtube.com/"
    };
});

builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAutoMapper(typeof(MappingConfig));

// add api versioning
builder.Services.AddApiVersioning(options =>
    {
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.ReportApiVersions = true;
    }
);
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services
    .AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(x =>
    {
        x.RequireHttpsMetadata = false;
        x.SaveToken = true;
        x.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("APISettings:SecretKey"))),
            // use 4 below lines to setting up issuer and audience
            ValidateIssuer = false,
            ValidateAudience = false,
            // ValidIssuer = "PhucNguyen",
            // ValidAudiences = new List<string>() { "PhucNguyen", "BaoThu" },

            ClockSkew = TimeSpan.Zero
        };
    });

// create an instance of LoggerConfiguration class belong to Serilog
// Log.Logger = new LoggerConfiguration()
//                 .MinimumLevel
//                 .Debug()
//                 .WriteTo.File("log/villalogs.txt", rollingInterval: RollingInterval.Day)
//                 .CreateLogger();
// builder.Host.UseSerilog();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSingleton<IConfigureOptions<SwaggerGenOptions>,ConfigurationSwaggerGenOptions>();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v2/swagger.json", "Magic Villa v2");
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Magic Villa v1");
    }
    );
}

// register an error handler controller in the pipeline using the default extension method provied by asp.net
// app.UseExceptionHandler("/ErrorHandling/ProcessError");

// using the custom extension method created by your own
// app.HandleError(app.Environment.IsDevelopment());

// register a custom middleware to handle the exception
app.UseMiddleware<CustomExceptionMiddleware>();

app.UseResponseCaching();

app.UseStaticFiles();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// using this method to auto-update db if migrations is exsiting
MigrationConfiguration.ApplyMigration(app);

app.Run();
