using System.Collections.Generic;

using iText.Layout.Element;

using Okulary.Enums;

namespace Okulary.Helpers
{
    public static class LokalizacjaHelper
    {
        public static string DajLokalizacje(Lokalizacja lokalizacja)
        {
            if (lokalizacja == Lokalizacja.Dynow)
                return "Dynów";

            if (lokalizacja == Lokalizacja.Dubiecko)
                return "Dubiecko";

            return "Wszystkie";
        }

        public static Lokalizacja DajLokalizacjaEnum(string lokaliacja)
        {
            if (lokaliacja == "Dynów")
                return Lokalizacja.Dynow;
            else if (lokaliacja == "Dubiecko")
                return Lokalizacja.Dubiecko;
            else return Lokalizacja.Nieznana;
        }

        public static List<Lokalizacja> DajDozwoloneLokalizacje(Lokalizacja lokalizacja)
        {
            List<Lokalizacja> lokalizacje;

            if (lokalizacja == Lokalizacja.Wszystkie)
                lokalizacje = new List<Lokalizacja>
                                  {
                                      Lokalizacja.Dynow,
                                      Lokalizacja.Dubiecko,
                                      Lokalizacja.Wszystkie
                                  };
            else
                lokalizacje = new List<Lokalizacja>
                                  {
                                      lokalizacja,
                                      Lokalizacja.Wszystkie
                                  };

            return lokalizacje;
        }
    }
}
