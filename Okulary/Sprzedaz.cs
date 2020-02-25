using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;

namespace Okulary
{
    public partial class Sprzedaz : Form
    {
        private readonly PayoutService _payoutService = new PayoutService();

        private readonly DoplataService _doplataService = new DoplataService();

        private readonly MoneyCountService _moneyCountService = new MoneyCountService();

        private readonly ElementsService _elementService = new ElementsService();

        private readonly BinocleService _binocleService = new BinocleService();

        private Lokalizacja _lokalizacja;

        private List<Lokalizacja> _dozwoloneLokalizacje;

        private DateTime _dataSelectora = DateTime.MinValue;

        private object _cellBeginEditValue;

        private decimal _dzienGotowka;

        private decimal _dzienKarta;

        public Sprzedaz(Lokalizacja lokalizacja)
        {
            InitializeComponent();
            _lokalizacja = lokalizacja;

            label5.Text = LokalizacjaHelper.DajLokalizacje(_lokalizacja);
            _dozwoloneLokalizacje = LokalizacjaHelper.DajDozwoloneLokalizacje(_lokalizacja);
        }

        private async void Sprzedaz_Load(object sender, EventArgs e)
        {
            await Laduj();
        }

        private async Task Laduj(bool ladujDzien = true, bool ladujMiesiac = true, bool aktualizujKase = true)
        {
            var nowyMiesiac = false;

            var data = dateTimePicker1.Value.Date;

            if (_dataSelectora == DateTime.MinValue || (_dataSelectora.Month != data.Month || _dataSelectora.Year != data.Year))
                nowyMiesiac = true;

            _dataSelectora = data;

            var allTasks = new List<Task>();

            if (ladujDzien)
            {
                allTasks.Add(LadujDzien(data));
                //await LadujDzien(data);
            }

            if (ladujMiesiac || nowyMiesiac)
            {
                allTasks.Add(LadujMiesiac(data));
                //await LadujMiesiac(data);
            }


            if (aktualizujKase || nowyMiesiac)
            {
                allTasks.Add(AktualizujKase());
                //await AktualizujKase();
            }

            await Task.WhenAll(allTasks);
        }

        private async Task LadujMiesiac(DateTime data)
        {
            //TODO: zmiana lokalizacji osoby powinna zmienić lokalizację w ELEMENCIE!!!??? Chyba nie, bo elementy były dodane bezpośrednio do tabeli, a te obliczone z zadatków będą miały wartości jak person.Lokalizacja
            var elementListMonthly = await _elementService.GetWithFilter(x => x.DataSprzedazy.Year == data.Year && x.DataSprzedazy.Month == data.Month && _dozwoloneLokalizacje.Contains(x.Lokalizacja));

            //TODO: dodać filtr na lokalizację? Done niżej!?
            var okularyMonthlyBezZadatku = await _binocleService.GetWithFilterWithIncludes(x => x.BuyDate.Year == data.Year && x.BuyDate.Month == data.Month && _dozwoloneLokalizacje.Contains(x.Person.Lokalizacja));
            var okularyMonthly = okularyMonthlyBezZadatku.Where(x => x.Zadatek > 0);
            var dodatkoweElementyMonthly = new List<Element>();

            foreach (var okular in okularyMonthly)
            {
                dodatkoweElementyMonthly.Add(new Element
                {
                    DataSprzedazy = okular.BuyDate,
                    Cena = okular.Zadatek,
                    Ilosc = 1,
                    FormaPlatnosci = okular.FormaPlatnosci
                });
            }

            // miesięczne dopłaty
            var doplatyMonthly = await _doplataService.GetWithFilterWithIncludes(x => x.DataDoplaty.Year == data.Year && x.DataDoplaty.Month == data.Month && _dozwoloneLokalizacje.Contains(x.Binocle.Person.Lokalizacja));
            var dodatkoweDoplatyMonthly = new List<Element>();

            foreach (var doplata in doplatyMonthly)
            {
                dodatkoweDoplatyMonthly.Add(new Element
                {
                    DataSprzedazy = doplata.DataDoplaty,
                    Cena = doplata.Kwota,
                    Ilosc = 1,
                    FormaPlatnosci = doplata.FormaPlatnosci
                });
            }

            elementListMonthly.AddRange(dodatkoweDoplatyMonthly);
            elementListMonthly.AddRange(dodatkoweElementyMonthly);

            var sumaMonthlyGotowka = elementListMonthly.Where(x => x.FormaPlatnosci == FormaPlatnosci.Gotowka).Sum(element => element.Cena * element.Ilosc);

            var sumaMonthlyKarta = elementListMonthly.Where(x => x.FormaPlatnosci == FormaPlatnosci.Karta).Sum(element => element.Cena * element.Ilosc);

            label7.Text = sumaMonthlyGotowka.ToString();
            label14.Text = sumaMonthlyKarta.ToString();
            label15.Text = (sumaMonthlyGotowka + sumaMonthlyKarta).ToString();

            label17.Text = (okularyMonthlyBezZadatku.Count(x => x.CenaOprawekBliz > 0) + okularyMonthlyBezZadatku.Count(x => x.CenaOprawekDal > 0)).ToString();
            label19.Text = (okularyMonthlyBezZadatku.Count(x => x.BlizOL.Cena > 0) +
                            okularyMonthlyBezZadatku.Count(x => x.BlizOP.Cena > 0) +
                            okularyMonthlyBezZadatku.Count(x => x.DalOL.Cena > 0) +
                            okularyMonthlyBezZadatku.Count(x => x.DalOP.Cena > 0)).ToString();
        }

