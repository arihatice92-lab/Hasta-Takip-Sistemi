namespace HastaTakip.Entities
{
    public class DoktorIzni
    {
        public int IzinID { get; set; }
        public short DoktorID { get; set; }
        public string IzinTuru { get; set; } = string.Empty;
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? Aciklama { get; set; }
        public int? EkleyenKullaniciID { get; set; }
        public DateTime EklemeTarihi { get; set; }
    }
}