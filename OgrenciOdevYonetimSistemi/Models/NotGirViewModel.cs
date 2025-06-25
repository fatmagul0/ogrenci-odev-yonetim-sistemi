namespace OgrenciOdevYonetimSistemi.Models
{
    // BU SINIF,öğretmenin bir öğrenciye not girmesi için kullanılan ViewModeldir.
    public class NotGirViewModel
    {
        public int OgrenciId { get; set; }
        public string AdSoyad { get; set; }
        public int? Vize { get; set; }
        public int? Final { get; set; }
        public int? Proje { get; set; }
    }
}
//Bu sınıf, öğretmenin bir öğrenciye not girdiği ekranda kullanılır.
//Öğrencinin adı gösterilir,
//Not alanları doldurulur,
//Arka tarafa OgrenciId gönderilir.
//Veritabanı ile doğrudan ilişkili değildir, sadece geçici veri taşımak için vardır.
//MVC'de bu tip ViewModel’ler sayfa özel ihtiyaçlarını karşılamak için kullanılır.
