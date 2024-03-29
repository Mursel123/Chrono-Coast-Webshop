using Microsoft.EntityFrameworkCore;
using StockApi.Data;
using StockApi.Data.DefaultData;
using StockApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IStockRepository, StockRepository>();
var connString = builder.Configuration.GetConnectionString("AppDbConnStock");
builder.Services.AddDbContext<DataContext>(opts =>
{
    opts.UseSqlServer(connString);
    opts.EnableSensitiveDataLogging(true);
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

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});
SeedData.SeedDatabase(app);
app.Run();
