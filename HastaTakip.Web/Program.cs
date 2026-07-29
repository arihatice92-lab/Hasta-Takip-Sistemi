using HastaTakip.DataAccess;
using HastaTakip.Business;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// DI (dependency injection) container
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DbHelper>(sp =>
    new DbHelper(builder.Configuration.GetConnectionString("HastaTakipDb")
        ?? throw new InvalidOperationException("HastaTakipDb connection string bulunamadı. appsettings.json dosyasını kontrol edin.")));

builder.Services.AddScoped<HastaDal>();
builder.Services.AddScoped<HastaBusiness>();

builder.Services.AddScoped<KullaniciDal>();
builder.Services.AddScoped<KullaniciBusiness>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();