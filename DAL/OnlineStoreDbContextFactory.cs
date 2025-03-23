using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace DAL
{
    public class OnlineStoreDbContextFactory : IDesignTimeDbContextFactory<OnlineStoreDbContext>
    {
        public OnlineStoreDbContext CreateDbContext(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddUserSecrets<OnlineStoreDbContextFactory>()
                .Build();

            var connectionString = configuration.GetConnectionString("Default") ??
                                   configuration["ConnectionStrings:LocalConnectionString"];

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string is not set. Check secrets, environment variables, or appsettings.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<OnlineStoreDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new OnlineStoreDbContext(optionsBuilder.Options);
        }
    }
}
