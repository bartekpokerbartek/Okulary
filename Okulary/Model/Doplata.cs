using System;
using System.ComponentModel.DataAnnotations.Schema;

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

        [ForeignKey(nameof(Binocle_BinocleId))]
        public Binocle Binocle { get; set; }
    }
}
