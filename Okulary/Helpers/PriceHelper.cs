using Okulary.Model;

namespace Okulary.Helpers
{
    public class PriceHelper
    {
        public decimal DajSume(Binocle _zakup)
        {
            return _zakup.Robocizna + _zakup.DalOP.Cena + _zakup.DalOL.Cena + _zakup.BlizOP.Cena +
                _zakup.BlizOL.Cena + _zakup.CenaOprawekBliz + _zakup.CenaOprawekDal - _zakup.Refundacja;
        }

        public decimal DajDoZaplaty(Binocle _zakup)
        {
            return DajSume(_zakup) - _zakup.Zadatek;
        }
    }
}
