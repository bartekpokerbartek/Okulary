﻿using System;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class DodajElement : Form
    {
        private Lokalizacja _lokalizacja;

        private DateTime _dateSelector;

        public DodajElement(Lokalizacja lokalizacja, DateTime dateSelector)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;
            _dateSelector = dateSelector;
            dateTimePicker1.Value = dateSelector;
            dateTimePicker2.Value = DateTime.Now;
            label6.Text = LokalizacjaHelper.DajLokalizacje(lokalizacja);
            comboBox1.DataSource = Enum.GetValues(typeof(FormaPlatnosci));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //button1.Enabled = false;
            //button2.Enabled = false;

            var nazwa = textBox1.Text;
            var ilosc = textBox2.Text;
            var cena = textBox3.Text;
            var dataSprzedazy = dateTimePicker1.Value.Date + dateTimePicker2.Value.TimeOfDay;

            if (string.IsNullOrEmpty(nazwa) || string.IsNullOrEmpty(ilosc) || string.IsNullOrEmpty(cena))
            {
                MessageBox.Show("Pola nazwa, ilość i cena nie mogą być puste");
                return;
            }

            int iloscResult;
            if (!int.TryParse(ilosc, out iloscResult))
            {
                MessageBox.Show("Ilość musi być liczbą całkowitą.");
                return;
            }

            if (iloscResult <= 0)
            {
                MessageBox.Show("Ilość musi być większa od zera.");
                return;
            }

            decimal cenaResult;
            if (!decimal.TryParse(cena, out cenaResult))
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
                DialogResult dialogResult = MessageBox.Show("Data sprzedaży nie jest datą dzisiejszą. Czy na pewno chcesz dodać sprzedaż w tej dacie?", "Dodaj", MessageBoxButtons.YesNo);
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
                FormaPlatnosci formaPlatnosci;
                Enum.TryParse(comboBox1.SelectedValue.ToString(), out formaPlatnosci);

                ctx.Elements.Add(new Element
                                     {
                                         Cena = cenaResult,
                                         Ilosc = iloscResult,
                                         Nazwa = nazwa,
                                         DataSprzedazy = dataSprzedazy,
                                         DataUtworzenia = DateTime.Now,
                                         Lokalizacja = _lokalizacja,
                                         FormaPlatnosci = formaPlatnosci
                });
                ctx.SaveChanges();
            }

            this.Close();
                //Search();

            //textBox1.Text = string.Empty;
            //textBox2.Text = string.Empty;
            //textBox3.Text = string.Empty;

            //button1.Enabled = true;
            //button2.Enabled = true;
        }
    }
}
