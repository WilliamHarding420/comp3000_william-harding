using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace SecurityWebSite
{
    public class Program
    {

        public static string SECURITY_KEY = "security-website-token-validation-security-key";
        public static TokenValidationParameters Validation_Parameters;

        public static void Main(string[] args)
        {

            // Getting env variables
            string? DBUser = Environment.GetEnvironmentVariable("dbUser");
            string? DBPassword = Environment.GetEnvironmentVariable("dbPassword");
            string? DBIP = Environment.GetEnvironmentVariable("dbIP");

            if (DBUser == null ||  DBPassword == null || DBIP == null)
            {
                Console.WriteLine("Missing required environment variables. Required: dbUser, dbPassword, dbIP");
                return;
            }

            // Setting up database
            Database.IP = DBIP;
            Database.Password = DBPassword;
            Database.Username = DBUser;

            Database db = new();
            db.BuildConnectionString();
            db.EnsureDBCreated();

            // Configuring JWT validaiton
            Validation_Parameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidAudience = "user",
                ValidateIssuer = true,
                ValidIssuer = "security-website",
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECURITY_KEY))
            };

            // Web server setup

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Configuring JWT authentication
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(settings =>
            {
                settings.TokenValidationParameters = Validation_Parameters;
            });

            // Configuring https certificate to use docker volume cert
            builder.WebHost.ConfigureKestrel(config =>
            {
                config.ListenAnyIP(444, options =>
                {
                    options.UseHttps(httpsOptions =>
                    {
                        httpsOptions.ServerCertificate = new X509Certificate2(@"/app/cert/websitecert.pfx", "password");
                    });
                });
                config.ListenAnyIP(8080);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Urls.Add("http://[::]:8080");
            app.Urls.Add("https://[::]:444");

            app.Run();
        }
    }
}
