using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class AileBilgileri
    {
        public int AileBilgileriID { get; set; }
        public string? HastaTC { get; set; } 

        public bool AnneYasiyorMu { get; set; }
        public string? AnneAd { get; set; }
        public string? AnneSoyad { get; set; }
        public byte? AnneYas { get; set; }
        public string? AnneEgitim { get; set; } 
        public string? AnneIs { get; set; } 
        public string? AnneTel { get; set; }
        public string? AnneAdres { get; set; }

        public bool BabaYasiyorMu { get; set; }
        public string? BabaAd { get; set; } 
        public string? BabaSoyad { get; set; } 
        public byte? BabaYas { get; set; }
        public string? BabaEgitim { get; set; } 
        public string? BabaIs { get; set; }
        public string? BabaTel { get; set; } 
        public string? BabaAdres { get; set; } 

        public bool UveyVeyaKoruyucuVarMi { get; set; }
        public string? UveyEbeveynTuru { get; set; } 
        public string? UveyAd { get; set; }
        public string? UveySoyad { get; set; } 
        public byte? UveyYas { get; set; }
        public string? UveyEgitim { get; set; } 
        public string? UveyIs { get; set; } 
        public string? UveyTel { get; set; }
        public string? UveyAdres { get; set; } 
        public bool? UveyYasiyorMu { get; set; }

        public bool AkrabaEvliligi { get; set; }
        public string? AileTipi { get; set; } 
        public string? EbeveynDurumu { get; set; } 
        public string? Kardesler { get; set; } 
        public string? AilePsikiyatrikOyku { get; set; }
        public string? AileTibbiOyku { get; set; } 
        public string? AileEkNotlar { get; set; }
        public int? OlusturanKullaniciID { get; set; }
        public DateTime? OlusturmaTarihi { get; set; }
        public int? SonGuncelleyenKullaniciID { get; set; }
        public DateTime? SonGuncellemeTarihi { get; set; }
    }
}
