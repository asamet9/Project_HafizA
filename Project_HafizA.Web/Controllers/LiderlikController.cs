using Microsoft.AspNetCore.Mvc;
using Project_HafizA.Business;

namespace Project_HafizA.Web.Controllers
{
    public class LiderlikController : Controller
    {
        private readonly ILiderlikService _liderlikService;

        public LiderlikController(ILiderlikService liderlikService)
        {
            _liderlikService = liderlikService;
        }

        public IActionResult Index()
        {
            var liste = _liderlikService.EnIyileriGetir(20);
            return View(liste);
        }

        [HttpPost]
        public IActionResult SkorKaydet([FromBody] SkorDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.KullaniciAdi) || dto.Skor < 0)
                return BadRequest();

            _liderlikService.SkorKaydet(dto.KullaniciAdi, dto.Skor);
            return Ok();
        }
    }

    public class SkorDto
    {
        public string KullaniciAdi { get; set; }
        public int Skor { get; set; }
    }
}