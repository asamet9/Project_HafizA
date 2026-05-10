using Project_HafizA.Core;
using Project_HafizA.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Project_HafizA.Business
{
    public class AyetService : IAyetService
    {
        private readonly AppDbContext _context;

        public AyetService(AppDbContext context)
        {
            _context = context;
        }

        public List<Main> GetAll()
        {
            return _context.Main.ToList();
        }

        public Main GetRandomAyet()
        {
            return _context.Main
                .FromSqlRaw("SELECT TOP 1 * FROM Main ORDER BY NEWID()")
                .FirstOrDefault();
        }

        public bool AyetDogruMu(int ayetKodu, string okunan, string zorluk = "orta")
        {
            var ayet = _context.Main.FirstOrDefault(a => a.Ayet_kodu == ayetKodu);
            if (ayet == null || string.IsNullOrWhiteSpace(okunan)) return false;

            string ayetTemiz = HarekeleriTemizle(ayet.Metin);
            string okunanTemiz = HarekeleriTemizle(okunan);
            double benzerlik = BenzerlikHesapla(ayetTemiz, okunanTemiz);

            double esik = zorluk switch
            {
                "kolay" => 0.50,
                "orta" => 0.65,
                "hafizpromax" => 0.90,
                _ => 0.65
            };

            return benzerlik >= esik;
        }
        private string HarekeleriTemizle(string metin)
        {
            if (string.IsNullOrEmpty(metin)) return "";

            // Arapça hareke unicode aralığı: U+064B - U+065F
            var sonuc = new System.Text.StringBuilder();
            foreach (char c in metin)
            {
                if (c >= 0x064B && c <= 0x065F) continue; // harekeleri atla
                if (c == 0x0670) continue; // elif maksure
                if (c == '\u200C' || c == '\u200D') continue; // zero-width chars
                sonuc.Append(c);
            }

            return sonuc.ToString().Trim();
        }

        private double BenzerlikHesapla(string kaynak, string hedef)
        {
            if (string.IsNullOrEmpty(kaynak) || string.IsNullOrEmpty(hedef)) return 0;

            kaynak = kaynak.Trim();
            hedef = hedef.Trim();

            int uzunluk = Math.Max(kaynak.Length, hedef.Length);
            if (uzunluk == 0) return 1.0;

            int mesafe = LevenshteinMesafesi(kaynak, hedef);
            return 1.0 - (double)mesafe / uzunluk;
        }

        private int LevenshteinMesafesi(string a, string b)
        {
            int[,] dp = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++) dp[i, 0] = i;
            for (int j = 0; j <= b.Length; j++) dp[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int maliyet = a[i - 1] == b[j - 1] ? 0 : 1;
                    dp[i, j] = Math.Min(
                        Math.Min(dp[i - 1, j] + 1, dp[i, j - 1] + 1),
                        dp[i - 1, j - 1] + maliyet
                    );
                }
            }

            return dp[a.Length, b.Length];
        }

        public List<KelimeSonuc> KelimeKarsilastir(int ayetKodu, string okunan)
        {
            var ayet = _context.Main.FirstOrDefault(a => a.Ayet_kodu == ayetKodu);
            if (ayet == null) return new List<KelimeSonuc>();

            var ayetKelimeler = HarekeleriTemizle(ayet.Metin)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var okunanKelimeler = HarekeleriTemizle(okunan)
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            var sonuc = new List<KelimeSonuc>();

            for (int i = 0; i < ayetKelimeler.Length; i++)
            {
                string ayetKelime = ayetKelimeler[i];
                bool dogru = false;

                if (i < okunanKelimeler.Length)
                {
                    double benzerlik = BenzerlikHesapla(ayetKelime, okunanKelimeler[i]);
                    dogru = benzerlik >= 0.70;
                }

                sonuc.Add(new KelimeSonuc { Kelime = ayet.Metin.Split(' ', StringSplitOptions.RemoveEmptyEntries)[i], Dogru = dogru });
            }

            return sonuc;
        }



    }
}