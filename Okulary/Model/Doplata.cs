using System;

namespace Okulary.Model
{
    public class Doplata
    {
        public Doplata()
        {
        }

        public int Binocle_BinocleId { get; set; }

        public int DoplataId { get; set; }

        public decimal Kwota { get; set; }

        public DateTime DataDoplaty { get; set; }
    }
}
