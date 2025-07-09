using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net; // For HttpStatusCode

namespace WebApplication1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configure logging (as discussed previously)
            builder.Logging.AddConsole();
            builder.Logging.SetMinimumLevel(LogLevel.Information);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer(); // Required for Swagger
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "Restaurant.Api", Version = "v1" });
            });
       

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            // --- GLOBAL EXCEPTION HANDLING ---
            // This middleware catches unhandled exceptions thrown from anywhere deeper in the pipeline.
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                    var exception = exceptionHandlerPathFeature?.Error;

                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>(); // Get logger instance
                    logger.LogError(exception, "An unhandled exception occurred during request processing. Path: {Path}", context.Request.Path);

                    context.Response.ContentType = "application/json";

                    if (app.Environment.IsDevelopment())
                    {
                        // In development, return more detailed error info for debugging
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Status = (int)HttpStatusCode.InternalServerError,
                            Message = "An unexpected error occurred.",
                            Detailed = exception?.Message
                        });
                    }
                    else
                    {
                        // In production, return a generic error message
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await context.Response.WriteAsJsonAsync(new
                        {
                            Status = (int)HttpStatusCode.InternalServerError,
                            Message = "An unexpected error occurred. Please try again later."
                        });
                    }
                });
            });
            // --- END GLOBAL EXCEPTION HANDLING ---


            
                    app.UseSwagger();                        // Enable middleware
                    app.UseSwaggerUI();                      // Enable Swagger UI
                    

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();

        }
    }
}
