namespace OgrenciOdevYonetimSistemi.Models
{
    public class VerilenOdev
    {
        public string OgrenciNo { get; set; }
        public string Konu { get; set; }
        public DateTime TeslimTarihi { get; set; }

        // ✅ Bu satırı EKLE!
        public string DosyaAdi { get; set; }
    }
}
