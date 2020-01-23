using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Repo;

namespace Okulary
{
    public partial class Wyplaty : Form
    {
        private MineContext _context;

        private Lokalizacja _lokalizacja;

        private DateTime _aktualizacjaKasy;

        public Wyplaty(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _context = new MineContext();
            _lokalizacja = lokalizacja;
            _aktualizacjaKasy = _context.Kasa.Where(y => y.Lokalizacja == _lokalizacja).OrderByDescending(x => x.CreatedOn).FirstOrDefault().CreatedOn;
        }

        private void Wyplaty_Load(object sender, EventArgs e)
        {
            Laduj();
        }

        private void Laduj()
        {
            var dozwoloneLokalizacje = LokalizacjaHelper.DajDozwoloneLokalizacje(_lokalizacja);

            var elementList = _context.Wyplaty.Where(x => x.CreatedOn > _aktualizacjaKasy).ToList();

            dataGridView1.DataSource = elementList;

            dataGridView1.Columns["PayoutId"].Visible = false;
            dataGridView1.Columns["CreatedOn"].HeaderText = "Data wypłaty";
            dataGridView1.Columns["Lokalizacja"].Visible = false;

            if (!dataGridView1.Columns.Contains("UsunCol"))
            {
                DataGridViewButtonColumn col = new DataGridViewDisableButtonColumn();
                col.UseColumnTextForButtonValue = true;
                col.Visible = true;
                col.Text = "Usuń";
                col.Name = "UsunCol";
                dataGridView1.Columns.Add(col);
            }

            dataGridView1.Columns["UsunCol"].Visible = true;
            dataGridView1.Columns["UsunCol"].HeaderText = "Usuń";
        }

        private void Wyplaty_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            var wyplataId = (int)dataGridView1["PayoutId", e.RowIndex].Value;

            DialogResult dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (_aktualizacjaKasy > (DateTime)dataGridView1["CreatedOn", e.RowIndex].Value)
                {
                    MessageBox.Show("Data wypłaty sprzed aktualizacji kasy. Nie zapisano.");
                    return;
                }

                var doplata = _context.Wyplaty.First(x => x.PayoutId == wyplataId);

                doplata.CreatedOn = (DateTime)dataGridView1["CreatedOn", e.RowIndex].Value;
                doplata.Amount = (decimal)dataGridView1["Amount", e.RowIndex].Value;

                _context.SaveChanges();

                Laduj();
            }
            else if (dialogResult == DialogResult.No)
            {
                var element = _context.Wyplaty.First(x => x.PayoutId == wyplataId);

                dataGridView1["CreatedOn", e.RowIndex].Value = element.CreatedOn;
                dataGridView1["Amount", e.RowIndex].Value = element.Amount;

                _context.SaveChanges();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol")
            {
                // button clicked - do some logic
                var wyplataId = (int)dataGridView1["PayoutId", e.RowIndex].Value;

                DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć dopłatę?", "Usuń", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var wyplata = _context.Wyplaty.First(x => x.PayoutId == wyplataId);
                    _context.Wyplaty.Remove(wyplata);
                    _context.SaveChanges();
                    Laduj();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Laduj();
                    //do something else
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var childForm = new DodajWyplate(_lokalizacja);

            childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
            childForm.ShowDialog();
        }

        private void Sprzedaz_Refresh(object sender, FormClosingEventArgs e)
        {
            Laduj();
        }
    }
}
