using AppointmentIngestion.Services.Extensions;
using AppointmentIngestion.Services.Mapping;
using Serilog;

namespace AppointmentIngestion.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Host.UseSerilog();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services
                .AddServicesLayer()
                .AddAutoMapper(typeof(AppointmentProfile).Assembly);

            var corsPolicyName = "FrontendPolicy";
            // CORS for your React app on http://localhost:5173
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(corsPolicyName, policy =>
                {
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowCredentials();
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();
            app.UseCors(corsPolicyName);
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
