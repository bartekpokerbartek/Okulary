using Okulary.Model;
using Okulary.Repo;
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
    public partial class Sprzedaz : Form
    {
        private MineContext _context;
        public Sprzedaz()
        {
            InitializeComponent();
            _context = new MineContext();
        }

        private void Sprzedaz_Load(object sender, EventArgs e)
        {
            Laduj();
        }

        private void Laduj()
        {
            var elementList = new List<Element>();

            var data = dateTimePicker1.Value.Date;

            //using (var ctx = new MineContext())
            //{
            //elementList = _context.Elements.Where(x => x.DataSprzedazy == data).ToList();
            elementList.Add(new Element
            {
                Cena = 14.0M,
                DataSprzedazy = DateTime.Now.Date,
                DataUtworzenia = DateTime.Now.Date,
                Ilosc = 2,
                Nazwa = "jakas nazwa"
            });
            //}

            dataGridView1.DataSource = elementList;

            //for (int i = 0; i < dataGridView1.Columns.Count; i++)
            //{
            //    dataGridView1.Columns[i].Visible = false;
            //}

            dataGridView1.Columns["DataUtworzenia"].Visible = false;
            dataGridView1.Columns["DataUtworzenia"].HeaderText = "Data zakupu";

            //if (!dataGridView1.Columns.Contains("ZakupCol"))
            //{
            //    DataGridViewButtonColumn col = new DataGridViewButtonColumn();
            //    col.UseColumnTextForButtonValue = true;
            //    col.Visible = true;
            //    col.Text = "Zakup";
            //    col.Name = "ZakupCol";
            //    dataGridView1.Columns.Add(col);
            //}

            //dataGridView1.Columns["ZakupCol"].Visible = true;
            //dataGridView1.Columns["ZakupCol"].HeaderText = "Zakup";

            if (!dataGridView1.Columns.Contains("UsunCol"))
            {
                DataGridViewButtonColumn col = new DataGridViewButtonColumn();
                col.UseColumnTextForButtonValue = true;
                col.Visible = true;
                col.Text = "Usuń";
                col.Name = "UsunCol";
                dataGridView1.Columns.Add(col);
            }

            dataGridView1.Columns["UsunCol"].Visible = true;
            dataGridView1.Columns["UsunCol"].HeaderText = "Usuń";
        }
    }
}
