using System;
using System.Windows.Forms;

using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajStanKasy : Form
    {
        public DodajStanKasy()
        {
            InitializeComponent();
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

            DialogResult dialogResult = MessageBox.Show("Czy na pewno chcesz zaktualizować stan kasy?", "Tak", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                using (var ctx = new MineContext())
                {
                    ctx.Kasa.Add(new MoneyCount()
                                        {
                                            Amount = stanKasy,
                                            CreatedOn = DateTime.Now
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