        private async Task AktualizujKase()
        {
            var aktualizacjaKasy = (await _moneyCountService.GetWithFilter(x => x.Lokalizacja == _lokalizacja)).OrderByDescending(x => x.CreatedOn).FirstOrDefault();

            if (aktualizacjaKasy == null)
            {
                label21.Text = "Nie podano stanu początkowego kasy";
            }
            else
            {
                var doplatyOdDaty = (await _doplataService.GetWithFilterWithIncludes(
                                            x => x.DataDoplaty > aktualizacjaKasy.CreatedOn
                                                 && x.FormaPlatnosci == FormaPlatnosci.Gotowka
                                                 && _dozwoloneLokalizacje.Contains(x.Binocle.Person.Lokalizacja)))
                    .Sum(x => x.Kwota);

                var elementyOdDaty = (await _elementService.GetWithFilter(x => x.DataUtworzenia > aktualizacjaKasy.CreatedOn && x.FormaPlatnosci == FormaPlatnosci.Gotowka && _dozwoloneLokalizacje.Contains(x.Lokalizacja))).Sum(x => x.Cena * x.Ilosc);

                var sprzedazOdDaty = (await _binocleService.GetWithFilterWithIncludes(x => x.BuyDate > aktualizacjaKasy.CreatedOn && x.FormaPlatnosci == FormaPlatnosci.Gotowka && _dozwoloneLokalizacje.Contains(x.Person.Lokalizacja))).Sum(x => x.Zadatek);

                var wyplatyOdDaty = (await _payoutService.GetWithFilter(x => x.CreatedOn > aktualizacjaKasy.CreatedOn && _dozwoloneLokalizacje.Contains(x.Lokalizacja))).Sum(x => x.Amount);

                label21.Text = (aktualizacjaKasy.Amount + doplatyOdDaty + elementyOdDaty + sprzedazOdDaty - wyplatyOdDaty).ToString();
            }
        }

        private async Task LadujDzien(DateTime data)
        {
            var elementList = await _elementService.GetWithFilter(x => EntityFunctions.TruncateTime(x.DataSprzedazy) == data.Date && _dozwoloneLokalizacje.Contains(x.Lokalizacja));

            var okulary = await _binocleService.GetWithFilterWithIncludes(x => EntityFunctions.TruncateTime(x.BuyDate) == data.Date && x.Zadatek > 0 && _dozwoloneLokalizacje.Contains(x.Person.Lokalizacja));
            var dodatkoweElementy = new List<Element>();

            foreach (var okular in okulary)
            {
                dodatkoweElementy.Add(new Element
                {
                    DataSprzedazy = okular.BuyDate,
                    Cena = okular.Zadatek,
                    Ilosc = 1,
                    Nazwa = $"Zadatek {okular.Person.FirstName} {okular.Person.LastName}",
                    Lokalizacja = okular.Person.Lokalizacja,
                    CannotEdit = true,
                    FormaPlatnosci = okular.FormaPlatnosci
                });
            }

            var dodatkoweDoplaty = new List<Element>();

            var doplaty = await _doplataService.GetWithFilterWithIncludes(x => EntityFunctions.TruncateTime(x.DataDoplaty) == data.Date && _dozwoloneLokalizacje.Contains(x.Binocle.Person.Lokalizacja));

            foreach (var doplata in doplaty)
            {
                var person = doplata.Binocle.Person;

                dodatkoweDoplaty.Add(new Element
                {
                    DataSprzedazy = doplata.DataDoplaty,
                    Cena = doplata.Kwota,
                    Ilosc = 1,
                    Nazwa = $"Dopłata {person.FirstName} {person.LastName}",
                    Lokalizacja = person.Lokalizacja,
                    CannotEdit = true,
                    FormaPlatnosci = doplata.FormaPlatnosci
                });
            }

            var wyplaty = await _payoutService.GetWithFilter(x => EntityFunctions.TruncateTime(x.CreatedOn) == data.Date && _dozwoloneLokalizacje.Contains(x.Lokalizacja));

            var dodatkoweWyplaty = new List<Element>();

            foreach (var wyplata in wyplaty)
            {
                dodatkoweWyplaty.Add(new Element
                {
                    DataSprzedazy = wyplata.CreatedOn,
                    Cena = wyplata.Amount,
                    Ilosc = 1,
                    Nazwa = $"Wypłata: {wyplata.Description}",
                    Lokalizacja = wyplata.Lokalizacja,
                    CannotEdit = true,
                    FormaPlatnosci = FormaPlatnosci.Gotowka
                });
            }

            elementList.AddRange(dodatkoweWyplaty);
            elementList.AddRange(dodatkoweElementy);
            elementList.AddRange(dodatkoweDoplaty);

            dataGridView1.DataSource = elementList;

            dataGridView1.Columns["Nazwa"].Width = 240;
            dataGridView1.Columns["DataUtworzenia"].Visible = false;
            dataGridView1.Columns["ElementId"].Visible = false;
            dataGridView1.Columns["CannotEdit"].Visible = false;
            dataGridView1.Columns["Lokalizacja"].ReadOnly = true;
            dataGridView1.Columns["DataUtworzenia"].HeaderText = "Data zakupu";
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

            var sumaGotowka = elementList.Where(x => x.FormaPlatnosci == FormaPlatnosci.Gotowka).Sum(element => element.Cena * element.Ilosc);

            var sumaKarta = elementList.Where(x => x.FormaPlatnosci == FormaPlatnosci.Karta).Sum(element => element.Cena * element.Ilosc);

            var sumaWyplatDzien = dodatkoweWyplaty.Sum(x => x.Cena); 
            label3.Text = (sumaGotowka - sumaWyplatDzien).ToString(); // Nie bierzemy wypłat pod uwagę w podsumowaniu
            label10.Text = sumaKarta.ToString();
            label11.Text = (sumaKarta + sumaGotowka - sumaWyplatDzien).ToString();
        }

