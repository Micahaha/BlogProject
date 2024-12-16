using BlogProject.Data;
using BlogProject.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("MY_SQL_CONNECTIONSTRING");
Console.WriteLine(connectionString);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));



 builder.Services.AddDbContext<ApplicationDbContext>(
    options =>
    {
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    });

builder.Services.AddSerilog();
builder.Services.AddIdentityCore<ApplicationUser>();
builder.Services.AddDefaultIdentity<ApplicationUser>()
        .AddRoles<IdentityRole>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddDefaultTokenProviders()
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(opts =>
{
    opts.SignIn.RequireConfirmedEmail = true;
});

builder.Services.AddControllersWithViews();
builder.Services.AddScoped<BlogService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

var configuration = builder.Configuration;


builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    var googleCredentials = builder.Configuration.GetSection("GoogleCredentials").Get<GoogleCrendentials>();
    googleOptions.ClientId = googleCredentials.ClientId;
    googleOptions.ClientSecret = googleCredentials.ClientSecret;
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.Cookie.Name = "Cookie";
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
    options.LoginPath = "/Account/Login";
    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
    options.SlidingExpiration = true;
});

builder.Logging.AddConsole();
builder.Services.AddMySqlDataSource(connectionString);




var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

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

    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Member", "Author" };

    foreach(var role in roles)
    {

        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }
}


// Give ADMIN role to my email account when I make it
using (var scope = app.Services.CreateScope())
{
    var userManager =
        scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    if (await userManager.FindByEmailAsync("xicer029@gmail.com") != null)
    {

        var user = await userManager.FindByEmailAsync("xicer029@gmail.com");

        // user.EmailConfirmed = true;

        await userManager.AddToRoleAsync(user, "Admin");
    }

}



app.MapRazorPages();

app.Run();
