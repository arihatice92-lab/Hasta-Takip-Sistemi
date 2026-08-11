
namespace HastaTakip.Entities
{
    public class PsikologRandevuSaat
    {
        public byte SaatID { get; set; }
        public TimeSpan RandevuBaslangicSaat { get; set; }
        public TimeSpan RandevuBitisSaat { get; set; }
    }
}