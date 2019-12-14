using System;

namespace Okulary.Model
{
    public class MoneyCount
    {
        public MoneyCount()
        {
        }

        public int MoneyCountId { get; set; }

        public decimal Amount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
