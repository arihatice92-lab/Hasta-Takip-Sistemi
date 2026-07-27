using HastaTakip.DataAccess;
using HastaTakip.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<DbHelper>(sp =>
    new DbHelper(builder.Configuration.GetConnectionString("HastaTakipSistemi")
        ?? throw new InvalidOperationException("HastaTakipDb connection string bulunamadı. appsettings.json dosyasını kontrol edin.")));

builder.Services.AddScoped<HastaDal>();
builder.Services.AddScoped<HastaBusiness>();

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

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
