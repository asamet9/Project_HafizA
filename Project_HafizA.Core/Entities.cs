using System.ComponentModel.DataAnnotations;

namespace Project_HafizA.Core
{
    public class Main
    {
        [Key]
        public int Ayet_kodu { get; set; }

        public int Sure_kodu { get; set; }
        public int Ayet_no { get; set; }
        public string Metin { get; set; }
        public bool Secde { get; set; }
        public bool Uzun { get; set; }
    }
}