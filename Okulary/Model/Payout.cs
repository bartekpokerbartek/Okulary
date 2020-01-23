using System;
using Okulary.Enums;

namespace Okulary.Model
{
    public class Payout
    {
        public Payout()
        {
        }

        public int PayoutId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedOn { get; set; }

        public Lokalizacja Lokalizacja { get; set; }
    }
}
