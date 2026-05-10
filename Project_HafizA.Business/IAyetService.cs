    using Project_HafizA.Core;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Project_HafizA.Business
    {
        public interface IAyetService
        {
        List<Main> GetAll();
        Main GetRandomAyet();
        bool AyetDogruMu(int ayetKodu, string okunan, string zorluk = "orta");
        List<KelimeSonuc> KelimeKarsilastir(int ayetKodu, string okunan);



         }
    
    }
