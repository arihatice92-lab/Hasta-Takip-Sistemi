
namespace HastaTakip.Entities
{
    public class PsikologRandevuTarihi
    {
        public int RandevuTarihID { get; set; }
        public string HastaTC { get; set; } = string.Empty;
        public byte PsikologID { get; set; }
        public byte SaatID { get; set; }
        public DateTime RandevuTarih { get; set; }
        public DateTime RandevuOlusturmaTarihi { get; set; }
        public string RandevuDurum { get; set; } = string.Empty;
        public DateTime? HastaGelisZamani { get; set; }
        public DateTime? TestBaslangicZamani { get; set; }
    }
}