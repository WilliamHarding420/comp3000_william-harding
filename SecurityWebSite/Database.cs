using Microsoft.EntityFrameworkCore;
using SecurityWebSite.DatabaseModels;
using System.Data.Common;

namespace SecurityWebSite
{
    public class Database : DbContext
    {

        public static string IP = "127.0.0.1";

        public static string Username = "SA";
        public static string Password = string.Empty;

        private static string ConnectionString = string.Empty;

        public void BuildConnectionString()
        {

            ConnectionString = $"User id={Username};" +
                $"Password={Password};" +
                $"Data Source={IP};" +
                $"TrustServerCertificate=True;" +
                $"Initial catalog=security_website";

        }

        public async void EnsureDBCreated()
        {

            try
            {

                if (Database.EnsureCreated())
                {

                    string password = "admin";
                    string salt = SecurityUtils.GenerateSalt();

                    string passwordHash = SecurityUtils.HashPassword(password, salt);

                    User adminAccount = new User()
                    {
                        Username = "admin",
                        PasswordHash = passwordHash,
                        Salt = salt
                    };

                    await Users.AddAsync(adminAccount);
                    await SaveChangesAsync();

                }

            }catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString, sqlOptions => sqlOptions.EnableRetryOnFailure(10));
        }

        public DbSet<User> Users { get; set; }

    }
}
