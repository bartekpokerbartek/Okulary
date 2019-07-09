using Okulary.Enums;

namespace Okulary.Helpers
{
    public static class LokalizacjaHelper
    {
        public static string DajLokalizacje(Lokalizacja lokalizacja)
        {
            if (lokalizacja == Lokalizacja.Dynow)
                return "Dynów";
            else if (lokalizacja == Lokalizacja.Dubiecko)
                return "Dubiecko";
            else return "Wszystkie";
        }

        public static Lokalizacja DajLokalizacjaEnum(string lokaliacja)
        {
            if (lokaliacja == "Dynów")
                return Lokalizacja.Dynow;
            else if (lokaliacja == "Dubiecko")
                return Lokalizacja.Dubiecko;
            else return Lokalizacja.Nieznana;

        }
    }
}
