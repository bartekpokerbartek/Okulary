using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows.Forms;
using Okulary.Enums;
using Okulary.Helpers;
using Okulary.Model;
using Okulary.Repo;
using Okulary.Services;

namespace Okulary
{
    public partial class Form3 : Form
    {
        int _binocleId;
        int _personId;
        string osFormat = "N0";
        Person _person;
        MineContext _context;
        Binocle _zakup;
        Mapper _mapper;
        PriceHelper _priceHelper;
        bool _rodzajOprawekDalCenaFocused;
        private bool _rodzajOprawekBlizCenaFocused;
        private bool _robociznaFocused;
        private bool _dalOPCylinderFocused;
        private bool _dalOLCylinderFocused;
        private bool _blizOLOdlFocused;
        private bool _blizOLVylinderFocused;
        private bool _dalOPOsFocused;
        private bool _dalOLOsFocused;
        private bool _blizOPOsFocused;
        private bool _blizOLOsFocused;
        private bool _dalOPOdlFocused;
        private bool _dalOLOdlFocused;
        private bool _blizOPOdlFocused;
        private bool _dalOPCenaFocused;
        private bool _dalOLCenaFocused;
        private bool _blizOPCenaFocused;
        private bool _blizOLCenaFocused;
        private bool _refundacjaFocused;
        private bool _zadatekFocused;
        private bool _blizOPCylinderFocused;

        public Form3()
        {
            InitializeComponent();
        }

        public Form3(int binocleId, int personId)
        {
            _binocleId = binocleId;
            _personId = personId;
            InitializeComponent();
            _context = new MineContext();
            _person = _context.Persons.First(x => x.PersonId == _personId);
            _priceHelper = new PriceHelper();
            _mapper = new Mapper();
            comboBox5.DataSource = Enum.GetValues(typeof(FormaPlatnosci));
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            if (_binocleId == -1)
            {
                var binocle = new Binocle
                {
                    BuyDate = DateTime.Now,
                    Person_PersonId = _personId,
                    DalOL = new Soczewka(),
                    DalOP = new Soczewka(),
                    BlizOL = new Soczewka(),
                    BlizOP = new Soczewka(),
                    DataOdbioru = DateTime.Now
                };

                _zakup = binocle;
                //_context.Binocles.Add(binocle);
                //_context.SaveChanges();
                //_binocleId = binocle.BinocleId;
            }
            else
            {
                _zakup = _context.Binocles.Include(x => x.Doplaty).Where(x => x.BinocleId == _binocleId).FirstOrDefault();
            }

            Mapuj();
        }

