using Okulary.Repo;
using Okulary.Model;
using System;
using System.Windows.Forms;

namespace Okulary
{
    public partial class DodajDoplate : Form
    {
        private int _binocleId;

        public DodajDoplate(int binocleId)
        {
            InitializeComponent();
            _binocleId = binocleId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var koszt = textBox1.Text;
            var dataSprzedazy = dateTimePicker1.Value.Date;

            if (string.IsNullOrEmpty(koszt))
            {
                MessageBox.Show("Pole koszt nie może być puste");
                return;
            }

            decimal cenaResult;
            if (!decimal.TryParse(koszt, out cenaResult))
            {
                MessageBox.Show("Koszt ma niewłaściwy format.");
                return;
            }

            if (dataSprzedazy != DateTime.Today.Date)
            {
                DialogResult dialogResult = MessageBox.Show("Data dopłaty nie jest datą dzisiejszą. Czy na pewno chcesz dodać dopłatę w tej dacie?", "Dodaj", MessageBoxButtons.YesNo);
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
                ctx.Doplaty.Add(new Doplata {
                    DataDoplaty = dataSprzedazy.Date,
                    Kwota = cenaResult,
                    Binocle_BinocleId = _binocleId
                });
                ctx.SaveChanges();
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
