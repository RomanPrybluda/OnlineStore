using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DAL
{
    public class OnlineStoreDbContextFactory : IDesignTimeDbContextFactory<OnlineStoreDbContext>
    {
        public OnlineStoreDbContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "WebAPI"); 

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) 
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<OnlineStoreDbContext>();
            var connectionString = config.GetConnectionString("Default");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException("Connection string is missing.");
            }

            optionsBuilder.UseSqlServer(connectionString);

            return new OnlineStoreDbContext(optionsBuilder.Options);
        }
    }
}
