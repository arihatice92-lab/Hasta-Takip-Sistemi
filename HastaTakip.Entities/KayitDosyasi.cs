namespace HastaTakip.Entities
{
    public class KayitDosyasi
    {
        public int DosyaID { get; set; }
        public string KayitTuru { get; set; } = string.Empty;
        public int KayitID { get; set; }
        public string DosyaAdi { get; set; } = string.Empty;
        public string DosyaYolu { get; set; } = string.Empty;
        public string DosyaTipi { get; set; } = string.Empty;
        public int YukleyenKullaniciID { get; set; }
        public DateTime YuklemeTarihi { get; set; }
    }
}