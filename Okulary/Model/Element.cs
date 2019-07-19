using System;

using Okulary.Enums;

namespace Okulary.Model
{
    public class Element
    {
        public Element()
        {
        }

        public int ElementId { get; set; }

        public string Nazwa { get; set; }

        public int Ilosc { get; set; }

        public decimal Cena { get; set; }

        public DateTime DataSprzedazy { get; set; }

        public DateTime DataUtworzenia { get; set; }

        public Lokalizacja Lokalizacja { get; set; }

        public bool CannotEdit { get; set; }

        public FormaPlatnosci FormaPlatnosci { get; set; }
    }
}
