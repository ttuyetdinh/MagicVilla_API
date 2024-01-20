using MagicVilla_Web.Extension;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MagicVilla_Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // add a filter to controller to catch custom exception
            builder.Services.AddControllersWithViews(u => u.Filters.Add(new AuthExceptionRedirection()));

            builder.Services.AddAutoMapper(typeof(MappingConfig));

            // .AddHttpClient binding a IHttpClientFactory for a type
            builder.Services.AddHttpClient<IVillaServices, VillaServices>();
            builder.Services.AddScoped<IVillaServices, VillaServices>();

            builder.Services.AddHttpClient<IVillaNumberServices, VillaNumberServices>();
            builder.Services.AddScoped<IVillaNumberServices, VillaNumberServices>();

            builder.Services.AddHttpClient<IAuthService, AuthService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddScoped<ITokenProvider, TokenProvider>();
            builder.Services.AddScoped<IBaseServices, BaseServices>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IApiMessageRequestBuilder, ApiMessageRequestBuilder>();

            // add authentication
            builder.Services
                .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
                    options.SlidingExpiration = true;

                    options.AccessDeniedPath = "/Auth/AccessDeniedPath";
                    options.LoginPath = "/Auth/Login";
                });

            // add session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}