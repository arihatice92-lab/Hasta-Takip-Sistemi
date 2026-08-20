namespace HastaTakip.Entities
{
    public class PsikologIzni
    {
        public int IzinID { get; set; }
        public byte PsikologID { get; set; }
        public string IzinTuru { get; set; } = string.Empty;
        public DateTime BaslangicTarihi { get; set; }
        public DateTime BitisTarihi { get; set; }
        public string? Aciklama { get; set; }
        public int? EkleyenKullaniciID { get; set; }
        public DateTime EklemeTarihi { get; set; }
    }
}