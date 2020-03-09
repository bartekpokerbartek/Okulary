using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Okulary.Enums;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class Doplaty : Form
    {
        private int _binocleId;

        private readonly DoplataService _doplataService = new DoplataService();

        private Doplata _oldValue;

        public List<Doplata> DoplatyZakup;

        public Doplaty(int binocleId, List<Doplata> doplatyZakup)
        {
            InitializeComponent();
            _binocleId = binocleId;
            DoplatyZakup = doplatyZakup;
        }

        private async void Doplaty_Load(object sender, System.EventArgs e)
        {
            await Laduj();
        }

        private async Task Laduj()
        {
            dataGridView1.DataSource = new List<Doplata>(DoplatyZakup);

            dataGridView1.Columns["DoplataId"].Visible = false;
            dataGridView1.Columns["Binocle_BinocleId"].Visible = false;
            dataGridView1.Columns["Binocle"].Visible = false;
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

        private async void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            //var doplataId = (int)dataGridView1["DoplataId", e.RowIndex].Value;

            DialogResult dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                var doplata = (Doplata)dataGridView1.CurrentRow.DataBoundItem;

                doplata.DataDoplaty = (DateTime)dataGridView1["DataDoplaty", e.RowIndex].Value;
                doplata.Kwota = (decimal)dataGridView1["Kwota", e.RowIndex].Value;
                doplata.FormaPlatnosci = (FormaPlatnosci)dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value;

                //await _doplataService.Update(doplata);
            }
            else if (dialogResult == DialogResult.No)
            {
                //var doplata = (Doplata)dataGridView1.CurrentRow.DataBoundItem;

                dataGridView1["DataDoplaty", e.RowIndex].Value = _oldValue.DataDoplaty;
                dataGridView1["Kwota", e.RowIndex].Value = _oldValue.Kwota;
                dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value = _oldValue.FormaPlatnosci;
            }
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //_oldValue = (Doplata)dataGridView1.CurrentRow.DataBoundItem;

            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol")
            {
                // button clicked - do some logic
                //var doplataId = (int)dataGridView1["DoplataId", e.RowIndex].Value;

                DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć dopłatę?", "Usuń", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    DoplatyZakup.Remove((Doplata)dataGridView1.CurrentRow.DataBoundItem);
                    //await _doplataService.Delete(doplataId);
                    await Laduj();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using (var childForm = new DodajDoplate(_binocleId))
            {
                childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
                var result = childForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    DoplatyZakup.Add(childForm.Doplata);

                    await Laduj();
                }
            }
        }

        private async void Sprzedaz_Refresh(object sender, FormClosingEventArgs e)
        {
            //await Laduj();
        }

        private void Doplaty_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_context.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var current = (Doplata)dataGridView1.CurrentRow.DataBoundItem;

            _oldValue = new Doplata
                            {
                                DataDoplaty = current.DataDoplaty,
                                Kwota = current.Kwota,
                                FormaPlatnosci = current.FormaPlatnosci
                            };
        }
    }
}
