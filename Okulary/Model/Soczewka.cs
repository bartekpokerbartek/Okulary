using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okulary.Model
{
    public class Soczewka
    {
        public Soczewka()
        {
        }

        public decimal Sfera { get; set; }

        public decimal Cylinder { get; set; }

        public decimal Os { get; set; }

        public string Pryzma { get; set; }

        public decimal OdlegloscZrenic { get; set; }

        public string H { get; set; }

        public decimal Cena { get; set; }
    }
}
