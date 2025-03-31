using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using ProjektNeveBackend.Models;
using Microsoft.Extensions.Hosting;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Cors;
using ProjektNeveBackend.Services;

namespace ProjektNeveBackend
{
    public class Program
    {
        public static string ftpUrl = "";
        public static string ftpUserName = "";
        public static string ftpPassword = "";

        public static int SaltLength = 64;

        public static Dictionary<string, User> LoggedInUsers = new Dictionary<string, User>();

        public static string GenerateSalt()
        {
            Random random = new Random();
            string karakterek = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            string salt = "";
            for (int i = 0; i < SaltLength; i++)
            {
                salt += karakterek[random.Next(karakterek.Length)];
            }
            return salt;
        }
        public static string CreateSHA256(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] data = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));
                var sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }
        public static void Main(string[] args)
        {
         //   LoggedInUsers["token"] = new User { Id = 1, Jogosultsag = 9 };
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                    options.AddPolicy("AllowAllOrigins",
                        policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
            builder.Services.AddDbContext<BackendAlapContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddScoped<IPasswordService, PasswordService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            var app = builder.Build();
            app.UseCors("AllowAllOrigins");
            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();
           // app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
//Scaffold-DbContext "SERVER=localhost;PORT=3306;DATABASE=backend_alap;USER=root;PASSWORD=;SSL MODE=none;" mysql.entityframeworkcore -outputdir Models -f

//a423d00fd9f48fd343ad3c214a28fcbcc0f69f0193ba087336a57b3aa04e15fb
