using Scalar.AspNetCore;
using Serilog;
using src.Features.User;
using src.Features.User.Interfaces;
using src.Middleware;

namespace src;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .CreateBootstrapLogger();

        try
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            
            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            
            builder.Host.UseSerilog((context, configuration) => 
                configuration.ReadFrom.Configuration(context.Configuration));
            
            builder.Services.AddOpenApi();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }
            
            app.UseHttpsRedirection();
            app.UseAuthorization();
            
            app.MapControllers();
            app.Run();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Application terminated unexpectedly");
        }
        finally
        {
            Console.WriteLine("\nClosing...");
            Log.CloseAndFlush();
        }
    }
}