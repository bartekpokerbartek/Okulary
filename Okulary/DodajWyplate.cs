using System;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajWyplate : Form
    {
        private Lokalizacja _lokalizacja;

        public DodajWyplate(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var koszt = textBox1.Text;
            var dataWyplaty = dateTimePicker1.Value.Date + dateTimePicker2.Value.TimeOfDay;

            if (string.IsNullOrEmpty(koszt))
            {
                MessageBox.Show("Pole kwota nie może być puste");
                return;
            }

            decimal cenaResult;
            if (!decimal.TryParse(koszt, out cenaResult))
            {
                MessageBox.Show("Kwota ma niewłaściwy format.");
                return;
            }

            if (dataWyplaty.Date != DateTime.Today.Date)
            {
                DialogResult dialogResult = MessageBox.Show("Data wypłaty nie jest datą dzisiejszą. Czy na pewno chcesz dodać wypłatę w tej dacie?", "Dodaj", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            using (var ctx = new MineContext())
            {
                ctx.Wyplaty.Add(new Payout
                                    {
                                        CreatedOn = dataWyplaty,
                                        Amount = cenaResult,
                                        Lokalizacja = _lokalizacja,
                                        Description = textBox2.Text
                });
                ctx.SaveChanges();
            }

            this.Close();
        }
    }
}
