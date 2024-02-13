using almondcove.Extensions;
using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Repositories;
using almondcove.Services;
using Almondcove.Interefaces.Services;
using Almondcove.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Session;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.Name = ".almondcove.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    
});
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
                builder.WithOrigins("https://jsm33t.in")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

builder.Services.AddRateLimiter(options => {

    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    //options.AddPolicy("fixed", HttpContext =>
    //RateLimitPartition.GetFixedWindowLimiter(
    //    partitionKey: HttpContext.Connection.RemoteIpAddress?.ToString(),
    //        factory: _ => new FixedWindowRateLimiterOptions { 
    //            PermitLimit = 10,
    //            Window = TimeSpan.FromSeconds(10)
    //        }
    //    )
    //);

    options.AddFixedWindowLimiter("fixed", o =>
    {
        o.PermitLimit = 10;
        o.Window = TimeSpan.FromSeconds(10);
    });
});

builder.Services.AddAuthentication()
     .AddCookie(options =>
     {
         options.Cookie.HttpOnly = true;
         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
         options.Cookie.Name = "Cookie"; // Change the cookie name as needed
         options.LoginPath = "/Account/Login"; // Set the login path
         options.LogoutPath = "/Account/Logout"; // Set the logout path
         options.AccessDeniedPath = "/Account/AccessDenied"; // Set the access denied path
         options.ExpireTimeSpan = TimeSpan.FromDays(100); // Adjust expiration time as needed
         options.SlidingExpiration = true;
     });
//exc services
builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddScoped<IMailer, Mailer>();
builder.Services.AddScoped<IMailingListRepository, MailingListRepository>();
builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ISearchRepository,SearchRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IBlogRepository, BlogRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseSession();
app.UseMiddleware<SessionMiddleware>();
app.UseCookieCheckMiddleware();
app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseStaticFiles();
app.Use(async (context, next) =>
{
    await next();
    if (context.Response.StatusCode == 404)
    {
        context.Request.Path = "/404";
        await next();
    }
});
app.UseRouting();
app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();