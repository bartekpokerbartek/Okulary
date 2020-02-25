using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;

using Okulary.Consts;
using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class Nieodebrane : Form
    {
        private Lokalizacja _lokalizacja;

        private OrderHelper _orderHelper = new OrderHelper();

        private readonly DateTime _dataOdbioru = DateTime.Parse(ConfigurationManager.AppSettings["DataOdbioru"]);

        public Nieodebrane(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;
        }

        private void Search()
        {
            var personList = new List<Person>();

            var lokalizacje = LokalizacjaHelper.DajDozwoloneLokalizacje(_lokalizacja);

            using (var ctx = new MineContext())
            {
                personList = ctx.Persons.Include(x => x.Binocles).Where(x => lokalizacje.Contains(x.Lokalizacja) && x.Binocles.Any(y => !(y.IsDataOdbioru || y.BuyDate <= _dataOdbioru))).ToList();
            }

            //https://stackoverflow.com/questions/7259567/linq-to-entities-does-not-recognize-the-method/7259649
            //var doWyswietlenia = personList.Where(x => x.Binocles.Any(y => !_orderHelper.CzyOdebrany(y))).ToList();

            dataGridView1.DataSource = personList;
            dataGridView1.Columns["Binocles"].Visible = false;
            dataGridView1.Columns["PersonId"].Visible = false;
            dataGridView1.RowHeadersVisible = true;
            dataGridView1.RowHeadersWidth = 60;

            dataGridView1.Columns["FirstName"].HeaderText = "Imię";
            dataGridView1.Columns["LastName"].HeaderText = "Nazwisko";
            dataGridView1.Columns["Address"].HeaderText = "Adres";
            dataGridView1.Columns["Address"].Width = 240;
            dataGridView1.Columns["BirthDate"].HeaderText = "Data urodzenia";
            dataGridView1.Columns["Lokalizacja"].Width = 75;

            if (!dataGridView1.Columns.Contains("ZamowieniaNazwa"))
            {
                DataGridViewButtonColumn col = new DataGridViewButtonColumn();
                col.UseColumnTextForButtonValue = true;
                col.Text = "Zamówienia";
                col.Name = "ZamowieniaNazwa";
                col.HeaderText = "Zamówienia";
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

            SetRowNumber(dataGridView1);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            var personId = (int)dataGridView1["PersonId", e.RowIndex].Value;

            DialogResult dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (var ctx = new MineContext())
                {
                    var person = ctx.Persons.First(x => x.PersonId == personId);

                    person.Address = (string)dataGridView1["Address", e.RowIndex].Value;
                    person.FirstName = (string)dataGridView1["FirstName", e.RowIndex].Value;
                    person.LastName = (string)dataGridView1["LastName", e.RowIndex].Value;
                    person.BirthDate = (DateTime)dataGridView1["BirthDate", e.RowIndex].Value;
                    person.Lokalizacja = (Lokalizacja)dataGridView1["Lokalizacja", e.RowIndex].Value;
                    ctx.SaveChanges();
                    Search();
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                using (var ctx = new MineContext())
                {
                    var person = ctx.Persons.First(x => x.PersonId == personId);

                    dataGridView1["Address", e.RowIndex].Value = person.Address;
                    dataGridView1["FirstName", e.RowIndex].Value = person.FirstName;
                    dataGridView1["LastName", e.RowIndex].Value = person.LastName;
                    dataGridView1["BirthDate", e.RowIndex].Value = person.BirthDate;
                    dataGridView1["Lokalizacja", e.RowIndex].Value = person.Lokalizacja;
                    ctx.SaveChanges();
                    //Search();
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Popraw dane!", "OK", MessageBoxButtons.OK);
        }

        private void SetRowNumber(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {
                row.HeaderCell.Value = string.Format("{0}", row.Index + 1);
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "ZamowieniaNazwa")
            {
                // button clicked - do some logic
                var personId = (int)dataGridView1["PersonId", e.RowIndex].Value;
                var childForm = new Form2(personId, FromWhereConsts.ODEBRANIE);
                childForm.ShowDialog();
            }

            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol")
            {
                // button clicked - do some logic
                var personId = (int)dataGridView1["PersonId", e.RowIndex].Value;

                DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć klienta?", "Usuń", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    using (var ctx = new MineContext())
                    {
                        var person = ctx.Persons.First(x => x.PersonId == personId);
                        var zakupy = ctx.Binocles.Where(x => x.Person_PersonId == personId);
                        ctx.Binocles.RemoveRange(zakupy);
                        ctx.Persons.Remove(person);
                        ctx.SaveChanges();
                        Search();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }

        private void Nieodebrane_Load(object sender, EventArgs e)
        {
            label2.Text = LokalizacjaHelper.DajLokalizacje(_lokalizacja);
            Search();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Search();
        }
    }
}
