
using Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using App.Services;

namespace app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            DotNetEnv.Env.Load();

            string connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");


            builder.Services.AddDbContext<DBContextTechEmporiumTrend>(
                options=>options.UseSqlServer(connectionString)

                );


            // Add services to the container.

            builder.Services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<FakeStoreService>();
            // Agregar servicios para FakeStoreService con HttpClient
            builder.Services.AddHttpClient<FakeStoreService>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
