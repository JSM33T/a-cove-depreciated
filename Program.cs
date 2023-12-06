using almondCove.Extensions;
using almondCove.Interefaces.Services;
using almondCove.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Session;
using WebMarkupMin.AspNetCore7;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".almondCove.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
});
builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication("MyCookieAuthenticationScheme")
     .AddCookie("MyCookieAuthenticationScheme", options =>
     {
         options.LoginPath = "/"; // Customize the login path
         options.AccessDeniedPath = "/404"; // Customize the access denied path
     });
//exc services
builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IMailer, Mailer>();
builder.Services.AddSingleton<ISqlService, SqlService>();

builder.Services.AddWebMarkupMin(options =>
{
    options.AllowMinificationInDevelopmentEnvironment = true;
    options.AllowCompressionInDevelopmentEnvironment = true;
})
//.AddHtmlMinification(options =>
//{
//    options.MinificationSettings.RemoveRedundantAttributes = true;
//    options.MinificationSettings.RemoveHttpProtocolFromAttributes = true;
//    options.MinificationSettings.RemoveHttpsProtocolFromAttributes = true;
//    options.MinificationSettings.PreserveNewLines = true;
//    options.MinificationSettings.MinifyEmbeddedCssCode = true;
//    options.MinificationSettings.RemoveHtmlComments = true;
//    options.MinificationSettings.RemoveHtmlCommentsFromScriptsAndStyles = true;
//    options.MinificationSettings.MinifyEmbeddedJsCode = true;
//})
.AddXmlMinification()
.AddHttpCompression();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseMiddleware<SessionMiddleware>();
app.UseWebMarkupMin();
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
