using System;
using System.Configuration;

using Okulary.Model;

namespace Okulary.Helpers
{
    public class OrderHelper
    {
        private readonly DateTime _dataOdbioru = DateTime.Parse(ConfigurationManager.AppSettings["DataOdbioru"]);

        public bool CzyOdebrany(Binocle zakup)
        {
            return zakup.IsDataOdbioru || zakup.BuyDate <= _dataOdbioru;
        }
    }
}
