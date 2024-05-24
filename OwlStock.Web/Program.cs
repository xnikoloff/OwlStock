using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OwlStock.Infrastructure;
using OwlStock.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OwlStockDbContext>(options =>
    options.UseSqlServer(connectionString ?? 
        throw new NullReferenceException($"{connectionString} is null")));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services
    .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<OwlStockDbContext>();

builder.Services.AddMvc().
    AddJsonOptions(options =>
    {
        JsonStringEnumConverter enumConverter = new();
        options.JsonSerializerOptions.Converters.Add(enumConverter);
    });

//configure password
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequiredLength = 8;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireDigit = true;
    options.Password.RequireNonAlphanumeric = false;
});

builder.Services.AddServices();

builder.Services.AddRazorPages();


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
