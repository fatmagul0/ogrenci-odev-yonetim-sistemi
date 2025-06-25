namespace OgrenciOdevYonetimSistemi.Models
{
    public class OgrenciKota
    {
        public string OgrenciNo { get; set; }
        public string AdSoyad { get; set; }       // ✔️ Yeni
        public string Sinif { get; set; }         // ✔️ Yeni
        public int MaksimumOdevSayisi { get; set; }
        public int YapilanOdevSayisi { get; set; }
    }
}
