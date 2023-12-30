using almondcove.Extensions;
using almondcove.Interefaces.Repositories;
using almondcove.Interefaces.Services;
using almondcove.Repositories;
using almondcove.Services;
using Almondcove.Interefaces.Services;
using Almondcove.Services;
using Microsoft.AspNetCore.Session;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSession(options =>
{
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
                builder.WithOrigins("https://jsm33t.in", "http://localhost:4200/")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});



builder.Services.AddAuthentication("MyCookieAuthenticationScheme")
     .AddCookie("MyCookieAuthenticationScheme", options =>
     {
         options.LoginPath = "/account/login";
         options.AccessDeniedPath = "/404";
         options.ExpireTimeSpan = TimeSpan.FromDays(100);
     });
//exc services
builder.Services.AddSingleton<IConfigManager, ConfigManager>();
builder.Services.AddSingleton<IMailer, Mailer>();
builder.Services.AddSingleton<ISqlService, SqlService>();

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

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();