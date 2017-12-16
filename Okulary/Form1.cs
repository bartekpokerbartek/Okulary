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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                            Binocles = new List<Binocle>()
                        });

                        ctx.SaveChanges();
                    }
                }
            }

            Search();

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
        }

        private void Search()
        {
            var personList = new List<Person>();

            var firstName = textBox1.Text;
            var lastName = textBox2.Text;

            using (var ctx = new MineContext())
            {
                personList = ctx.Persons.Where(x => (string.IsNullOrEmpty(firstName) || x.FirstName.Contains(firstName)) && (string.IsNullOrEmpty(lastName) || x.LastName.Contains(lastName))).ToList();
            }

            dataGridView1.DataSource = personList;
            dataGridView1.Columns["Binocles"].Visible = false;

            if (!dataGridView1.Columns.Contains("ZamowieniaNazwa"))
            {
                DataGridViewButtonColumn col = new DataGridViewButtonColumn();
                col.UseColumnTextForButtonValue = true;
                col.Text = "Zamowienia";
                col.Name = "ZamowieniaNazwa";
                dataGridView1.Columns.Add(col);
            }
        }
    }
}
