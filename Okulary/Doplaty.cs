using Okulary.Model;
using Okulary.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

using Okulary.Enums;

namespace Okulary
{
    public partial class Doplaty : Form
    {
        private int _binocleId;

        private MineContext _context;

        public Doplaty(int binocleId)
        {
            InitializeComponent();
            _binocleId = binocleId;
            _context = new MineContext();
        }

        private void Doplaty_Load(object sender, System.EventArgs e)
        {
            Laduj();
        }

        private void Laduj()
        {
            var doplatyLista = new List<Doplata>
            {
                new Doplata
                {
                    DataDoplaty = DateTime.Now.Date,
                    Kwota = 1.5M
                }
            };

            doplatyLista = _context.Doplaty.Where(x => x.Binocle_BinocleId == _binocleId).ToList();

            dataGridView1.DataSource = doplatyLista;

            //for (int i = 0; i < dataGridView1.Columns.Count; i++)
            //{
            //    dataGridView1.Columns[i].Visible = false;
            //}

            dataGridView1.Columns["DoplataId"].Visible = false;
            dataGridView1.Columns["Binocle_BinocleId"].Visible = false;
            dataGridView1.Columns["Binocle"].Visible = false;
            //dataGridView1.Columns["CannotEdit"].Visible = false;
            //dataGridView1.Columns["Lokalizacja"].ReadOnly = true;
            dataGridView1.Columns["DataDoplaty"].HeaderText = "Data dopłaty";

            dataGridView1.Columns["FormaPlatnosci"].Visible = false;

            if (!dataGridView1.Columns.Contains("FormaPlatnosciCombo"))
            {
                var col = new DataGridViewComboBoxColumn();
                col.DataSource = Enum.GetValues(typeof(FormaPlatnosci));
                col.ValueType = typeof(FormaPlatnosci);
                col.Visible = true;
                col.DataPropertyName = "FormaPlatnosci";
                col.HeaderText = "Forma";
                col.Name = "FormaPlatnosciCombo";

                dataGridView1.Columns.Add(col);
            }

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

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            var doplataId = (int)dataGridView1["DoplataId", e.RowIndex].Value;

            DialogResult dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                var doplata = _context.Doplaty.First(x => x.DoplataId == doplataId);

                doplata.DataDoplaty = (DateTime)dataGridView1["DataDoplaty", e.RowIndex].Value;
                doplata.Kwota = (decimal)dataGridView1["Kwota", e.RowIndex].Value;
                doplata.FormaPlatnosci = (FormaPlatnosci)dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value;

                _context.SaveChanges();
            }
            else if (dialogResult == DialogResult.No)
            {
                var element = _context.Doplaty.First(x => x.DoplataId == doplataId);

                dataGridView1["DataDoplaty", e.RowIndex].Value = element.DataDoplaty;
                dataGridView1["Kwota", e.RowIndex].Value = element.Kwota;
                dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value = element.FormaPlatnosci;

                _context.SaveChanges();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol")
            {
                // button clicked - do some logic
                var doplataId = (int)dataGridView1["DoplataId", e.RowIndex].Value;

                DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć dopłatę?", "Usuń", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var doplata = _context.Doplaty.First(x => x.DoplataId == doplataId);
                    _context.Doplaty.Remove(doplata);
                    _context.SaveChanges();
                    Laduj();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var childForm = new DodajDoplate(_binocleId);

            childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
            childForm.Show();
        }

        private void Sprzedaz_Refresh(object sender, FormClosingEventArgs e)
        {
            Laduj();
        }

        private void Doplaty_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
