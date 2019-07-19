using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

using Okulary.Enums;

namespace Okulary.Model
{
    public class Binocle
    {
        public Binocle()
        {
            Doplaty = new List<Doplata>();
        }

        public int BinocleId { get; set; }

        public string Description { get; set; }

        public DateTime BuyDate { get; set; }

        public int Person_PersonId { get; set; }

        public string NumerZlecenia { get; set; }

        public DateTime DataOdbioru { get; set; }

        public bool IsDataOdbioru { get; set; }

        public decimal Zadatek { get; set; }

        public string RodzajOprawekDal { get; set; }

        public decimal CenaOprawekDal { get; set; }

        public string RodzajOprawekBliz { get; set; }

        public decimal CenaOprawekBliz { get; set; }

        public decimal Robocizna { get; set; }

        public Soczewka DalOP { get; set; }

        public Soczewka DalOL { get; set; }

        public Soczewka BlizOP { get; set; }

        public Soczewka BlizOL { get; set; }

        public string RodzajSoczewek1 { get; set; }

        public string RodzajSoczewek2 { get; set; }

        public decimal Refundacja { get; set; }

        public List<Doplata> Doplaty { get; set; }

        [ForeignKey(nameof(Person_PersonId))]
        public Person Person { get; set; }

        public FormaPlatnosci FormaPlatnosci { get; set; }
    }
}
