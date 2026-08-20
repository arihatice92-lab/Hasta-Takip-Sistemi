using HastaTakip.DataAccess;
using HastaTakip.Business;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// DI (dependency injection) container. biri DbHelper isterse, ona appsettings.json'daki HastaTakipDb connection string'i ile oluşturulmuş bir DbHelper nesnesi ver
builder.Services.AddControllersWithViews(options =>
{
    options.ModelBindingMessageProvider.SetValueIsInvalidAccessor(
        value => "Bu alan geçerli bir değer içermiyor.");

    options.ModelBindingMessageProvider.SetMissingBindRequiredValueAccessor(
        name => "Bu alan boş geçilemez.");

    options.ModelBindingMessageProvider.SetAttemptedValueIsInvalidAccessor(
        (value, name) => "Bu alan geçerli bir değer içermiyor.");
    options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
        value => "Bu alan boş geçilemez.");
});

builder.Services.AddScoped<DbHelper>(sp =>
    new DbHelper(builder.Configuration.GetConnectionString("HastaTakipDb")
        ?? throw new InvalidOperationException("HastaTakipDb connection string bulunamadı. appsettings.json dosyasını kontrol edin.")));

builder.Services.AddScoped<HastaDal>();
builder.Services.AddScoped<HastaBusiness>();

builder.Services.AddScoped<KullaniciDal>();
builder.Services.AddScoped<KullaniciBusiness>();

builder.Services.AddScoped<DoktorDal>();
builder.Services.AddScoped<DoktorBusiness>();

builder.Services.AddScoped<RandevuSaatDal>();
builder.Services.AddScoped<RandevuDal>();
builder.Services.AddScoped<RandevuBusiness>();
builder.Services.AddScoped<RandevuSaatBusiness>();

builder.Services.AddScoped<TaniDal>();
builder.Services.AddScoped<TaniBusiness>();
builder.Services.AddScoped<IlacDal>();
builder.Services.AddScoped<IlacBusiness>();
builder.Services.AddScoped<HastaTaniDal>();
builder.Services.AddScoped<HastaTaniBusiness>();
builder.Services.AddScoped<HastaTedaviDal>();
builder.Services.AddScoped<HastaTedaviBusiness>();

builder.Services.AddScoped<TestDal>();
builder.Services.AddScoped<TestBusiness>();
builder.Services.AddScoped<OlcekDal>();
builder.Services.AddScoped<OlcekBusiness>();
builder.Services.AddScoped<PsikologDal>();
builder.Services.AddScoped<PsikologBusiness>();
builder.Services.AddScoped<HastaTestSonucDal>();
builder.Services.AddScoped<HastaTestSonucBusiness>();
builder.Services.AddScoped<HastaOlcekSonucDal>();
builder.Services.AddScoped<HastaOlcekSonucBusiness>();

builder.Services.AddScoped<TestAltKumeDal>();
builder.Services.AddScoped<TestAltKumeBusiness>();
builder.Services.AddScoped<AltKumeSonucDal>();
builder.Services.AddScoped<AltKumeSonucBusiness>();

builder.Services.AddScoped<RandevuNotuDal>();
builder.Services.AddScoped<RandevuNotuBusiness>();

builder.Services.AddScoped<AileBilgileriDal>();
builder.Services.AddScoped<AileBilgileriBusiness>();
builder.Services.AddScoped<AileOykusuDal>();
builder.Services.AddScoped<AileOykusuBusiness>();
builder.Services.AddScoped<GelisimselOykuDal>();
builder.Services.AddScoped<GelisimselOykuBusiness>();

builder.Services.AddScoped<KayitNotuDal>(); 
builder.Services.AddScoped<KayitNotuBusiness>();

builder.Services.AddScoped<PsikologRandevuSaatDal>();
builder.Services.AddScoped<PsikologRandevuSaatBusiness>();
builder.Services.AddScoped<PsikologRandevuDal>();
builder.Services.AddScoped<PsikologRandevuBusiness>();

builder.Services.AddScoped<IstatistikDal>();
builder.Services.AddScoped<IstatistikBusiness>();

builder.Services.AddScoped<DoktorIzniDal>();
builder.Services.AddScoped<DoktorIzniBusiness>();

builder.Services.AddScoped<KayitDosyasiDal>();
builder.Services.AddScoped<KayitDosyasiBusiness>();

builder.Services.AddScoped<PsikologIzniDal>();
builder.Services.AddScoped<PsikologIzniBusiness>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/Login";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Dosya klasörünü, wwwroot DIŞINDA, uygulama kök dizininde oluşturuyoruz
var dosyaKlasoruAdi = builder.Configuration["DosyaAyarlari:KayitDosyalariKlasoru"] ?? "KayitDosyalari";
var dosyaKlasoruYolu = Path.Combine(app.Environment.ContentRootPath, dosyaKlasoruAdi);
if (!Directory.Exists(dosyaKlasoruYolu))
{
    Directory.CreateDirectory(dosyaKlasoruYolu);
}

// Configure the HTTP request pipeline."geliştirirken bana her şeyi göster, ama gerçek kullanıcılara teknik detayları gösterme" diyen bir güvenlik/kullanılabilirlik ayrımı
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