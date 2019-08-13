using Okulary.Model;
using Okulary.Repo;
using System;
using System.Configuration;
using System.Linq;

namespace Okulary.Helpers
{
    public class PriceHelper
    {
        private readonly DateTime _dataNiezbalansowani = DateTime.Parse(ConfigurationManager.AppSettings["DataNiezbalansowani"].ToString());

        public decimal DajSume(Binocle _zakup)
        {
            return _zakup.Robocizna + _zakup.DalOP.Cena + _zakup.DalOL.Cena + _zakup.BlizOP.Cena +
                _zakup.BlizOL.Cena + _zakup.CenaOprawekBliz + _zakup.CenaOprawekDal - _zakup.Refundacja;
        }

        public decimal DajDoZaplaty(Binocle _zakup)
        {
            decimal doplaty;

            var help = _zakup.Doplaty;

            if (help.Any())
                doplaty = help.Sum(x => x.Kwota);
            else
                doplaty = 0.0M;

            return DajSume(_zakup) - _zakup.Zadatek - doplaty;
        }

        public bool CzyZbalansowany(Binocle zakup)
        {
            return DajDoZaplaty(zakup) != 0M && zakup.BuyDate >= _dataNiezbalansowani;
        }
    }
}
