using System;
using System.Windows.Forms;
using Okulary.Enums;

namespace Okulary
{
    public partial class StartupFormForm : Form
    {
        public StartupFormForm()
        {
            InitializeComponent();
        }

        private void StartupFormForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = "Wszystkie";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lokalizacja = Lokalizacja.Wszystkie;
            if (comboBox1.SelectedItem.ToString() == "Dynów")
                lokalizacja = Lokalizacja.Dynow;
            else if (comboBox1.SelectedItem.ToString() == "Dubiecko")
                lokalizacja = Lokalizacja.Dubiecko;
            else
                lokalizacja = Lokalizacja.Wszystkie;

            var childForm = new Form1(lokalizacja);

            childForm.Show();
            Hide();
        }
    }
}
