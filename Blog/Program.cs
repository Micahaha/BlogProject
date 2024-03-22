using BlogProject.Data;
using BlogProject.Models;
using BlogProject.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("SQL_CONNECTIONSTRING");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentityCore<ApplicationUser>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddRoles<IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<BlogService>();


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");






// create roles if web app is moved from place to place 
using(var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Member", "Author" };

    foreach(var role in roles)
    {

        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}

// Create test users DONT PUSH TO PROD
using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string email = "admin@admin.com";
    string password = "Firelegend101$";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email
        };
        // user.EmailConfirmed = true;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}



app.MapRazorPages();

app.Run();
