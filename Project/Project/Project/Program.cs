using Project.EmailService;
using Project.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// C?u h�nh DbContext
builder.Services.AddDbContext<DatabaseContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnection"));
});


// C?u h�nh d?ch v? cho EmailService
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<PayPalService>();
// C?u h�nh c�c d?ch v? kh�c
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// C?u h�nh authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
    options.LoginPath = "/Account/Index";
    options.AccessDeniedPath = "/Account/AccessDenied";
});



// C?u h�nh c�c repository
builder.Services.AddTransient<IAccountRepository, AccountRepository>();

var app = builder.Build();

// C?u h�nh HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Web}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Web}/{action=Index}/{id?}");

app.Run();
