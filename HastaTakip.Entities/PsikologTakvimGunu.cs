
namespace HastaTakip.Entities
{
    public class PsikologTakvimGunu
    {
        public DateTime Tarih { get; set; }
        public int ToplamSaat { get; set; }
        public int DoluSaat { get; set; }
        public bool TamamenMusait => DoluSaat < ToplamSaat;
        public bool IzinliMi { get; set; }
    }
}