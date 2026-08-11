namespace HastaTakip.Entities
{
    public class DoktorTakvimGunu
    {
        public DateTime Tarih { get; set; }
        public int ToplamSaat { get; set; }
        public int DoluSaat { get; set; }
        public bool TamamenMusait => DoluSaat < ToplamSaat;
    }
}
