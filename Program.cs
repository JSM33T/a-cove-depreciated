using almondCove.Extensions;
using almondCove.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Session;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".almondCove.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
});
// Add services to the container.
builder.Services.AddControllersWithViews();

//exc services
builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IMailer, Mailer>();
builder.Services.AddSingleton<ISqlService, SqlService>();



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.Cookie.HttpOnly = true;
           options.Cookie.IsEssential = true;
           options.ExpireTimeSpan = TimeSpan.FromDays(150);
           options.SlidingExpiration = true; // Extend the expiration time with each request
       });


Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console() 
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day) 
                .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
        loggingBuilder.AddSerilog());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseMiddleware<SessionMiddleware>();
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseCookieCheckMiddleware();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
