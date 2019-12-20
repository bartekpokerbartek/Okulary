using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using Okulary.Consts;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class Form2 : Form
    {
        int _personId;
        MineContext _context;
        private PriceHelper _priceHelper = new PriceHelper();
        private OrderHelper _orderHelper = new OrderHelper();

        private string _fromWhereOpened;

        public Form2()
        {
            InitializeComponent();
        }

        public Form2(int personId, string fromWhere)
        {
            _personId = personId;
            InitializeComponent();
            _context = new MineContext();
            _fromWhereOpened = fromWhere;
        }

        public int PersonId { get; set; }

        private void Form2_Load(object sender, EventArgs e)
        {
            Laduj();
        }

        private void Laduj()
        {
            var binocleList = new List<Binocle>();

            //using (var ctx = new MineContext())
            //{
            binocleList = _context.Binocles.Include(x => x.Doplaty).Where(x => x.Person_PersonId == _personId).ToList();
            //}

            dataGridView1.DataSource = binocleList;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Visible = false;
            }

            dataGridView1.Columns["BuyDate"].Visible = true;
            dataGridView1.Columns["BuyDate"].HeaderText = "Data zakupu";

            if (!dataGridView1.Columns.Contains("ZakupCol"))
            {
                DataGridViewButtonColumn col = new DataGridViewButtonColumn();
                col.UseColumnTextForButtonValue = true;
                col.Visible = true;
                col.Text = "Zakup";
                col.Name = "ZakupCol";
                dataGridView1.Columns.Add(col);
            }

            dataGridView1.Columns["ZakupCol"].Visible = true;
            dataGridView1.Columns["ZakupCol"].HeaderText = "Zakup";

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

            foreach (var row in dataGridView1.Rows)
            {
                if (_fromWhereOpened == FromWhereConsts.ZBALANSOWANI)
                {
                    if (_priceHelper.CzyZbalansowany((Binocle)((DataGridViewRow)row).DataBoundItem))
                        ((DataGridViewRow)row).DefaultCellStyle.BackColor = Color.Crimson;
                }

                if (_fromWhereOpened == FromWhereConsts.ODEBRANIE)
                {
                    if (!_orderHelper.CzyOdebrany((Binocle)((DataGridViewRow)row).DataBoundItem))
                        ((DataGridViewRow)row).DefaultCellStyle.BackColor = Color.DarkViolet;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var childForm = new Form3(-1, _personId);
            childForm.FormClosing += new FormClosingEventHandler(this.Form2_Refresh);
            childForm.ShowDialog();

            //button1.Enabled = false;
            //button2.Enabled = false;
            ////using (var ctx = new MineContext())
            ////{
            //var description = textBox1.Text;
            //    var sellDate = dateTimePicker1.Value;

            //    if (string.IsNullOrEmpty(description))
            //        MessageBox.Show("Pole opis nie może być puste");
            //    else
            //    {
            //        var binocle = new Binocle
            //        {
            //            BuyDate = sellDate,
            //            Description = description,
            //            Person_PersonId = _personId,
            //            DalOL = new Soczewka(),
            //            DalOP = new Soczewka(),
            //            BlizOL = new Soczewka(),
            //            BlizOP = new Soczewka(),
            //            DataOdbioru = DateTime.Now
            //        };

            //        _context.Binocles.Add(binocle);

            //        _context.SaveChanges();
            //        MessageBox.Show("Dodano rekord.");
            //        dataGridView1.DataSource = _context.Binocles.Where(x => x.Person_PersonId == _personId).ToList();
            //    }
            ////}
            //button1.Enabled = true;
            //button2.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;
            //using (var ctx = new MineContext())
            //{
            //    ctx.SaveChanges();
            //}

            DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz zapisać zmiany?", "Zapisz zmiany", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                //do something
                _context.SaveChanges();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }

            button1.Enabled = true;
            button2.Enabled = true;
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
        }

        private void Form2_Refresh(object sender, FormClosingEventArgs e)
        {
            Laduj();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "ZakupCol")
            {
                // button clicked - do some logic
                var binocleId = (int)dataGridView1["BinocleId", e.RowIndex].Value;
                var childForm = new Form3(binocleId, _personId);
                childForm.FormClosing += new FormClosingEventHandler(this.Form2_Refresh);
                childForm.ShowDialog();
            }

            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol")
            {
                // button clicked - do some logic
                var binocleId = (int)dataGridView1["BinocleId", e.RowIndex].Value;

                DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć zakup?", "Usuń", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var binocle = _context.Binocles.First(x => x.BinocleId == binocleId);
                    _context.Binocles.Remove(binocle);
                    _context.SaveChanges();
                    Laduj();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
        }
    }
}
