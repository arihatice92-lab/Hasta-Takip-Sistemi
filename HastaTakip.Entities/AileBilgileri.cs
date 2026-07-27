using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class AileBilgileri
    {
        public int AileBilgileriID { get; set; }
        public string HastaTC { get; set; } = string.Empty;

        public bool AnneYasiyorMu { get; set; }
        public string AnneAd { get; set; } = string.Empty;
        public string AnneSoyad { get; set; } = string.Empty;
        public byte? AnneYas { get; set; }
        public string AnneEgitim { get; set; } = string.Empty;
        public string AnneIs { get; set; } = string.Empty;
        public string AnneTel { get; set; } = string.Empty;
        public string AnneAdres { get; set; } = string.Empty;

        public bool BabaYasiyorMu { get; set; }
        public string BabaAd { get; set; } = string.Empty;
        public string BabaSoyad { get; set; } = string.Empty;
        public byte? BabaYas { get; set; }
        public string BabaEgitim { get; set; } = string.Empty;
        public string BabaIs { get; set; } = string.Empty;
        public string BabaTel { get; set; } = string.Empty;
        public string BabaAdres { get; set; } = string.Empty;

        public bool UveyVeyaKoruyucuVarMi { get; set; }
        public string UveyEbeveynTuru { get; set; } = string.Empty; 
        public string UveyAd { get; set; } = string.Empty;
        public string UveySoyad { get; set; } = string.Empty;
        public byte? UveyYas { get; set; }
        public string UveyEgitim { get; set; } = string.Empty;
        public string UveyIs { get; set; } = string.Empty;
        public string UveyTel { get; set; } = string.Empty;
        public string UveyAdres { get; set; } = string.Empty;
        public bool? UveyYasiyorMu { get; set; }

        public bool AkrabaEvliligi { get; set; }
        public string AileTipi { get; set; } = string.Empty;
        public string EbeveynDurumu { get; set; } = string.Empty;
        public string Kardesler { get; set; } = string.Empty;
        public string AilePsikiyatrikOyku { get; set; } = string.Empty;
        public string AileTibbiOyku { get; set; } = string.Empty;
        public string AileEkNotlar { get; set; } = string.Empty;
    }
}
