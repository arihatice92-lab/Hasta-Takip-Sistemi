using HastaTakip.DataAccess;
using HastaTakip.Business;

var builder = WebApplication.CreateBuilder(args);

// DI(dependency injection) container
builder.Services.AddControllersWithViews();

//session kullanımı
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<DbHelper>(sp =>
    new DbHelper(builder.Configuration.GetConnectionString("HastaTakipDb")
        ?? throw new InvalidOperationException("HastaTakipDb connection string bulunamadı. appsettings.json dosyasını kontrol edin.")));

builder.Services.AddScoped<HastaDal>();
builder.Services.AddScoped<HastaBusiness>();

builder.Services.AddScoped<KullaniciDal>();
builder.Services.AddScoped<KullaniciBusiness>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
