using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using OgrenciOdevYonetimSistemi.Models;
using System.Linq;
using OgrenciOdevYonetimSistemi.Filters;

namespace OgrenciOdevYonetimSistemi.Controllers
{
    [LoginCheck] // ✅ Giriş kontrol filtresi
    public class OgrenciController : Controller
    {
        private readonly UygulamaDbContext _context;

        public OgrenciController(UygulamaDbContext context)
        {
            _context = context;
        }

        // 🏠 PANEL SAYFASI
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Panel()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");

            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            ViewBag.AdSoyad = HttpContext.Session.GetString("AdSoyad");
            ViewBag.OkulNo = okulNo;

            return View();
        }

        // 📘 ÖDEVLERİM
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Odevlerim()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");
            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == okulNo);
            if (ogrenci == null)
                return RedirectToAction("OgrenciLogin", "Account");

            var odevler = _context.OdevTeslimler
                            .Where(o => o.OgrenciId == ogrenci.OgrenciId)
                            .ToList();

            return View(odevler);
        }

        // 📝 NOTLARIM
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult Notlarim()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");
            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == okulNo);
            if (ogrenci == null)
                return RedirectToAction("OgrenciLogin", "Account");

            var notlar = _context.OgrenciNotlari
                                 .Where(n => n.OgrenciId == ogrenci.OgrenciId)
                                 .Include(n => n.Ogretmen)
                                 .ToList();

            var model = new List<OgrenciNotViewModel>();
            foreach (var not in notlar)
            {
                var values = new List<int?> { not.Vize, not.Final, not.Proje }
                                .Where(n => n.HasValue)
                                .Select(n => n.Value);

                double? ortalama = values.Any() ? Math.Round(values.Average(), 2) : null;
                string durum = "Yetersiz";
                string renk = "danger";

                if (ortalama >= 85) { durum = "Pekiyi"; renk = "success"; }
                else if (ortalama >= 70) { durum = "İyi"; renk = "info"; }
                else if (ortalama >= 50) { durum = "Orta"; renk = "warning"; }

                model.Add(new OgrenciNotViewModel
                {
                    OgretmenAd = not.Ogretmen.AdSoyad,
                    Ders = not.Ogretmen.Brans,
                    Vize = not.Vize,
                    Final = not.Final,
                    Proje = not.Proje,
                    Ortalama = ortalama,
                    BasariDurumu = durum,
                    Renk = renk
                });
            }

            return View(model);
        }

        // 📈 KOTA BİLGİM
        [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
        public IActionResult KotaBilgim()
        {
            var okulNo = HttpContext.Session.GetString("OgrenciNo");
            if (string.IsNullOrEmpty(okulNo))
                return RedirectToAction("OgrenciLogin", "Account");

            var ogrenci = _context.Ogrenciler.FirstOrDefault(o => o.OkulNo == okulNo);
            if (ogrenci == null)
                return RedirectToAction("OgrenciLogin", "Account");

            var yapilanOdevSayisi = _context.OdevTeslimler.Count(x => x.OgrenciId == ogrenci.OgrenciId);

            var model = new OgrenciKota
            {
                OgrenciNo = ogrenci.OkulNo,
                AdSoyad = ogrenci.AdSoyad,
                Sinif = ogrenci.Sinif,
                MaksimumOdevSayisi = 5,
                YapilanOdevSayisi = yapilanOdevSayisi
            };

            return View(model);
        }
    }
}
