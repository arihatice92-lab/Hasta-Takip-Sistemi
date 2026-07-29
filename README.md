#Hasta Takip Sistemi
Psikiyatri/psikoloji kliniklerinde kullanılmak üzere geliştirilen, hasta kayıtlarını, randevuları, tanı/tedavi süreçlerini ve test-ölçek sonuçlarını yöneten bir web uygulaması.
#Özellikler
•	Hasta kayıt, güncelleme, listeleme ve pasife alma (arşivleme) işlemleri
•	Doktor ve psikolog bilgi yönetimi
•	Randevu oluşturma, çakışma kontrolü ve durum takibi (Planlandı / Tamamlandı / İptal / Gelmedi)
•	Tanı, tedavi (ilaç) ve görüşme notu kayıtları
•	Psikolojik test ve ölçek sonuçlarının kaydı
•	Aile bilgileri ve gelişimsel öykü modülleri
•	18 yaş altı hasta kaydı doğrulaması (başvuru tarihine göre yaş hesaplama)
#Teknolojiler
•	ASP.NET Core MVC — sunum katmanı
•	ADO.NET (Microsoft.Data.SqlClient) — veri erişim katmanı
•	SQL Server — veritabanı (stored procedure, trigger, view ve CHECK constraint'ler ile)
•	Katmanlı Mimari (N-Tier) — Entities / DataAccess / Business / Web katmanları
#Proje Yapısı
HastaTakipSistemi.sln
├── HastaTakip.Entities      → Veritabanı tablolarına karşılık gelen POCO sınıfları
├── HastaTakip.DataAccess    → ADO.NET ile CRUD işlemleri (DbHelper, [Tablo]Dal sınıfları)
├── HastaTakip.Business      → İş kuralları ve doğrulamalar ([Tablo]Business sınıfları)
├── HastaTakip.Web           → MVC Controller'lar, View'lar
└── Database/                → SQL script'leri (tablo, trigger, SP, view, index tanımları)
Bağımlılık yönü tek yönlü ilerler:
Web  →  Business  →  DataAccess  →  Entities
Web katmanı DataAccess'e doğrudan referans vermez; tüm veri erişimi Business katmanı üzerinden yapılır.
#Veritabanı Tasarımı
•	21 tablo: hasta, doktor, psikolog, randevu (tarih/saat/not), tedavi, tanı, test/test alt küme, ölçek, sonuç tabloları, aile bilgileri ve gelişimsel öykü
•	Trigger'lar: otomatik hasta dosya numarası üretimi, randevu durum geçiş kontrolü, randevu tamamlanmadan önce not girişi zorunluluğu
•	Stored procedure'ler: hasta kaydet/güncelle/sil/pasife al, randevu oluştur, hasta tedavi/tanı sorguları
•	CHECK constraint'ler: TC kimlik formatı, telefon formatı, randevu tarihinin geçmişte olmaması gibi veri bütünlüğü kuralları
#Kurulum
1.	Database/ klasöründeki SQL script'lerini sırasıyla SQL Server üzerinde çalıştırın.
2.	HastaTakip.Web/appsettings.json dosyasındaki ConnectionStrings:HastaTakipDb değerini kendi SQL Server bağlantı bilginizle güncelleyin:
json
   "ConnectionStrings": {
     "HastaTakipDb": "Server=.;Database=HastaTakipSistemi;Trusted_Connection=True;TrustServerCertificate=True;"
   }
3.	Solution'ı Visual Studio'da açıp HastaTakip.Web projesini başlatın.
#Geliştirme Durumu
Proje aktif geliştirme aşamasındadır. Şu ana kadar tamamlananlar:
•	 Veritabanı şeması (tablolar, trigger'lar, stored procedure'ler, view'lar, index'ler)
•	 Entity katmanı (21 sınıf)
•	 Hasta modülü — DataAccess ve Business katmanları
•	 Hasta modülü — Controller ve View'lar
•	 Diğer modüller (Doktor, Randevu, Tedavi, Tanı, Test/Ölçek Sonuçları, Aile Bilgileri, Gelişimsel Öykü)


