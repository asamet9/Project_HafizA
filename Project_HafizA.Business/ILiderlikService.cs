using Project_HafizA.Core;

namespace Project_HafizA.Business
{
    public interface ILiderlikService
    {
        void SkorKaydet(string kullaniciAdi, int skor);
        List<Liderlik> EnIyileriGetir(int kac = 10);
    }
}