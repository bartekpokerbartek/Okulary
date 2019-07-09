using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Okulary
{
    public partial class Form1 : Form
    {
        private Lokalizacja _lokalizacja;
        public Form1()
        {
            InitializeComponent();
            //this.dataGridView1.Columns["Binocles"].Visible = false;
        }

        public Form1(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;
            //this.dataGridView1.Columns["Binocles"].Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            Search();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;

            using (var ctx = new MineContext())
            {
                var firstName = textBox1.Text;
                var lastName = textBox2.Text;
                var address = textBox3.Text;
                var birthDate = dateTimePicker1.Value.Date;

                if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                    MessageBox.Show("Imię i nazwisko nie mogą być puste");
                else
                {
                    if (!Exists(firstName, lastName, birthDate, ctx))
                    {
                        ctx.Persons.Add(new Person
                        {
                            FirstName = firstName,
                            LastName = lastName,
                            BirthDate = birthDate,
                            Address = address,
                            Binocles = new List<Binocle>(),
                            Lokalizacja = _lokalizacja
                        });

                        ctx.SaveChanges();
                    }
                }
            }

            Search();

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;

            button1.Enabled = true;
            button2.Enabled = true;
        }

        private bool Exists(string firstName, string LastName, DateTime birth, MineContext ctx)
        {
            return ctx.Persons.Any(x => x.FirstName == firstName && x.LastName == LastName && x.BirthDate == birth.Date);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "ZamowieniaNazwa")
            {
                // button clicked - do some logic
                var personId = (int)dataGridView1["PersonId", e.RowIndex].Value;
                var childForm = new Form2(personId);
                childForm.Show();
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

        private void Search()
        {
            var personList = new List<Person>();

            var firstName = textBox1.Text;
            var lastName = textBox2.Text;

            List<Lokalizacja> lokalizacje;

            if (_lokalizacja == Lokalizacja.Wszystkie)
                lokalizacje = new List<Lokalizacja>
                {
                    Lokalizacja.Dynow,
                    Lokalizacja.Dubiecko,
                    Lokalizacja.Wszystkie
                };
            else
                lokalizacje = new List<Lokalizacja>
                {
                    _lokalizacja,
                    Lokalizacja.Wszystkie
                };

            using (var ctx = new MineContext())
            {
                personList = ctx.Persons.Where(x => (string.IsNullOrEmpty(firstName) || x.FirstName.Contains(firstName)) && (string.IsNullOrEmpty(lastName) || x.LastName.Contains(lastName)) && (lokalizacje.Contains(x.Lokalizacja))).ToList();
            }

            dataGridView1.DataSource = personList;
            dataGridView1.Columns["Binocles"].Visible = false;
            dataGridView1.Columns["PersonId"].Visible = false;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns["FirstName"].HeaderText = "Imię";
            dataGridView1.Columns["LastName"].HeaderText = "Nazwisko";
            dataGridView1.Columns["Address"].HeaderText = "Adres";
            dataGridView1.Columns["Address"].Width = 240;
            dataGridView1.Columns["BirthDate"].HeaderText = "Data urodzenia";
            dataGridView1.Columns["Lokalizacja"].Width = 75;

            //DODANIE KOLUMNY Z COMBO!!!
            //var column = new DataGridViewComboBoxColumn();
            //var lista = Enum.GetNames(typeof(Lokalizacja)).ToList();
            //column.DataSource = lista;
            //dataGridView1.Columns.Add(column);

            //dataGridView1.Columns["Lokalizacja"].

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

            //if (!dataGridView1.Columns.Contains("ZapiszCol"))
            //{
            //    DataGridViewButtonColumn col = new DataGridViewButtonColumn();
            //    col.UseColumnTextForButtonValue = true;
            //    col.Visible = true;
            //    col.Text = "Zapisz";
            //    col.Name = "ZapiszCol";
            //    dataGridView1.Columns.Add(col);
            //}

            //dataGridView1.Columns["ZapiszCol"].Visible = true;
            //dataGridView1.Columns["ZapiszCol"].HeaderText = "Usu";
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
                    //Search();
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

            //Search();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label6.Text = LokalizacjaHelper.DajLokalizacje(_lokalizacja);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Popraw lokalizację!", "OK", MessageBoxButtons.OK);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var childForm = new Sprzedaz();
            //childForm.FormClosing += new FormClosingEventHandler();
            childForm.Show();
        }
    }
}
