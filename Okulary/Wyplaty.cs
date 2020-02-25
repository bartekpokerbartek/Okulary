using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Repo;

namespace Okulary
{
    public partial class Wyplaty : Form
    {
        private readonly PayoutService _payoutService = new PayoutService();

        private readonly MoneyCountService _moneyCountService = new MoneyCountService();

        private Lokalizacja _lokalizacja;

        private DateTime _aktualizacjaKasy;

        public Wyplaty(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;
        }

        private async void Wyplaty_Load(object sender, EventArgs e)
        {
            _aktualizacjaKasy = (await _moneyCountService.GetWithFilter(y => y.Lokalizacja == _lokalizacja)).OrderByDescending(x => x.CreatedOn).FirstOrDefault().CreatedOn;

            await Laduj();
        }

        private async Task Laduj()
        {
            var dozwoloneLokalizacje = LokalizacjaHelper.DajDozwoloneLokalizacje(_lokalizacja);

            var elementList = await _payoutService.GetWithFilter(x => x.CreatedOn > _aktualizacjaKasy && dozwoloneLokalizacje.Contains(_lokalizacja));

            dataGridView1.DataSource = elementList;

            dataGridView1.Columns["PayoutId"].Visible = false;
            dataGridView1.Columns["CreatedOn"].HeaderText = "Data wypłaty";
            dataGridView1.Columns["Description"].HeaderText = "Opis";
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
            //_context.Dispose();
        }

        private async void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            var wyplataId = (int)dataGridView1["PayoutId", e.RowIndex].Value;

            var dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                if (_aktualizacjaKasy > (DateTime)dataGridView1["CreatedOn", e.RowIndex].Value)
                {
                    MessageBox.Show("Data wypłaty sprzed aktualizacji kasy. Nie zapisano.");
                    return;
                }

                var wyplata = await _payoutService.GetById(wyplataId);

                wyplata.CreatedOn = (DateTime)dataGridView1["CreatedOn", e.RowIndex].Value;
                wyplata.Amount = (decimal)dataGridView1["Amount", e.RowIndex].Value;

                var descriptionValue = dataGridView1["Description", e.RowIndex].Value;

                wyplata.Description = descriptionValue == null ? string.Empty : descriptionValue.ToString();

                await _payoutService.Update(wyplata);

                await Laduj();
            }
            else if (dialogResult == DialogResult.No)
            {
                var element = await _payoutService.GetById(wyplataId);

                dataGridView1["CreatedOn", e.RowIndex].Value = element.CreatedOn;
                dataGridView1["Amount", e.RowIndex].Value = element.Amount;
            }
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol")
            {
                // button clicked - do some logic
                var wyplataId = (int)dataGridView1["PayoutId", e.RowIndex].Value;

                DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć dopłatę?", "Usuń", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    await _payoutService.Delete(wyplataId);

                    await Laduj();
                }
                else if (dialogResult == DialogResult.No)
                {
                    await Laduj();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var childForm = new DodajWyplate(_lokalizacja);

            childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
            childForm.ShowDialog();
        }

        private async void Sprzedaz_Refresh(object sender, FormClosingEventArgs e)
        {
            await Laduj();
        }
    }
}
