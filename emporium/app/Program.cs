
<<<<<<< Updated upstream
=======
using Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

>>>>>>> Stashed changes
namespace app
{
    public class Program
    {

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

<<<<<<< Updated upstream
=======
            DotNetEnv.Env.Load();

            string connectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
            string jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            string jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");


            builder.Services.AddDbContext<DBContextTechEmporiumTrend>(
                options=>options.UseSqlServer(connectionString)
                
                );

>>>>>>> Stashed changes
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add authentication service
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddAuthorization();

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
