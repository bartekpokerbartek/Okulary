using System;
using System.Windows.Forms;
using Okulary.Enums;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajDoplate : Form
    {
        //private readonly DoplataService _doplataService = new DoplataService();

        private int _binocleId;

        public Doplata Doplata;

        public DodajDoplate(int binocleId)
        {
            InitializeComponent();
            _binocleId = binocleId;
            comboBox1.DataSource = Enum.GetValues(typeof(FormaPlatnosci));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var koszt = textBox1.Text;
            var dataSprzedazy = dateTimePicker1.Value.Date + dateTimePicker2.Value.TimeOfDay;

            if (string.IsNullOrEmpty(koszt))
            {
                MessageBox.Show("Pole koszt nie może być puste");
                return;
            }

            if (!decimal.TryParse(koszt, out var cenaResult))
            {
                MessageBox.Show("Koszt ma niewłaściwy format.");
                return;
            }

            if (dataSprzedazy.Date != DateTime.Today.Date)
            {
                DialogResult dialogResult = MessageBox.Show("Data dopłaty nie jest datą dzisiejszą. Czy na pewno chcesz dodać dopłatę w tej dacie?", "Dodaj", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            Enum.TryParse(comboBox1.SelectedValue.ToString(), out FormaPlatnosci formaPlatnosci);

            Doplata = new Doplata
                              {
                                  DataDoplaty = dataSprzedazy,
                                  Kwota = cenaResult,
                                  //Binocle_BinocleId = _binocleId,
                                  FormaPlatnosci = formaPlatnosci
                              };

            //_doplataService.Create(doplata);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
