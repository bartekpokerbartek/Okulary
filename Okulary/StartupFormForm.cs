using Okulary.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            comboBox1.SelectedItem = "Obydwie";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var lokalizacja = Lokalizacja.Obydwie;
            if (comboBox1.SelectedItem.ToString() == "Dynów")
                lokalizacja = Lokalizacja.Dynow;
            else if (comboBox1.SelectedItem.ToString() == "Dubiecko")
                lokalizacja = Lokalizacja.Dubiecko;
            else
                lokalizacja = Lokalizacja.Obydwie;

            var childForm = new Form1(lokalizacja);

            childForm.Show();
            this.Hide();
        }
    }
}
