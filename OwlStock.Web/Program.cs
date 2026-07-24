using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OwlStock.Infrastructure;
using OwlStock.Services;
using PuppeteerSharp;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Load configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
    .AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true)
    .Build();

// Add services to the container.
var columnOptions = new ColumnOptions();
columnOptions.Store.Remove(StandardColumn.Properties); 
columnOptions.Store.Add(StandardColumn.LogEvent);
columnOptions.LogEvent.DataType = SqlDbType.NVarChar;

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration) // Read config from appsettings.json
    .WriteTo.MSSqlServer(
        connectionString: (connectionString ??
        throw new NullReferenceException($"{connectionString} is null")),
        sinkOptions: new MSSqlServerSinkOptions
        {
            TableName = "Logs",
            AutoCreateSqlTable = true // Automatically creates table if not exists
        },
        columnOptions: columnOptions
    )
    .CreateLogger();

// Add Serilog to ASP.NET Core
builder.Host.UseSerilog();


builder.Services.AddDbContext<OwlStockDbContext>(options =>
    options.UseSqlServer(connectionString ?? 
        throw new NullReferenceException($"{connectionString} is null")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
{
    //password policy
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;

    //set lockout settings
    options.Lockout.AllowedForNewUsers = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
    options.Lockout.MaxFailedAccessAttempts = 6;
})
    .AddEntityFrameworkStores<OwlStockDbContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
});

builder.Services.AddMvc().
    AddJsonOptions(options =>
    {
        JsonStringEnumConverter enumConverter = new();
        options.JsonSerializerOptions.Converters.Add(enumConverter);
    });

builder.Services.AddServices();

builder.Services.AddRazorPages();

// Download Chromium if it doesn't exist
BrowserFetcher browserFetcher = new();
await browserFetcher.DownloadAsync();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Views/Shared/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapDefaultControllerRoute();

//app.MapControllerRoute("dreampixRoute", "{action}/{id?}");

using (var scope = app.Services.CreateScope())
{
    RoleManager<IdentityRole> roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    string[] roles = { "Administrator", "User" };

    for(int i = 0; i < roles.Length; i++)
    {
        if(!await roleManager.RoleExistsAsync(roles[i]))
        {
            await roleManager.CreateAsync(new (roles[i]));
        }
    }

}

app.Run();
