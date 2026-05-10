using Microsoft.AspNetCore.Mvc;
using Project_HafizA.Business;
using Project_HafizA.Core;

namespace Project_HafizA.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAyetService _service;

        public HomeController(IAyetService service)
        {
            _service = service;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult RastgeleAyet()
        {
            var ayet = _service.GetRandomAyet();
            return View("Index", ayet);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Antrenman(string mod, string kullaniciAdi = "", int sure = 0, string zorluk = "orta")
        {
            var ayet = _service.GetRandomAyet();
            ViewBag.Mod = mod;
            ViewBag.KullaniciAdi = kullaniciAdi;
            ViewBag.Sure = sure;
            ViewBag.Zorluk = zorluk;
            return View(ayet);
        }

        public IActionResult RastgeleAyetJson()
        {
            var ayet = _service.GetRandomAyet();
            return Json(new
            {
                ayet_kodu = ayet.Ayet_kodu,
                metin = ayet.Metin,
                sure_kodu = ayet.Sure_kodu,
                ayet_no = ayet.Ayet_no
            });
        }

        public IActionResult Rekabetci(string kullaniciAdi)
        {
            var ayet = _service.GetRandomAyet();
            ViewBag.KullaniciAdi = kullaniciAdi;
            return View(ayet);
        }
    }
}