using BSExpPhotos.Interfaces;
using BSExpPhotos.Services;
using DAL;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Options;
using System.Text.Json.Serialization;
using WebAPI;
using WebAPI.Middleware;
using WebAPI.Filters;
using Quartz;
using WebAPI.Schedulers;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Default");

var localConnectionString = builder.Configuration["ConnectionStrings:LocalConnectionString"];

if (!string.IsNullOrWhiteSpace(localConnectionString))
{
    connectionString = localConnectionString;
}

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("Connection string is not set. Check environment variables, appsettings.json, or secrets.");
}

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Craft Sweets",
        Version = "v1"
    });
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.NumberHandling = JsonNumberHandling.Strict;
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<OnlineStoreDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DAL"));
});

builder.Services.AddIdentityCore<AppUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<OnlineStoreDbContext>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<FavoriteProductService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<AppUserService>();
builder.Services.AddScoped<PromotionService>();
builder.Services.AddScoped<IImageInfoExtractor, ImageInfoExtractor>(); 
builder.Services.AddScoped<IImageUploadMetadataService, ImageUploadMetadataService>();
builder.Services.AddScoped<TrackImageUploadAttribute>();
builder.Services.AddScoped<IImageCleanupService,PhotoCleanupService>();
builder.Services.AddScoped<ImageCleanupMiddlewareForDeleteMethods>();



// Configure Quartz.NET
builder.Services.AddScoped<PhotoCleanupJob>(); 

builder.Services.AddQuartz(q =>
{
    var jobKey = new JobKey("PhotoCleanupJob");
    q.AddJob<PhotoCleanupJob>(opts => opts.WithIdentity(jobKey));
    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("PhotoCleanupTrigger")
        .WithCronSchedule("0 0 0 * * ?")); // --Run every day at midnight: 0 0 0 * * ?

});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.Configure<ImageStorageSettings>(
    builder.Configuration.GetSection("ImageStorageSettings"));
builder.Services.AddScoped<ImageService>();
builder.Services.AddSingleton(resolver =>
    resolver.GetRequiredService<IOptions<ImageStorageSettings>>().Value);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        policy => policy.WithOrigins(
                "https://sweet-online-store.vercel.app",
                "https://online-store-git-feature-basket-doboshdiana404s-projects.vercel.app",
                "https://online-store-git-page-catalog-doboshdiana404s-projects.vercel.app",
                "http://localhost:5173",  
                "http://192.168.0.108:5173"  
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OnlineStoreDbContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
   
    

    var migrator = context.Database.GetService<IMigrator>();

    var appliedMigrations = context.Database.GetAppliedMigrations().ToList();
    var pendingMigrations = context.Database.GetPendingMigrations().ToList();

    if (!appliedMigrations.Any())
    {
        context.Database.Migrate();
    }
    else if (pendingMigrations.Any())
    {
        foreach (var migration in pendingMigrations)
        {
            migrator.Migrate(migration);
        }
    }
    


    var categoryInitializer = new CategoryInitializer(context);
    categoryInitializer.InitializeCategories();

    var productInitializer = new ProductInitializer(context);
    productInitializer.InitializeProducts();

    var appUserInitializer = new AppUserInitializer(context, userManager);
    appUserInitializer.InitializeUsers();

    var promotionInitializer = new PromotionInitializer(context);
    await promotionInitializer.InitializePromotions();
    
}

app.UseCors("AllowSpecificOrigin");

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Craft Sweets API v1");
    options.DocumentTitle = "Craft Sweets";
});
//}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseMiddleware<ExceptionHandlerMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();
app.UseMiddleware<ImageCleanupMiddlewareForDeleteMethods>();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
