using Okulary.Model;
using Okulary.Repo;
using System.Linq;

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
            decimal doplaty;
            using (var _context = new MineContext())
            {
                var help = _context.Doplaty.Where(x => x.Binocle_BinocleId == _zakup.BinocleId);

                if (help.Any())
                    doplaty = help.Sum(x => x.Kwota);
                else
                    doplaty = 0.0M;
            }

            return DajSume(_zakup) - _zakup.Zadatek - doplaty;
        }
    }
}
