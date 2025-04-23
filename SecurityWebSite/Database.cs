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

                    User admin = CreateDefaultUser("admin", "admin");
                    User publishUser = CreateDefaultUser("camera", "LET_ME_IN");
                    publishUser.CanStream = true;

                    await Users.AddAsync(admin);
                    await Users.AddAsync(publishUser);
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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Location>()
                .HasMany(e => e.Cameras)
                .WithOne(e => e.Location)
                .HasForeignKey(e => e.LocationID)
                .HasPrincipalKey(e => e.LocationID)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Camera> Cameras { get; set; }
        public DbSet<Location> Locations { get; set; }

        private User CreateDefaultUser(string name, string password)
        {
            string salt = SecurityUtils.GenerateSalt();

            string passwordHash = SecurityUtils.HashPassword(password, salt);

            User userAccount = new User()
            {
                Username = name,
                PasswordHash = passwordHash,
                Salt = salt
            };

            return userAccount;
        }

    }
}
