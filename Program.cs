using almondcove.Extensions;
using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Repositories;
using almondcove.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".almondcove.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
});
builder.Services.AddControllersWithViews();
    

builder.Services.AddAuthentication("MyCookieAuthenticationScheme")
     .AddCookie("MyCookieAuthenticationScheme", options =>
     {
         options.LoginPath = "/account/login"; // Customize the login path
         options.AccessDeniedPath = "/404"; // Customize the access denied path
         options.ExpireTimeSpan = TimeSpan.FromDays(100);
     });
//exc services
builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IMailer, Mailer>();
builder.Services.AddSingleton<ISqlService, SqlService>();
builder.Services.AddScoped<IMailingListRepository, MailingListRepository>();


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<SessionMiddleware>();
app.UseCookieCheckMiddleware();
app.UseSession();
app.UseHttpsRedirection();
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
app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
