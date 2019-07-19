﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Windows.Forms;
using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class Sprzedaz : Form
    {
        private MineContext _context;

        private Lokalizacja _lokalizacja;

        private DateTime _dataSelectora;

        private object _cellBeginEditValue;

        public Sprzedaz(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _context = new MineContext();
            _lokalizacja = lokalizacja;
        }

        private void Sprzedaz_Load(object sender, EventArgs e)
        {
            Laduj();
        }

        private void Laduj()
        {
            label5.Text = LokalizacjaHelper.DajLokalizacje(_lokalizacja);

            var data = dateTimePicker1.Value.Date;
            _dataSelectora = data;

            var dozwoloneLokalizacje = LokalizacjaHelper.DajDozwoloneLokalizacje(_lokalizacja);

            var elementList = _context.Elements.Where(x => x.DataSprzedazy == data && dozwoloneLokalizacje.Contains(x.Lokalizacja)).ToList();
            
            var okulary = _context.Binocles.Where(x => EntityFunctions.TruncateTime(x.BuyDate) == data && x.Zadatek > 0).ToList();
            var dodatkoweElementy = new List<Element>();

            foreach (var okular in okulary)
            {
                var person = _context.Persons.FirstOrDefault(x => x.PersonId == okular.Person_PersonId && dozwoloneLokalizacje.Contains(x.Lokalizacja));

                if (person == null)
                    continue;

                dodatkoweElementy.Add(new Element
                                          {
                                              DataSprzedazy = data,
                                              Cena = okular.Zadatek,
                                              Ilosc = 1,
                                              Nazwa = $"Zadatek {person.FirstName} {person.LastName}",
                                              Lokalizacja = person.Lokalizacja,
                                              CannotEdit = true,
                                              FormaPlatnosci = okular.FormaPlatnosci
                                          });
            }

            var dodatkoweDoplaty = new List<Element>();

            var doplaty = _context.Doplaty.Where(x => EntityFunctions.TruncateTime(x.DataDoplaty) == data).ToList();

            foreach (var doplata in doplaty)
            {
                var okular = _context.Binocles.FirstOrDefault(x => x.BinocleId == doplata.Binocle_BinocleId);

                if (okular == null)
                    continue;
                
                var person = _context.Persons.FirstOrDefault(x => x.PersonId == okular.Person_PersonId && dozwoloneLokalizacje.Contains(x.Lokalizacja));

                if (person == null)
                    continue;

                dodatkoweDoplaty.Add(new Element
                {
                    DataSprzedazy = data,
                    Cena = doplata.Kwota,
                    Ilosc = 1,
                    Nazwa = $"Dopłata {person.FirstName} {person.LastName}",
                    Lokalizacja = person.Lokalizacja,
                    CannotEdit = true,
                    FormaPlatnosci = doplata.FormaPlatnosci
                });
            }

            elementList.AddRange(dodatkoweElementy);
            elementList.AddRange(dodatkoweDoplaty);

            dataGridView1.DataSource = elementList;

            dataGridView1.Columns["Nazwa"].Width = 240;
            dataGridView1.Columns["DataUtworzenia"].Visible = false;
            dataGridView1.Columns["ElementId"].Visible = false;
            dataGridView1.Columns["CannotEdit"].Visible = false;
            dataGridView1.Columns["Lokalizacja"].ReadOnly = true;
            dataGridView1.Columns["DataUtworzenia"].HeaderText = "Data zakupu";

            //Combo kolumna

            dataGridView1.Columns["FormaPlatnosci"].Visible = false;

            if (!dataGridView1.Columns.Contains("FormaPlatnosciCombo"))
            {
                var col = new DataGridViewComboBoxColumn();
                col.DataSource = Enum.GetValues(typeof(FormaPlatnosci));
                col.ValueType = typeof(FormaPlatnosci);
                //col.ValueMember = "FormaPlatnosci";
                col.Visible = true;
                col.DataPropertyName = "FormaPlatnosci";
                col.HeaderText = "Forma";
                col.Name = "FormaPlatnosciCombo";
                
                dataGridView1.Columns.Add(col);
            }
            //dataGridView1.Bin

            //var column = new DataGridViewComboBoxColumn();
            //var lista = Enum.GetNames(typeof(Lokalizacja)).ToList();
            //column.DataSource = lista;
            //dataGridView1.Columns.Add(column);

            //Combo kolumna

            //dataGridView1.Columns["ZakupCol"].Visible = true;
            //dataGridView1.Columns["ZakupCol"].HeaderText = "Zakup";

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

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                ((DataGridViewDisableButtonCell)row.Cells["UsunCol"]).Enabled = !((Element)row.DataBoundItem).CannotEdit;
                ((DataGridViewDisableButtonCell)row.Cells["UsunCol"]).ReadOnly = !((Element)row.DataBoundItem).CannotEdit;
            }
            dataGridView1.Refresh();


            decimal suma = 0.0M;
            foreach (var element in elementList.Where(x => x.FormaPlatnosci == FormaPlatnosci.Gotowka))
            {
                suma += element.Cena * element.Ilosc;
            }

            label3.Text = suma.ToString();

            //TODO: zmiana lokalizacji osoby powinna zmienić lokalizację w ELEMENCIE!!!??? Chyba nie, bo elementy były dodane bezpośrednio do tabeli, a te obliczone z zadatków będą miały wartości jak person.Lokalizacja
            var elementListMonthly = _context.Elements.Where(x => x.DataSprzedazy.Year == data.Year && x.DataSprzedazy.Month == data.Month && dozwoloneLokalizacje.Contains(x.Lokalizacja)).ToList();

            //TODO: dodać filtr na lokalizację? Done niżej!?
            var okularyMonthly = _context.Binocles.Where(x => x.BuyDate.Year == data.Year && x.BuyDate.Month == data.Month && x.Zadatek > 0).ToList();
            var dodatkoweElementyMonthly = new List<Element>();

            var personyIdsMiesiac = okularyMonthly.Select(x => x.Person_PersonId).ToList();
            var persony = _context.Persons.Where(x => personyIdsMiesiac.Contains(x.PersonId) && dozwoloneLokalizacje.Contains(x.Lokalizacja));

            foreach (var okular in okularyMonthly)
            {
                var person = persony.FirstOrDefault(x => x.PersonId == okular.Person_PersonId);

                if (person == null)
                    continue;

                dodatkoweElementyMonthly.Add(new Element
                                          {
                                              DataSprzedazy = data,
                                              Cena = okular.Zadatek,
                                              Ilosc = 1
                                          });
            }

            // miesięczne dopłaty
            var doplatyMonthly = _context.Doplaty.Where(x => x.DataDoplaty.Year == data.Year && x.DataDoplaty.Month == data.Month).ToList();
            var dodatkoweDoplatyMonthly = new List<Element>();

            var okularyPersonyIdsMiesiac = doplatyMonthly.Select(x => x.Binocle_BinocleId);
            var binoklePersonyIdsMiesiac = _context.Binocles.Where(x => okularyPersonyIdsMiesiac.Contains(x.BinocleId));
            var personyPersonyIdsMiesiac = binoklePersonyIdsMiesiac.Select(x => x.Person_PersonId).ToList();

            var personyZDoplat = _context.Persons.Where(x => personyPersonyIdsMiesiac.Contains(x.PersonId) && dozwoloneLokalizacje.Contains(x.Lokalizacja)).ToList();
            
            foreach(var doplata in doplatyMonthly)
            {
                var okularkiId = okularyPersonyIdsMiesiac.FirstOrDefault(x => x == doplata.Binocle_BinocleId);

                if (okularkiId == null)
                    continue;

                var personkaId = binoklePersonyIdsMiesiac.FirstOrDefault(x => x.BinocleId == okularkiId);

                if (personkaId == null)
                    continue;

                var personka = personyZDoplat.Any(x => x.PersonId == personkaId.Person_PersonId);

                if (!personka)
                    continue;

                dodatkoweDoplatyMonthly.Add(new Element
                {
                    DataSprzedazy = doplata.DataDoplaty,
                    Cena = doplata.Kwota,
                    Ilosc = 1
                });
            }

            //

            elementListMonthly.AddRange(dodatkoweDoplatyMonthly);
            elementListMonthly.AddRange(dodatkoweElementyMonthly);

            decimal sumaMonthly = 0.0M;
            foreach (var element in elementListMonthly.Where(x => x.FormaPlatnosci == FormaPlatnosci.Gotowka))
            {
                sumaMonthly += element.Cena * element.Ilosc;
            }

            label7.Text = sumaMonthly.ToString();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            var elementId = (int)dataGridView1["ElementId", e.RowIndex].Value;

            var rowIndex = dataGridView1.CurrentCell.RowIndex;
            var row = (DataGridViewDisableButtonCell)dataGridView1.Rows[rowIndex].Cells["UsunCol"];

            if (!row.Enabled)
            {
                MessageBox.Show("Nie można edytować bezpośrednio wierszy wygenerowanych z zadatku, bądź dopłaty.");
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = _cellBeginEditValue;
                dataGridView1.Refresh();
                return;
            }

            DialogResult dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                var element = _context.Elements.First(x => x.ElementId == elementId);

                element.Nazwa = (string)dataGridView1["Nazwa", e.RowIndex].Value;
                element.DataSprzedazy = (DateTime)dataGridView1["DataSprzedazy", e.RowIndex].Value;
                element.Ilosc = (int)dataGridView1["Ilosc", e.RowIndex].Value;
                element.Cena = (decimal)dataGridView1["Cena", e.RowIndex].Value;
                element.FormaPlatnosci = (FormaPlatnosci)dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value;

                _context.SaveChanges();
            }
            else if (dialogResult == DialogResult.No)
            {
                var element = _context.Elements.First(x => x.ElementId == elementId);

                dataGridView1["Nazwa", e.RowIndex].Value = element.Nazwa;
                dataGridView1["DataSprzedazy", e.RowIndex].Value = element.DataSprzedazy;
                dataGridView1["Ilosc", e.RowIndex].Value = element.Ilosc;
                dataGridView1["Cena", e.RowIndex].Value = element.Cena;
                dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value = element.FormaPlatnosci;

                _context.SaveChanges();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "UsunCol" && ((DataGridViewDisableButtonCell)dataGridView1.CurrentCell).Enabled)
            {
                // button clicked - do some logic
                var elementId = (int)dataGridView1["ElementId", e.RowIndex].Value;

                DialogResult dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć zakup?", "Usuń", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    var element = _context.Elements.First(x => x.ElementId == elementId);
                    _context.Elements.Remove(element);
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
            var childForm = new DodajElement(_lokalizacja, _dataSelectora);

            childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
            childForm.Show();
        }

        private void Sprzedaz_Refresh(object sender, FormClosingEventArgs e)
        {
            Laduj();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            Laduj();
        }

        private void Sprzedaz_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Wprowadzono niepoprawne dane!", "OK", MessageBoxButtons.OK);
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _cellBeginEditValue = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
        }
    }
}
