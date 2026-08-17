namespace HastaTakip.Entities
{
    public class RandevuCakismasi
    {
        public int RandevuTarihID { get; set; }
        public DateTime RandevuTarih { get; set; }
        public TimeSpan Saat { get; set; }
        public string HastaAdSoyad { get; set; } = string.Empty;
    }
}