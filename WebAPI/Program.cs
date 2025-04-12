using DAL;
using Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

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
        Title = "Craft Sweets Online Store API",
        Version = "v1"
    });
});


builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CategoryService>();

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddHttpClient();

//builder.Services.AddIdentityCore<AppUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<OnlineStoreDbContext>();

builder.Services.AddDbContext<OnlineStoreDbContext>(options =>
{
    options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DAL"));
});

builder.Logging.AddConsole();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<OnlineStoreDbContext>();

    context.Database.Migrate();

    var categoryInitializer = new CategoryInitializer(context);
    categoryInitializer.InitializeCategories();
    var productInitializer = new ProductInitializer(context);
    productInitializer.InitializeProducts();
}

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Craft Sweets API v1");
    options.DocumentTitle = "Craft Sweets";
});

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.Run();