        private void Mapuj()
        {
            checkBox2.Checked = _zakup.Doplaty.Any();
            checkBox2.Enabled = false;
            checkBox2.Visible = checkBox2.Checked;

            comboBox5.SelectedItem = _zakup.FormaPlatnosci;

            dataZakupu.Value = _zakup.BuyDate;
            checkBox1.Checked = _zakup.IsDataOdbioru;
            SetCheckbox();

            dataOdbioru.Value = _zakup.DataOdbioru;
            NumerZlecenia.Text = _zakup.NumerZlecenia;
            RodzajOprawekDal.Text = _zakup.RodzajOprawekDal;
            RodzajOprawekBliz.Text = _zakup.RodzajOprawekBliz;
            RodzajOprawekBlizCena.Text = _zakup.CenaOprawekBliz.ToString();
            RodzajOprawekDalCena.Text = _zakup.CenaOprawekDal.ToString();
            comboBox1.Text = _mapper.MapujDodatnie(_zakup.DalOP.Sfera);
            comboBox2.Text = _mapper.MapujDodatnie(_zakup.DalOL.Sfera);
            comboBox3.Text = _mapper.MapujDodatnie(_zakup.BlizOP.Sfera);
            comboBox4.Text = _mapper.MapujDodatnie(_zakup.BlizOL.Sfera);

            robocizna.Text = _zakup.Robocizna.ToString();

            dalOPCylinder.Text = _mapper.MapujDodatnie(_zakup.DalOP.Cylinder);
            dalOPOs.Text = _zakup.DalOP.Os.ToString(osFormat);
            dalOPPryzma.Text = _zakup.DalOP.Pryzma;
            dalOPOdl.Text = _zakup.DalOP.OdlegloscZrenic.ToString();
            dalOPH.Text = _zakup.DalOP.H;
            dalOPCena.Text = _zakup.DalOP.Cena.ToString();

            dalOLCylinder.Text = _mapper.MapujDodatnie(_zakup.DalOL.Cylinder);
            dalOLOs.Text = _zakup.DalOL.Os.ToString(osFormat);
            dalOLPryzma.Text = _zakup.DalOL.Pryzma;
            dalOLOdl.Text = _zakup.DalOL.OdlegloscZrenic.ToString();
            dalOLH.Text = _zakup.DalOL.H;
            dalOLCena.Text = _zakup.DalOL.Cena.ToString();

            blizOPCylinder.Text = _mapper.MapujDodatnie(_zakup.BlizOP.Cylinder);
            blizOPOs.Text = _zakup.BlizOP.Os.ToString(osFormat);
            blizOPPryzma.Text = _zakup.BlizOP.Pryzma;
            blizOPOdl.Text = _zakup.BlizOP.OdlegloscZrenic.ToString();
            blizOPH.Text = _zakup.BlizOP.H;
            blizOPCena.Text = _zakup.BlizOP.Cena.ToString();

            blizOLCylinder.Text = _mapper.MapujDodatnie(_zakup.BlizOL.Cylinder);
            blizOLOs.Text = _zakup.BlizOL.Os.ToString(osFormat);
            blizOLPryzma.Text = _zakup.BlizOL.Pryzma;
            blizOLOdl.Text = _zakup.BlizOL.OdlegloscZrenic.ToString();
            blizOLH.Text = _zakup.BlizOL.H;
            blizOLCena.Text = _zakup.BlizOL.Cena.ToString();

            rodzajSoczewek1.Text = _zakup.RodzajSoczewek1;
            rodzajSoczewek2.Text = _zakup.RodzajSoczewek2;

            refundacja.Text = _zakup.Refundacja.ToString();
            suma.Text = _priceHelper.DajSume(_zakup).ToString();
            zadatek.Text = _zakup.Zadatek.ToString();
            doZaplaty.Text = _priceHelper.DajDoZaplaty(_zakup).ToString();

            uwagi.Text = _zakup.Description;

            UpdateSuma();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
            _context.Dispose();
        }

        private void RodzajOprawekDalCena_Validating(object sender, CancelEventArgs e)
        {
            //Walidacja(sender);
        }

