using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.Security.Cryptography.X509Certificates;

namespace SecurityWebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Configuring https certificate to use docker volume cert
            builder.WebHost.ConfigureKestrel(config =>
            {
                config.ListenAnyIP(443, options =>
                {
                    options.UseHttps(httpsOptions =>
                    {
                        httpsOptions.ServerCertificate = new X509Certificate2(@"/app/cert/websitecert.pfx", "password");
                    });
                });
                config.ListenAnyIP(80);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Urls.Add("http://[::]:80");
            app.Urls.Add("https://[::]:443");

            app.Run();
        }
    }
}
