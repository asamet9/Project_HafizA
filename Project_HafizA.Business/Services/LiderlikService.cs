using Project_HafizA.Core;
using Project_HafizA.DataAccess;

namespace Project_HafizA.Business
{
    public class LiderlikService : ILiderlikService
    {
        private readonly AppDbContext _context;

        public LiderlikService(AppDbContext context)
        {
            _context = context;
        }

        public void SkorKaydet(string kullaniciAdi, int skor)
        {
            _context.Liderlik.Add(new Liderlik
            {
                KullaniciAdi = kullaniciAdi,
                Skor = skor,
                Tarih = DateTime.Now
            });
            _context.SaveChanges();
        }

        public List<Liderlik> EnIyileriGetir(int kac = 10)
        {
            return _context.Liderlik
                .OrderByDescending(l => l.Skor)
                .Take(kac)
                .ToList();
        }
    }
}