        private void Walidacja(object sender)
        {
            TextBox tx = sender as TextBox;
            double test;
            if (!Double.TryParse(tx.Text, out test))
            {
                MessageBox.Show("Podaj cenę w poprawnym formacie");
            }
            else
                tx.Text = test.ToString("#,##0.00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Zapisz();
        }

        private void Zapisz()
        {
            var bledy = new List<string>();
            _zakup.BuyDate = dataZakupu.Value;
            _zakup.IsDataOdbioru = checkBox1.Checked;
            _zakup.DataOdbioru = dataOdbioru.Value;
            _zakup.NumerZlecenia = NumerZlecenia.Text;
            _zakup.RodzajOprawekDal = RodzajOprawekDal.Text;
            _zakup.RodzajOprawekBliz = RodzajOprawekBliz.Text;

            decimal wynik;
            if (decimal.TryParse(RodzajOprawekBlizCena.Text, out wynik))
            {
                _zakup.CenaOprawekBliz = wynik;
            }
            else
                bledy.Add("Zła cena rodzaj oprawki bliż");

            var cenaOprawek1 = wynik;

            if (decimal.TryParse(RodzajOprawekDalCena.Text, out wynik))
            {
                _zakup.CenaOprawekDal = wynik;
            }
            else
                bledy.Add("Zła cena rodzaj oprawki dal");

            var cenaOprawek2 = wynik;

            if (!string.IsNullOrEmpty(comboBox1.Text) && decimal.TryParse(comboBox1.Text, out wynik))
            {
                _zakup.DalOP.Sfera = wynik;
            }

            if (!string.IsNullOrEmpty(comboBox2.Text) && decimal.TryParse(comboBox2.Text, out wynik))
            {
                _zakup.DalOL.Sfera = wynik;
            }

            if (!string.IsNullOrEmpty(comboBox3.Text) && decimal.TryParse(comboBox3.Text, out wynik))
            {
                _zakup.BlizOP.Sfera = wynik;
            }

            if (!string.IsNullOrEmpty(comboBox4.Text) && decimal.TryParse(comboBox4.Text, out wynik))
            {
                _zakup.BlizOL.Sfera = wynik;
            }

            if (decimal.TryParse(robocizna.Text, out wynik))
            {
                _zakup.Robocizna = wynik;
            }
            else
                bledy.Add("Zła cena robocizna");

            var robociznaCena = wynik;

            var soczewkiDalOPCena = DalOPMap(bledy);
            var soczewkiDalOLCena = DalOLMap(bledy);
            var soczewkiBlizOPCena = BlizOPMap(bledy);
            var soczewkiBlizOLCena = BlizOLMap(bledy);


            _zakup.RodzajSoczewek1 = rodzajSoczewek1.Text;
            _zakup.RodzajSoczewek2 = rodzajSoczewek2.Text;


            if (decimal.TryParse(refundacja.Text, out wynik))
            {
                _zakup.Refundacja = wynik;
            }
            else
                bledy.Add("Zła cena refundacja");

            var refundacjaCena = wynik;

            if (decimal.TryParse(zadatek.Text, out wynik))
            {
                _zakup.Zadatek = wynik;
            }
            else
                bledy.Add("Zła cena zadatek");

            var zadatekCena = wynik;

            _zakup.Description = uwagi.Text;

            var sumka = cenaOprawek1 + cenaOprawek2 + robociznaCena + soczewkiDalOPCena
                + soczewkiDalOLCena + soczewkiBlizOPCena + soczewkiBlizOLCena - refundacjaCena;

            suma.Text = (sumka).ToString();
            doZaplaty.Text = (sumka - zadatekCena).ToString();

            Enum.TryParse(comboBox5.SelectedValue.ToString(), out FormaPlatnosci formaPlatnosci);
            _zakup.FormaPlatnosci = formaPlatnosci;

            if (bledy.Any())
            {
                var opis = string.Empty;
                foreach (var blad in bledy)
                {
                    opis += blad + Environment.NewLine;
                }
                MessageBox.Show("Skoryguj następujące błędy: " + Environment.NewLine + opis);
            }
            else
            {
                _context.Binocles.AddOrUpdate(_zakup);

                //var changes = _context.ChangeTracker.Entries();
                _context.SaveChanges();
                this.Close();
            }
        }

        private decimal DalOPMap(List<string> bledy)
        {
            decimal wynik;
            if (decimal.TryParse(dalOPCylinder.Text, out wynik))
            {
                _zakup.DalOP.Cylinder = wynik;
            }
            else
                bledy.Add("Złe pole dal OP Cylinder");

            if (decimal.TryParse(dalOPOs.Text, out wynik))
            {
                _zakup.DalOP.Os = wynik;
            }
            else
                bledy.Add("Złe pole dal OP oś");

            _zakup.DalOP.Pryzma = dalOPPryzma.Text;

            if (decimal.TryParse(dalOPOdl.Text, out wynik))
            {
                _zakup.DalOP.OdlegloscZrenic = wynik;
            }
            else
            {
                //bledy.Add("Złe pole dal OP odl.");
                _zakup.DalOP.OdlegloscZrenic = 0.0M;
            }
                

            _zakup.DalOP.H = dalOPH.Text;

            if (decimal.TryParse(dalOPCena.Text, out wynik))
            {
                _zakup.DalOP.Cena = wynik;
            }
            else
                bledy.Add("Złe pole dal OP cena");

            return wynik;
        }

        private decimal DalOLMap(List<string> bledy)
        {
            decimal wynik;
            if (decimal.TryParse(dalOLCylinder.Text, out wynik))
            {
                _zakup.DalOL.Cylinder = wynik;
            }
            else
                bledy.Add("Złe pole dal OL Cylinder");

            if (decimal.TryParse(dalOLOs.Text, out wynik))
            {
                _zakup.DalOL.Os = wynik;
            }
            else
                bledy.Add("Złe pole dal OL oś");

            _zakup.DalOL.Pryzma = dalOLPryzma.Text;

            if (decimal.TryParse(dalOLOdl.Text, out wynik))
            {
                _zakup.DalOL.OdlegloscZrenic = wynik;
            }
            else
            {
                _zakup.DalOL.OdlegloscZrenic = 0.0M;
                //bledy.Add("Złe pole dal OL odl.");
            }
                

            _zakup.DalOL.H = dalOLH.Text;

            if (decimal.TryParse(dalOLCena.Text, out wynik))
            {
                _zakup.DalOL.Cena = wynik;
            }
            else
                bledy.Add("Złe pole dal OL cena");

            return wynik;
        }

        private decimal BlizOPMap(List<string> bledy)
        {
            decimal wynik;
            if (decimal.TryParse(blizOPCylinder.Text, out wynik))
            {
                _zakup.BlizOP.Cylinder = wynik;
            }
            else
                bledy.Add("Złe pole bliz OP Cylinder");

            if (decimal.TryParse(blizOPOs.Text, out wynik))
            {
                _zakup.BlizOP.Os = wynik;
            }
            else
                bledy.Add("Złe pole bliz OP oś");

            _zakup.BlizOP.Pryzma = blizOPPryzma.Text;

            if (decimal.TryParse(blizOPOdl.Text, out wynik))
            {
                _zakup.BlizOP.OdlegloscZrenic = wynik;
            }
            else
            {
                //bledy.Add("Złe pole bliz OP odl.");
                _zakup.BlizOP.OdlegloscZrenic = 0.0M;
            }
                

            _zakup.BlizOP.H = blizOPH.Text;

            if (decimal.TryParse(blizOPCena.Text, out wynik))
            {
                _zakup.BlizOP.Cena = wynik;
            }
            else
                bledy.Add("Złe pole bliz OP cena");

            return wynik;
        }

        private decimal BlizOLMap(List<string> bledy)
        {
            decimal wynik;
            if (decimal.TryParse(blizOLCylinder.Text, out wynik))
            {
                _zakup.BlizOL.Cylinder = wynik;
            }
            else
                bledy.Add("Złe pole bliz OL Cylinder");

            if (decimal.TryParse(blizOLOs.Text, out wynik))
            {
                _zakup.BlizOL.Os = wynik;
            }
            else
                bledy.Add("Złe pole bliz OL oś");

            _zakup.BlizOL.Pryzma = blizOLPryzma.Text;

            if (decimal.TryParse(blizOLOdl.Text, out wynik))
            {
                _zakup.BlizOL.OdlegloscZrenic = wynik;
            }
            else
            {
                //bledy.Add("Złe pole bliz OL odl.");
                _zakup.BlizOL.OdlegloscZrenic = 0.0M;
            }
                

            _zakup.BlizOL.H = blizOLH.Text;

            if (decimal.TryParse(blizOLCena.Text, out wynik))
            {
                _zakup.BlizOL.Cena = wynik;
            }
            else
                bledy.Add("Złe pole bliz OL cena");

            return wynik;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void blizOLOs_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void UpdateSuma()
        {
            var dobraSuma = true;

            if (!decimal.TryParse(RodzajOprawekBlizCena.Text, out var rodzajOprawekBlizCena))
            {
                dobraSuma = false;
            }

            if (!decimal.TryParse(RodzajOprawekDalCena.Text, out var rodzajOprawekDalCena))
            {
                dobraSuma = false;
            }

            if (!decimal.TryParse(robocizna.Text, out var robociznaCena))
            {
                dobraSuma = false;
            }

            if (!decimal.TryParse(dalOPCena.Text, out var soczewkiDalOPCena))
            {
                dobraSuma = false;
            }

            if (!decimal.TryParse(dalOLCena.Text, out var soczewkiDalOLCena))
            {
                dobraSuma = false;
            }

            if (!decimal.TryParse(blizOPCena.Text, out var soczewkiBlizOPCena))
            {
                dobraSuma = false;
            }

            if (!decimal.TryParse(blizOLCena.Text, out var soczewkiBlizOLCena))
            {
                dobraSuma = false;
            }

            if (!decimal.TryParse(refundacja.Text, out var refundacjaCena))
            {
                dobraSuma = false;
            }

            var sumka = rodzajOprawekDalCena + rodzajOprawekBlizCena + robociznaCena + soczewkiDalOPCena
                + soczewkiDalOLCena + soczewkiBlizOPCena + soczewkiBlizOLCena - refundacjaCena;

            if (dobraSuma)
                suma.Text = sumka.ToString();
            else
                suma.Text = "Błąd";

            if (!decimal.TryParse(zadatek.Text, out var zadatekCena))
            {
                dobraSuma = false;
            }

            var doplaty = _zakup.Doplaty.Sum(x => x.Kwota);

            var doZaplatyCena = sumka - zadatekCena - doplaty;

            doZaplaty.Text = dobraSuma ? doZaplatyCena.ToString() : "Błąd";

            if (doZaplatyCena < 0)
            {
                MessageBox.Show("Ujemna kwota do zapłaty. Zweryfikuj czy wszystko jest w porządku.");
            }

            _zakup.NumerZlecenia = NumerZlecenia.Text;

            _zakup.RodzajOprawekDal = RodzajOprawekDal.Text;
            _zakup.RodzajOprawekBliz = RodzajOprawekBliz.Text;
            _zakup.CenaOprawekDal = rodzajOprawekDalCena;
            _zakup.CenaOprawekBliz = rodzajOprawekBlizCena;
            _zakup.Robocizna = robociznaCena;

            if (!decimal.TryParse(comboBox1.Text, out var dalOpSfera))
            {
                dalOpSfera = decimal.Zero;
            }

            if (!decimal.TryParse(comboBox2.Text, out var dalOlSfera))
            {
                dalOlSfera = decimal.Zero;
            }

            if (!decimal.TryParse(dalOPCylinder.Text, out var dalOpCylinder))
            {
                dalOpCylinder = decimal.Zero;
            }

            if (!decimal.TryParse(dalOLCylinder.Text, out var dalOlCylinder))
            {
                dalOlCylinder = decimal.Zero;
            }

            if (!decimal.TryParse(dalOPOs.Text, out var dalOpOs))
            {
                dalOpOs = decimal.Zero;
            }

            if (!decimal.TryParse(dalOLOs.Text, out var dalOlOs))
            {
                dalOlOs = decimal.Zero;
            }

            if (!decimal.TryParse(dalOPOdl.Text, out var dalOpOdl))
            {
                dalOpOdl = decimal.Zero;
            }

            if (!decimal.TryParse(dalOLOdl.Text, out var dalOlOdl))
            {
                dalOlOdl = decimal.Zero;
            }

            if (!decimal.TryParse(dalOPCena.Text, out var dalOpCena))
            {
                dalOpOdl = decimal.Zero;
            }

            if (!decimal.TryParse(dalOLCena.Text, out var dalOlCena))
            {
                dalOlOdl = decimal.Zero;
            }

            _zakup.DalOP.Sfera = dalOpSfera;
            _zakup.DalOP.Cylinder = dalOpCylinder;
            _zakup.DalOP.Os = dalOpOs;
            _zakup.DalOP.Pryzma = dalOPPryzma.Text;
            _zakup.DalOP.OdlegloscZrenic = dalOpOdl;
            _zakup.DalOP.H = dalOPH.Text;
            _zakup.DalOP.Cena = dalOpCena;
            _zakup.DalOL.Sfera = dalOlSfera;
            _zakup.DalOL.Cylinder = dalOlCylinder;
            _zakup.DalOL.Os = dalOlOs;
            _zakup.DalOL.Pryzma = dalOLPryzma.Text;
            _zakup.DalOL.OdlegloscZrenic = dalOlOdl;
            _zakup.DalOL.H = dalOLH.Text;
            _zakup.DalOL.Cena = dalOlCena;

            // bliż

            if (!decimal.TryParse(comboBox1.Text, out var blizOpSfera))
            {
                blizOpSfera = decimal.Zero;
            }

            if (!decimal.TryParse(comboBox2.Text, out var blizOlSfera))
            {
                blizOlSfera = decimal.Zero;
            }

            if (!decimal.TryParse(blizOPCylinder.Text, out var blizOpCylinder))
            {
                blizOpCylinder = decimal.Zero;
            }

            if (!decimal.TryParse(blizOLCylinder.Text, out var blizOlCylinder))
            {
                blizOlCylinder = decimal.Zero;
            }

            if (!decimal.TryParse(blizOPOs.Text, out var blizOpOs))
            {
                blizOpOs = decimal.Zero;
            }

            if (!decimal.TryParse(blizOLOs.Text, out var blizOlOs))
            {
                blizOlOs = decimal.Zero;
            }

            if (!decimal.TryParse(blizOPOdl.Text, out var blizOpOdl))
            {
                blizOpOdl = decimal.Zero;
            }

            if (!decimal.TryParse(blizOLOdl.Text, out var blizOlOdl))
            {
                blizOlOdl = decimal.Zero;
            }

            if (!decimal.TryParse(blizOPCena.Text, out var blizOpCena))
            {
                blizOpOdl = decimal.Zero;
            }

            if (!decimal.TryParse(blizOLCena.Text, out var blizOlCena))
            {
                blizOlOdl = decimal.Zero;
            }

            _zakup.BlizOP.Sfera = blizOpSfera;
            _zakup.BlizOP.Cylinder = blizOpCylinder;
            _zakup.BlizOP.Os = blizOpOs;
            _zakup.BlizOP.Pryzma = blizOPPryzma.Text;
            _zakup.BlizOP.OdlegloscZrenic = blizOpOdl;
            _zakup.BlizOP.H = blizOPH.Text;
            _zakup.BlizOP.Cena = blizOpCena;
            _zakup.BlizOL.Sfera = blizOlSfera;
            _zakup.BlizOL.Cylinder = blizOlCylinder;
            _zakup.BlizOL.Os = blizOlOs;
            _zakup.BlizOL.Pryzma = blizOLPryzma.Text;
            _zakup.BlizOL.OdlegloscZrenic = blizOlOdl;
            _zakup.BlizOL.H = blizOLH.Text;
            _zakup.BlizOL.Cena = blizOlCena;

            _zakup.RodzajSoczewek1 = rodzajSoczewek1.Text;
            _zakup.RodzajSoczewek2 = rodzajSoczewek2.Text;
            _zakup.Refundacja = refundacjaCena;

            _zakup.Zadatek = zadatekCena;
            Enum.TryParse(comboBox5.SelectedValue.ToString(), out FormaPlatnosci formaPlatnosci);
            _zakup.FormaPlatnosci = formaPlatnosci;

            _zakup.IsDataOdbioru = checkBox1.Checked;
            _zakup.Description = uwagi.Text;
        }

        private void RodzajOprawekDalCena_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _rodzajOprawekDalCenaFocused = false;
        }

        private void RodzajOprawekBlizCena_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _rodzajOprawekBlizCenaFocused = false;
        }

