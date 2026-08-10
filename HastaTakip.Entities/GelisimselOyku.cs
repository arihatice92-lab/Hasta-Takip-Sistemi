using System;
using System.Collections.Generic;
using System.Text;

namespace HastaTakip.Entities
{
    public class GelisimselOyku
    {
        public int GelisimOykuID { get; set; }
        public string HastaTC { get; set; } = string.Empty;

        public byte? DogumAnneYasi { get; set; }
        public byte? DogumBabaYasi { get; set; }
        public byte? DogumHaftasi { get; set; }
        public string? DogumSekli { get; set; } 
        public string? DogumKomplikasyonu { get; set; } 
        public short? DogumAgirligi { get; set; }
        public bool? PlanliGebelikMi { get; set; }
        public string? GebeKalmadaGucluk { get; set; } 
        public string? AileCinsiyetBeklentisi { get; set; }
        public string? AileDogumaTepki { get; set; } 

        public byte? OturmaYasi { get; set; }
        public byte? EmeklemeYasi { get; set; }
        public byte? YurumeYasi { get; set; }
        public byte? IlkSozcukYasi { get; set; }
        public byte? IlkCumleYasi { get; set; }
        public string? TuvaletEgitimi { get; set; } 
        public string? GecirilenKaza { get; set; }

        public string? BebeklikDonemi { get; set; } 
        public string? CocuklukDonemi { get; set; } 
        public string? OkulOykusu { get; set; } 
        public string? SosyalIliskileri { get; set; } 
        public string? KisilikOzellikleri { get; set; } 
        public string? GelisimselOykuEkNot { get; set; } 
        public int? SonGuncelleyenKullaniciID { get; set; }
        public DateTime? SonGuncellemeTarihi { get; set; }
        public int? OlusturanKullaniciID { get; set; }
        public DateTime? OlusturmaTarihi { get; set; }
    }
}
