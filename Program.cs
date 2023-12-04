using almondCove.Extensions;
using almondCove.Interefaces.Services;
using almondCove.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Session;
using WebMarkupMin.AspNetCore7;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".almondCove.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
});
builder.Services.AddControllersWithViews();

//exc services

builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IMailer, Mailer>();
builder.Services.AddSingleton<ISqlService, SqlService>();


builder.Services.AddWebMarkupMin(options =>
{
    options.AllowMinificationInDevelopmentEnvironment = false;
    options.AllowCompressionInDevelopmentEnvironment = false;
})
.AddHtmlMinification(options =>
{
    options.MinificationSettings.RemoveRedundantAttributes = true;
    options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
    options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
    options.MinificationSettings.PreserveNewLines = true;
    options.MinificationSettings.MinifyEmbeddedCssCode = true;
    options.MinificationSettings.RemoveHtmlComments = true;
    options.MinificationSettings.RemoveHtmlCommentsFromScriptsAndStyles = true;
    options.MinificationSettings.MinifyEmbeddedJsCode = true;
})
.AddXmlMinification()
.AddHttpCompression();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.Cookie.HttpOnly = true;
           options.Cookie.IsEssential = true;
           options.ExpireTimeSpan = TimeSpan.FromDays(200);
           options.SlidingExpiration = true;
       })
       ;

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<SessionMiddleware>();
//app.UseWebMarkupMin();
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
app.UseCookieCheckMiddleware();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