        private void robocizna_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _robociznaFocused = false;
        }

        private void dalOPCena_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _dalOPCenaFocused = false;
        }

        private void dalOLCena_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _dalOLCenaFocused = false;
        }

        private void blizOPCena_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _blizOPCenaFocused = false;
        }

        private void blizOLCena_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _blizOLCenaFocused = false;
        }

        private void refundacja_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _refundacjaFocused = false;
        }

        private void zadatek_Leave(object sender, EventArgs e)
        {
            UpdateSuma();
            _zadatekFocused = false;
        }

        private void RodzajOprawekDalCena_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.RodzajOprawekDalCena.SelectAll();
                _rodzajOprawekDalCenaFocused = true;
            }
        }

        private void RodzajOprawekDalCena_MouseLeave(object sender, EventArgs e)
        {

        }

        private void RodzajOprawekDalCena_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_rodzajOprawekDalCenaFocused && this.RodzajOprawekDalCena.SelectionLength == 0)
            {
                _rodzajOprawekDalCenaFocused = true;
                this.RodzajOprawekDalCena.SelectAll();
            }
        }

        private void RodzajOprawekBlizCena_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_rodzajOprawekBlizCenaFocused && this.RodzajOprawekBlizCena.SelectionLength == 0)
            {
                _rodzajOprawekBlizCenaFocused = true;
                this.RodzajOprawekBlizCena.SelectAll();
            }
        }

        private void RodzajOprawekBlizCena_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.RodzajOprawekBlizCena.SelectAll();
                _rodzajOprawekBlizCenaFocused = true;
            }
        }

        private void robocizna_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_robociznaFocused && this.robocizna.SelectionLength == 0)
            {
                _robociznaFocused = true;
                this.robocizna.SelectAll();
            }
        }

        private void dalOPCylinder_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOPCylinderFocused && this.dalOPCylinder.SelectionLength == 0)
            {
                _dalOPCylinderFocused = true;
                this.dalOPCylinder.SelectAll();
            }
        }

        private void dalOLCylinder_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOLCylinderFocused && this.dalOLCylinder.SelectionLength == 0)
            {
                _dalOLCylinderFocused = true;
                this.dalOLCylinder.SelectAll();
            }
        }

        private void blizOPCylinder_TextChanged(object sender, EventArgs e)
        {

        }

        private void blizOPCylinder_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOPCylinderFocused && this.blizOPCylinder.SelectionLength == 0)
            {
                _blizOPCylinderFocused = true;
                this.blizOPCylinder.SelectAll();
            }
        }

        private void blizOLCylinder_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOLVylinderFocused && this.blizOLCylinder.SelectionLength == 0)
            {
                _blizOLVylinderFocused = true;
                this.blizOLCylinder.SelectAll();
            }
        }

        private void dalOPOs_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOPOsFocused && this.dalOPOs.SelectionLength == 0)
            {
                _dalOPOsFocused = true;
                this.dalOPOs.SelectAll();
            }
        }

        private void dalOLOs_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOLOsFocused && this.dalOLOs.SelectionLength == 0)
            {
                _dalOLOsFocused = true;
                this.dalOLOs.SelectAll();
            }
        }

        private void blizOPOs_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOPOsFocused && this.blizOPOs.SelectionLength == 0)
            {
                _blizOPOsFocused = true;
                this.blizOPOs.SelectAll();
            }
        }

        private void blizOLOs_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOLOsFocused && this.blizOLOs.SelectionLength == 0)
            {
                _blizOLOsFocused = true;
                this.blizOLOs.SelectAll();
            }
        }

        private void dalOPOdl_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOPOdlFocused && this.dalOPOdl.SelectionLength == 0)
            {
                _dalOPOdlFocused = true;
                this.dalOPOdl.SelectAll();
            }
        }

        private void dalOLOdl_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOLOdlFocused && this.dalOLOdl.SelectionLength == 0)
            {
                _dalOLOdlFocused = true;
                this.dalOLOdl.SelectAll();
            }
        }

        private void blizOPOdl_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOPOdlFocused && this.blizOPOdl.SelectionLength == 0)
            {
                _blizOPOdlFocused = true;
                this.blizOPOdl.SelectAll();
            }
        }

        private void blizOLOdl_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOLOdlFocused && this.blizOLOdl.SelectionLength == 0)
            {
                _blizOLOdlFocused = true;
                this.blizOLOdl.SelectAll();
            }
        }

        private void dalOPCena_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOPCenaFocused && this.dalOPCena.SelectionLength == 0)
            {
                _dalOPCenaFocused = true;
                this.dalOPCena.SelectAll();
            }
        }

        private void dalOLCena_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_dalOLCenaFocused && this.dalOLCena.SelectionLength == 0)
            {
                _dalOLCenaFocused = true;
                this.dalOLCena.SelectAll();
            }
        }

        private void blizOPCena_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOPCenaFocused && this.blizOPCena.SelectionLength == 0)
            {
                _blizOPCenaFocused = true;
                this.blizOPCena.SelectAll();
            }
        }

        private void blizOLCena_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_blizOLCenaFocused && this.blizOLCena.SelectionLength == 0)
            {
                _blizOLCenaFocused = true;
                this.blizOLCena.SelectAll();
            }
        }

        private void refundacja_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_refundacjaFocused && this.refundacja.SelectionLength == 0)
            {
                _refundacjaFocused = true;
                this.refundacja.SelectAll();
            }
        }

        private void zadatek_MouseUp(object sender, MouseEventArgs e)
        {
            if (!_zadatekFocused && this.zadatek.SelectionLength == 0)
            {
                _zadatekFocused = true;
                this.zadatek.SelectAll();
            }
        }

        private void dalOPCylinder_Leave(object sender, EventArgs e)
        {
            _dalOPCylinderFocused = false;
        }

        private void dalOLCylinder_Leave(object sender, EventArgs e)
        {
            _dalOLCylinderFocused = false;
        }

        private void blizOPCylinder_Leave(object sender, EventArgs e)
        {
            _blizOPCylinderFocused = false;
        }

        private void blizOLCylinder_Leave(object sender, EventArgs e)
        {
            _blizOLVylinderFocused = false;
        }

        private void dalOPOs_Leave(object sender, EventArgs e)
        {
            _dalOPOsFocused = false;
        }

        private void dalOLOs_Leave(object sender, EventArgs e)
        {
            _dalOLOsFocused = false;
        }

        private void blizOPOs_Leave(object sender, EventArgs e)
        {
            _blizOPOsFocused = false;
        }

        private void blizOLOs_Leave(object sender, EventArgs e)
        {
            _blizOLOsFocused = false;
        }

        private void dalOPOdl_Leave(object sender, EventArgs e)
        {
            _dalOPOdlFocused = false;
        }

        private void dalOLOdl_Leave(object sender, EventArgs e)
        {
            _dalOLOdlFocused = false;
        }

        private void blizOPOdl_Leave(object sender, EventArgs e)
        {
            _blizOPOdlFocused = false;
        }

        private void blizOLOdl_Leave(object sender, EventArgs e)
        {
            _blizOLOdlFocused = false;
        }

        private void robocizna_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.robocizna.SelectAll();
                _robociznaFocused = true;
            }
        }

        private void dalOPCylinder_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOPCylinder.SelectAll();
                _dalOPCylinderFocused = true;
            }
        }

        private void dalOLCylinder_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOLCylinder.SelectAll();
                _dalOLCylinderFocused = true;
            }
        }

        private void blizOPCylinder_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOPCylinder.SelectAll();
                _blizOPCylinderFocused = true;
            }
        }

        private void blizOLCylinder_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOLCylinder.SelectAll();
                _blizOLVylinderFocused = true;
            }
        }

        private void dalOPOs_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOPOs.SelectAll();
                _dalOPOsFocused = true;
            }
        }

        private void dalOLOs_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOLOs.SelectAll();
                _dalOLOsFocused = true;
            }
        }

        private void blizOPOs_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOPOs.SelectAll();
                _blizOPOsFocused = true;
            }
        }

        private void blizOLOs_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOLOs.SelectAll();
                _blizOLOsFocused = true;
            }
        }

        private void dalOPOdl_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOPOdl.SelectAll();
                _dalOPOdlFocused = true;
            }
        }

        private void dalOLOdl_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOLOdl.SelectAll();
                _dalOLOdlFocused = true;
            }
        }

        private void blizOPOdl_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOPOdl.SelectAll();
                _blizOPOdlFocused = true;
            }
        }

        private void blizOLOdl_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOLOdl.SelectAll();
                _blizOLOdlFocused = true;
            }
        }

        private void dalOPCena_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOPCena.SelectAll();
                _dalOPCenaFocused = true;
            }
        }

        private void dalOLCena_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.dalOLCena.SelectAll();
                _dalOLCenaFocused = true;
            }
        }

        private void blizOPCena_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOPCena.SelectAll();
                _blizOPCenaFocused = true;
            }
        }

        private void blizOLCena_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.blizOLCena.SelectAll();
                _blizOLCenaFocused = true;
            }
        }

        private void refundacja_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.refundacja.SelectAll();
                _refundacjaFocused = true;
            }
        }

        private void zadatek_Enter(object sender, EventArgs e)
        {
            if (MouseButtons == MouseButtons.None)
            {
                this.zadatek.SelectAll();
                _zadatekFocused = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Zapisz();
            var pdfGenerator = new PDFGenerator();
            pdfGenerator.Generate(_zakup, _person);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            SetCheckbox();
        }

        private void SetCheckbox()
        {
            dataOdbioru.Enabled = checkBox1.Checked;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            using (var childForm = new Doplaty(_binocleId, _zakup.Doplaty))
            {
                childForm.FormClosing += new FormClosingEventHandler(Sprzedaz_Refresh);
                var result = childForm.ShowDialog();

                if (result == DialogResult.OK)
                {
                    _zakup.Doplaty = new List<Doplata>(childForm.DoplatyZakup);
                    UpdateSuma();
                    Mapuj();
                }
            }
        }

        private void Sprzedaz_Refresh(object sender, FormClosingEventArgs e)
        {
            //Mapuj();
        }
    }
}
