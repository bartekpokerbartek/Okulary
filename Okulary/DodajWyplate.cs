using System;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajWyplate : Form
    {
        private readonly PayoutService _payoutService = new PayoutService();

        private readonly Lokalizacja _lokalizacja;

        public DodajWyplate(Lokalizacja lokalizacja)
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
            var dataWyplaty = dateTimePicker1.Value.Date + dateTimePicker2.Value.TimeOfDay;

            if (string.IsNullOrEmpty(koszt))
            {
                MessageBox.Show("Pole kwota nie może być puste.");
                return;
            }

            var opis = textBox2.Text;

            if (string.IsNullOrEmpty(opis))
            {
                MessageBox.Show("Pole opis nie może być puste.");
                return;
            }

            if (!decimal.TryParse(koszt, out var cenaResult))
            {
                MessageBox.Show("Kwota ma niewłaściwy format.");
                return;
            }

            if (dataWyplaty.Date != DateTime.Today.Date)
            {
                var dialogResult = MessageBox.Show("Data wypłaty nie jest datą dzisiejszą. Czy na pewno chcesz dodać wypłatę w tej dacie?", "Dodaj", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            var payout = new Payout
                             {
                                 CreatedOn = dataWyplaty,
                                 Amount = cenaResult,
                                 Lokalizacja = _lokalizacja,
                                 Description = opis
                             };

            await _payoutService.Create(payout);

            Close();
        }
    }
}
