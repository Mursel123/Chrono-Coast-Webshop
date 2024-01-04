using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PXLPRO2023Shoppers29.Data;
using PXLPRO2023Shoppers29.Data.DefaultData;
using PXLPRO2023Shoppers29.Models;
using PXLPRO2023Shoppers29.Services;
using PXLPRO2023Shoppers29.Services.Interfaces;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Http;
using System;
using System.Configuration;
using Microsoft.AspNetCore.Builder.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient(ApiConstants.StockApiHttpClientName, c => { c.BaseAddress = new Uri("https://localhost:7176/api/"); });
var connectionString = builder.Configuration.GetConnectionString("AppDbConn");
builder.Services.AddDbContext<ShopDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddIdentity<ShopUser, IdentityRole>(identity =>
{
    identity.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<ShopDbContext>();
builder.Services.Configure<IdentityOptions>(options => {
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = true;
});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "756658752925-6pji7skoas523uvl2nakstr5jlqreheg.apps.googleusercontent.com";
    options.ClientSecret = "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";
}).AddFacebook(fbOpts => {
    fbOpts.AppId = "5988912264531409";
    fbOpts.AppSecret = "548858cea6bea8abb9fa1f42e68444c4";
});
builder.Services.AddAuthentication()
        //.AddCookie("Cookies")
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:5001";
            options.ClientId = "mvc";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.Scope.Add("openid");
            options.Scope.Add("profile");
            options.Scope.Add("email");
            options.GetClaimsFromUserInfoEndpoint = true;
        });
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IShoppingCartService, ShoppingCartService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

    
    app.MapBlazorHub();
});




await SeedDataIdentity.EnsurePopulatedAsync(app);

app.Run();
