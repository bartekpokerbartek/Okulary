using System;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajStanKasy : Form
    {
        private readonly MoneyCountService _moneyCountService = new MoneyCountService();

        private readonly Lokalizacja _lokalizacja;

        public DodajStanKasy(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var koszt = textBox1.Text;

            if (!decimal.TryParse(koszt, out var stanKasy))
            {
                MessageBox.Show("Koszt ma niewłaściwy format.");
                return;
            }

            if (_lokalizacja != Lokalizacja.Dynow && _lokalizacja != Lokalizacja.Dubiecko)
            {
                MessageBox.Show("Dodajesz stan kasy dla złej lokalizacji.");
                return;
            }

            var dialogResult = MessageBox.Show("Czy na pewno chcesz zaktualizować stan kasy?", "Tak", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                var kasa = new MoneyCount
                               {
                                   Amount = stanKasy,
                                   CreatedOn = DateTime.Now,
                                   Lokalizacja = _lokalizacja
                               };

                await _moneyCountService.Create(kasa);
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }

            Close();
        }
    }
}
