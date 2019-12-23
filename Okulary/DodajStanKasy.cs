using System;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajStanKasy : Form
    {
        private readonly Lokalizacja _lokalizacja;

        public DodajStanKasy(Lokalizacja lokalizacja)
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

            if (!decimal.TryParse(koszt, out decimal stanKasy))
            {
                MessageBox.Show("Koszt ma niewłaściwy format.");
                return;
            }

            if (_lokalizacja != Lokalizacja.Dynow && _lokalizacja != Lokalizacja.Dubiecko)
            {
                MessageBox.Show("Dodajesz stan kasy dla złej lokalizacji.");
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz zaktualizować stan kasy?", "Tak", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                using (var ctx = new MineContext())
                {
                    ctx.Kasa.Add(new MoneyCount()
                                        {
                                            Amount = stanKasy,
                                            CreatedOn = DateTime.Now,
                                            Lokalizacja = _lokalizacja
                    });

                    ctx.SaveChanges();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }

            Close();
        }
    }
}
