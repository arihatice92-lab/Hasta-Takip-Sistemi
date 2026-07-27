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
        public string DogumSekli { get; set; } = string.Empty;
        public string DogumKomplikasyonu { get; set; } = string.Empty;
        public short? DogumAgirligi { get; set; }
        public bool? PlanliGebelikMi { get; set; }
        public string GebeKalmadaGucluk { get; set; } = string.Empty;
        public string AileCinsiyetBeklentisi { get; set; } = string.Empty;
        public string AileDogumaTepki { get; set; } = string.Empty;

        public byte? OturmaYasi { get; set; }
        public byte? EmeklemeYasi { get; set; }
        public byte? YurumeYasi { get; set; }
        public byte? IlkSozcukYasi { get; set; }
        public byte? IlkCumleYasi { get; set; }
        public string TuvaletEgitimi { get; set; } = string.Empty;
        public string GecirilenKaza { get; set; } = string.Empty;

        public string BebeklikDonemi { get; set; } = string.Empty;
        public string CocuklukDonemi { get; set; } = string.Empty;
        public string OkulOykusu { get; set; } = string.Empty;
        public string SosyalIliskileri { get; set; } = string.Empty;
        public string KisilikOzellikleri { get; set; } = string.Empty;
        public string GelisimselOykuEkNot { get; set; } = string.Empty;
    }
}
