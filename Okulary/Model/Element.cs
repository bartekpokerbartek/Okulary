using System;

namespace Okulary.Model
{
    public class Element
    {
        public Element()
        {
        }

        public string Nazwa { get; set; }

        public int Ilosc { get; set; }

        public decimal Cena { get; set; }

        public DateTime DataSprzedazy { get; set; }

        public DateTime DataUtworzenia { get; set; }
    }
}
