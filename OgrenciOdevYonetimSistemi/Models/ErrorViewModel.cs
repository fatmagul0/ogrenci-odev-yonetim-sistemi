namespace OgrenciOdevYonetimSistemi.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }//HTTP isteðine ait benzersi ID'yi tutar
        //Bu ID, hatanýn hangi istekte oluþtuðunu takip etmeye yarar.

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
        //Bu, bir "yalancý property" (sadece okunabilir).
        //Eðer RequestId boþ deðilse true döner.
        //View’da RequestId gösterilmeli mi diye kontrol etmek için kullanýlýr.
    }
}
//sadece hata sayfasý için vardýr.
