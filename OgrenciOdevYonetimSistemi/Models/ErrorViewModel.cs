namespace OgrenciOdevYonetimSistemi.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }//HTTP iste�ine ait benzersi ID'yi tutar
        //Bu ID, hatan�n hangi istekte olu�tu�unu takip etmeye yarar.

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        //Bu, bir "yalanc� property" (sadece okunabilir).
        //E�er RequestId bo� de�ilse true d�ner.
        //View�da RequestId g�sterilmeli mi diye kontrol etmek i�in kullan�l�r.
    }
}
//sadece hata sayfas� i�in vard�r.
