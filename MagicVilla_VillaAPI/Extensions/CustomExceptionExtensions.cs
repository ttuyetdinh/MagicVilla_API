using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;

namespace MagicVilla_VillaAPI.Extensions
{
    public static class CustomExceptionExtensions
    {
        public static void HandleError(this IApplicationBuilder app, bool isDevelopment)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerFeature>();

                    if (feature != null)
                    {
                        if (isDevelopment)
                        {
                            await context.Response.WriteAsync(
                                JsonConvert.SerializeObject(new
                                {
                                    StatusCode = context.Response.StatusCode,
                                    ErrorMessage = feature.Error.Message,
                                    StackTrace = feature.Error.StackTrace
                                })
                            );
                        }
                        else
                        {
                            await context.Response.WriteAsync(
                                JsonConvert.SerializeObject(new
                                {
                                    StatusCode = context.Response.StatusCode,
                                    ErrorMessage = "Hello from exception extension",
                                })
                            );
                        }
                    }
                });
            });
        }
    }
}