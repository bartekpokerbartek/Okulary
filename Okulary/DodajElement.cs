using System;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajElement : Form
    {
        private readonly ElementsService _elementService = new ElementsService();

        private Lokalizacja _lokalizacja;

        public DodajElement(Lokalizacja lokalizacja, DateTime dateSelector)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;

            dateTimePicker1.Value = dateSelector;
            dateTimePicker2.Value = DateTime.Now;
            label6.Text = LokalizacjaHelper.DajLokalizacje(lokalizacja);
            comboBox1.DataSource = Enum.GetValues(typeof(FormaPlatnosci));
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            var nazwa = textBox1.Text;
            var ilosc = textBox2.Text;
            var cena = textBox3.Text;

            var dataSprzedazy = dateTimePicker1.Value.Date + dateTimePicker2.Value.TimeOfDay;

            if (string.IsNullOrEmpty(nazwa) || string.IsNullOrEmpty(ilosc) || string.IsNullOrEmpty(cena))
            {
                MessageBox.Show("Pola nazwa, ilość i cena nie mogą być puste");
                return;
            }

            if (!int.TryParse(ilosc, out var iloscResult))
            {
                MessageBox.Show("Ilość musi być liczbą całkowitą.");
                return;
            }

            if (iloscResult <= 0)
            {
                MessageBox.Show("Ilość musi być większa od zera.");
                return;
            }

            if (!decimal.TryParse(cena, out var cenaResult))
            {
                MessageBox.Show("Cena ma niewłaściwy format.");
                return;
            }

            if (cenaResult <= 0)
            {
                MessageBox.Show("Cena musi być większa od zera.");
                return;
            }

            if (dataSprzedazy.Date != DateTime.Today.Date)
            {
                var dialogResult = MessageBox.Show("Data sprzedaży nie jest datą dzisiejszą. Czy na pewno chcesz dodać sprzedaż w tej dacie?", "Dodaj", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            Enum.TryParse(comboBox1.SelectedValue.ToString(), out FormaPlatnosci formaPlatnosci);

            var element = new Element
                              {
                                  Cena = cenaResult,
                                  Ilosc = iloscResult,
                                  Nazwa = nazwa,
                                  DataSprzedazy = dataSprzedazy,
                                  DataUtworzenia = DateTime.Now,
                                  Lokalizacja = _lokalizacja,
                                  FormaPlatnosci = formaPlatnosci
                              };

            await _elementService.Create(element);

            Close();
        }
    }
}
