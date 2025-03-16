using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

using DAL;

public class OnlineStoreDbContextFactory : IDesignTimeDbContextFactory<OnlineStoreDbContext>
{
    public OnlineStoreDbContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())  
            .AddJsonFile(@"appsettings.json")  
            .Build();
        
        var optionsBuilder = new DbContextOptionsBuilder<OnlineStoreDbContext>();
        var connectionString = config.GetConnectionString("Default");

        optionsBuilder.UseSqlServer(connectionString);

        return new OnlineStoreDbContext(optionsBuilder.Options);
    }
}