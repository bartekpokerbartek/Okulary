using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Okulary.Consts;
using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;
using PagedList;

namespace Okulary
{
    public partial class Form1 : Form
    {
        private readonly PersonService _personService = new PersonService();

        private Lokalizacja _lokalizacja;

        private IPagedList<Person> _personsToDisplay;
        private int _pageNumber = 1;
        private int _pageSize = int.Parse(ConfigurationManager.AppSettings["RozmiarStrony"]);

        public Form1()
        {
            InitializeComponent();
            textBox4.Text = _pageNumber.ToString();
        }

        public Form1(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            await Search();
            button1.Enabled = true;
            button2.Enabled = true;
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;

            var firstName = textBox1.Text;
            var lastName = textBox2.Text;
            var address = textBox3.Text;
            var birthDate = dateTimePicker1.Value.Date;

            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
                MessageBox.Show("Imię i nazwisko nie mogą być puste");
            else
            {
                if (!await _personService.Exists(firstName, lastName, birthDate))
                {
                    var person = new Person
                                     {
                                         FirstName = firstName,
                                         LastName = lastName,
                                         BirthDate = birthDate,
                                         Address = address,
                                         Binocles = new List<Binocle>(),
                                         Lokalizacja = _lokalizacja
                                     };

                    _personService.Create(person);
                }
            }

            await Search();

            textBox1.Text = string.Empty;
            textBox2.Text = string.Empty;
            textBox3.Text = string.Empty;

            button1.Enabled = true;
            button2.Enabled = true;
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "ZamowieniaNazwa")
            {
                // button clicked - do some logic
                var personId = (int)dataGridView1["PersonId", e.RowIndex].Value;
                var childForm = new Form2(personId, FromWhereConsts.ZBALANSOWANI);
                childForm.ShowDialog();
            }

            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol")
            {
                // button clicked - do some logic
                var personId = (int)dataGridView1["PersonId", e.RowIndex].Value;

                var dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć klienta?", "Usuń", MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    //cascade delete?
                    using (var ctx = new MineContext())
                    {
                        var person = ctx.Persons.First(x => x.PersonId == personId);
                        var zakupy = ctx.Binocles.Where(x => x.Person_PersonId == personId);
                        //TODO: Czy nie brakuje usuwania doplat?
                        ctx.Binocles.RemoveRange(zakupy);
                        ctx.Persons.Remove(person);
                        ctx.SaveChanges();
                    }

                    await Search();
                }
            }
        }

        private async Task Search()
        {
            var firstName = textBox1.Text;
            var lastName = textBox2.Text;

            var lokalizacje = LokalizacjaHelper.DajDozwoloneLokalizacje(_lokalizacja);

            _personsToDisplay = await _personService.GetWithFilter(x => (string.IsNullOrEmpty(firstName) || x.FirstName.Contains(firstName)) && (string.IsNullOrEmpty(lastName) || x.LastName.Contains(lastName)) && lokalizacje.Contains(x.Lokalizacja), _pageNumber, _pageSize);

            labelZIlu.Text = _personsToDisplay.PageCount.ToString();
            textBox4.Text = _pageNumber.ToString();
            btnPrevious.Enabled = _personsToDisplay.HasPreviousPage;
            btnNext.Enabled = _personsToDisplay.HasNextPage;

            dataGridView1.DataSource = _personsToDisplay.ToList();
            dataGridView1.Columns["Binocles"].Visible = false;
            dataGridView1.Columns["PersonId"].Visible = false;
            dataGridView1.RowHeadersVisible = false;

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

            //if (dataGridView1.RowCount > 0)
            //    dataGridView1.FirstDisplayedScrollingRowIndex = dataGridView1.RowCount - 1;
        }

        private async void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            var personId = (int)dataGridView1["PersonId", e.RowIndex].Value;

            DialogResult dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var person = await _personService.GetById(personId);

                person.Address = (string)dataGridView1["Address", e.RowIndex].Value;
                person.FirstName = (string)dataGridView1["FirstName", e.RowIndex].Value;
                person.LastName = (string)dataGridView1["LastName", e.RowIndex].Value;
                person.BirthDate = (DateTime)dataGridView1["BirthDate", e.RowIndex].Value;
                person.Lokalizacja = (Lokalizacja)dataGridView1["Lokalizacja", e.RowIndex].Value;

                await _personService.Update(person);

                await Search();
            }
            else if (dialogResult == DialogResult.No)
            {
                var person = await _personService.GetById(personId);

                dataGridView1["Address", e.RowIndex].Value = person.Address;
                dataGridView1["FirstName", e.RowIndex].Value = person.FirstName;
                dataGridView1["LastName", e.RowIndex].Value = person.LastName;
                dataGridView1["BirthDate", e.RowIndex].Value = person.BirthDate;
                dataGridView1["Lokalizacja", e.RowIndex].Value = person.Lokalizacja;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            label6.Text = LokalizacjaHelper.DajLokalizacje(_lokalizacja);

            //Dummy call do bazy z migracjami
            await _personService.Exists("dsfasdfsdf", "vtpoacasf", DateTime.Now);
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
            var childForm = new Sprzedaz(_lokalizacja);
            //childForm.FormClosing += new FormClosingEventHandler();
            childForm.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var childForm = new Niezbalansowani(_lokalizacja);
            //childForm.FormClosing += new FormClosingEventHandler();
            childForm.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            var childForm = new Nieodebrane(_lokalizacja);
            //childForm.FormClosing += new FormClosingEventHandler();
            childForm.ShowDialog();
        }

        private void Button6_Click(object sender, EventArgs e)
        {
            if (_personsToDisplay.HasPreviousPage)
            {
                _pageNumber--;
                Search();
                btnPrevious.Enabled = _personsToDisplay.HasPreviousPage;
                btnNext.Enabled = _personsToDisplay.HasNextPage;
            }
        }

        private void Button6_Click_1(object sender, EventArgs e)
        {
            if (_personsToDisplay.HasNextPage)
            {
                _pageNumber++;
                Search();
                btnPrevious.Enabled = _personsToDisplay.HasPreviousPage;
                btnNext.Enabled = _personsToDisplay.HasNextPage;
            }
        }

        private void TextBox4_Leave(object sender, EventArgs e)
        {
            var wynik = int.TryParse(textBox4.Text, out var somePageNumber);

            if (!wynik)
            {
                MessageBox.Show("Niepoprawna wartość", "OK", MessageBoxButtons.OK);
                return;
            }

            if (_personsToDisplay == null)
            {
                MessageBox.Show("Brak wyników wyszukiwania", "OK", MessageBoxButtons.OK);
                return;
            }

            if (_pageNumber < 1 || _pageNumber > _personsToDisplay.PageCount)
            {
                MessageBox.Show("Wartość musi być z zakresu od 1 do maksymalnej ilości stron", "OK", MessageBoxButtons.OK);
                return;
            }

            _pageNumber = somePageNumber;
            Search();
        }

        private void TextBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13){
                e.Handled = true;

            SelectNextControl(labelZIlu, true, true, true, true);
        }
    }
    }
}
