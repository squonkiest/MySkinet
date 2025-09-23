using Microsoft.EntityFrameworkCore;
using Skinet.API.Middlewares;
using Skinet.Core.Interfaces;
using Skinet.Infrastructure.Data;
using Skinet.Infrastructure.Services;
using StackExchange.Redis;

namespace Skinet.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddDbContext<StoreContext>(opt =>
            {
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
            });

            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddCors();
            builder.Services.AddSingleton<IConnectionMultiplexer>(config =>
            {
                string connString = builder.Configuration.GetConnectionString("Redis")
                    ?? throw new Exception("Cannot get redis connection string");
                var configuration = ConfigurationOptions.Parse(connString);
                return ConnectionMultiplexer.Connect(configuration);
            });
            builder.Services.AddSingleton<ICartService, CartService>();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseCors(opt => opt.AllowAnyHeader().AllowAnyMethod()
                        .WithOrigins("https://localhost:4200", "http://localhost:4200"));
            app.MapControllers();

            try
            {
                using var scope = app.Services.CreateScope();
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<StoreContext>();
                await context.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(context);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            app.Run();
        }
    }
}