        private async void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0)
                return;

            var elementId = (int)dataGridView1["ElementId", e.RowIndex].Value;

            var rowIndex = dataGridView1.CurrentCell.RowIndex;
            var row = (DataGridViewDisableButtonCell)dataGridView1.Rows[rowIndex].Cells["UsunCol"];

            if (!row.Enabled)
            {
                MessageBox.Show("Nie można edytować wierszy bezpośrednio wygenerowanych z zadatku, wypłaty, bądź dopłaty.");
                dataGridView1[e.ColumnIndex, e.RowIndex].Value = _cellBeginEditValue;
                dataGridView1.Refresh();
                return;
            }

            var dialogResult = MessageBox.Show("Zapisać zmiany?", "Zapisz", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                var cena = (decimal)dataGridView1["Cena", e.RowIndex].Value;
                var ilosc = (int)dataGridView1["Ilosc", e.RowIndex].Value;

                if (cena <= 0 || ilosc <= 0)
                {
                    MessageBox.Show("Cena i ilość muszą być większe od zera. Nie zapisano, popraw dane.");
                    return;
                }

                var element = await _elementService.GetById(elementId);

                element.Nazwa = (string)dataGridView1["Nazwa", e.RowIndex].Value;
                element.DataSprzedazy = (DateTime)dataGridView1["DataSprzedazy", e.RowIndex].Value;
                element.Ilosc = ilosc;
                element.Cena = cena;
                element.FormaPlatnosci = (FormaPlatnosci)dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value;

                await _elementService.Update(element);

                await Laduj();
            }
            else if (dialogResult == DialogResult.No)
            {
                var element = await _elementService.GetById(elementId);

                dataGridView1["Nazwa", e.RowIndex].Value = element.Nazwa;
                dataGridView1["DataSprzedazy", e.RowIndex].Value = element.DataSprzedazy;
                dataGridView1["Ilosc", e.RowIndex].Value = element.Ilosc;
                dataGridView1["Cena", e.RowIndex].Value = element.Cena;
                dataGridView1["FormaPlatnosciCombo", e.RowIndex].Value = element.FormaPlatnosci;
            }
        }

        private async void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || dataGridView1.Columns[e.ColumnIndex].Name != "UsunCol"
                                  || !((DataGridViewDisableButtonCell)dataGridView1.CurrentCell).Enabled)
            {
                return;
            }

            // button clicked - do some logic
            var elementId = (int)dataGridView1["ElementId", e.RowIndex].Value;

            var dialogResult = MessageBox.Show("Czy jesteś pewien, że chcesz usunąć zakup?", "Usuń", MessageBoxButtons.YesNo);

            if (dialogResult == DialogResult.Yes)
            {
                await _elementService.Delete(elementId);
                await Laduj();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var childForm = new DodajElement(_lokalizacja, _dataSelectora);

            childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
            childForm.ShowDialog();
        }

        private async void Sprzedaz_Refresh(object sender, FormClosingEventArgs e)
        {
            await Laduj();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            //Laduj();
        }

        private void Sprzedaz_FormClosing(object sender, FormClosingEventArgs e)
        {
            //_context.Dispose();
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Wprowadzono niepoprawne dane!", "OK", MessageBoxButtons.OK);
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            _cellBeginEditValue = dataGridView1[e.ColumnIndex, e.RowIndex].Value;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var childForm = new DodajStanKasy(_lokalizacja);

            childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
            childForm.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var childForm = new Wyplaty(_lokalizacja);

            childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
            childForm.ShowDialog();
        }

        private async void dateTimePicker1_CloseUp(object sender, EventArgs e)
        {
            await Laduj(aktualizujKase: false, ladujMiesiac: false);
        }
    }
}
