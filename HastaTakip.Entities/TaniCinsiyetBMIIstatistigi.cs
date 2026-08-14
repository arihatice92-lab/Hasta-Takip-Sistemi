namespace HastaTakip.Entities
{
    public class TaniCinsiyetBMIIstatistigi
    {
        public string TaniAdi { get; set; } = string.Empty;
        public string Cinsiyet { get; set; } = string.Empty;
        public int HastaSayisi { get; set; }
        public decimal OrtalamaBMI { get; set; }
        public decimal MinBMI { get; set; }
        public decimal MaxBMI { get; set; }
    }
}