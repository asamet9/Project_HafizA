using Microsoft.AspNetCore.Mvc;
using Project_HafizA.Business;

namespace Project_HafizA.Web.Controllers
    {
        public class SesController : Controller
        {
            private readonly ISesService _sesService;
            private readonly IAyetService _ayetService;

            public SesController(ISesService sesService, IAyetService ayetService)
            {
                _sesService = sesService;
                _ayetService = ayetService;
            }

        [HttpPost]
        public async Task<IActionResult> KontrolEt(IFormFile sesData, int ayetKodu, string zorluk = "orta")
        {
            try
            {
                if (sesData == null || sesData.Length == 0)
                    return Json(new { hata = "Ses verisi alınamadı." });

                using var ms = new MemoryStream();
                await sesData.CopyToAsync(ms);
                var sesBytes = ms.ToArray();

                var uzanti = Path.GetExtension(sesData.FileName).TrimStart('.').ToLower();
                if (string.IsNullOrEmpty(uzanti)) uzanti = "webm";

                var okunan = await _sesService.SesiMetneÇevir(sesBytes, uzanti);

                if (string.IsNullOrWhiteSpace(okunan))
                    return Json(new { hata = "Ses metne çevrilemedi." });
                var dogru = _ayetService.AyetDogruMu(ayetKodu, okunan, zorluk);
                var kelimeler = _ayetService.KelimeKarsilastir(ayetKodu, okunan);
                return Json(new { dogru, okunan, kelimeler });
            }
            catch (Exception ex)
            {
                return Json(new { hata = ex.Message + " | " + ex.InnerException?.Message });
            }
        }
    }
    }