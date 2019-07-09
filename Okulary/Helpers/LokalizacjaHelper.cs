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
            else return "Obydwie";
        }
    }
}
