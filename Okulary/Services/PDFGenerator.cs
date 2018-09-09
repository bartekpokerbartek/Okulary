using System;
using System.Configuration;
using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.IO.Util;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Okulary.Helpers;
using Okulary.Model;

namespace Okulary.Services
{
    public class PDFGenerator
    {
        public readonly PriceHelper _priceHelper = new PriceHelper();
        public readonly Mapper _mapper = new Mapper();

        public string format = "dd.MM.yyyy";
        public string osFormat = "N0";

        public void Generate(Binocle okulary, Person osoba)
        {
            var DEST = ConfigurationManager.AppSettings["PdfFolder"].ToString() + osoba.LastName + "_" + osoba.FirstName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".pdf";

            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            CreatePdf(DEST, okulary, osoba);

            System.Diagnostics.Process.Start(DEST);
        }

        public virtual void CreatePdf(String dest, Binocle okulary, Person osoba)
        {
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            //Document document = new Document(pdf, PageSize.A4);
            Document document = new Document(pdf, new PageSize(623, 1058));

            document.SetMargins(20, 20, 20, 40);
            //document.SetMargins(0, 0, 0, 0);

            //BaseFont courier = BaseFont.createFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.EMBEDDED);
            //Font font = new Font(courier, 12, Font.NORMAL);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN, PdfEncodings.CP1250, true);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

            ////var uri = new Uri("c:\\koperta.jpg");

            ////Image img = new Image((ImageDataFactory.Create(uri)));
            ////document.Add(img.SetFixedPosition(0, 0));

            ////document.Add(new Paragraph("Testing").SetFixedPosition(20, 100, 50));
            ////document.Add(new Paragraph("Testing").SetFixedPosition(70, 100, 50));

            DrawHeader(document, font, okulary, osoba);

            DrawSection1(document, font, okulary, osoba);

            DrawSection2(document, font, okulary);

            Draw10x5Table(document, font, okulary);

            DrawSection3(document, font, okulary);

            DrawSection4(document, font, okulary);

            DrawSection5(document, font, okulary, osoba);

            //Close document
            document.Close();
        }

        private void DrawSection5(Document document, PdfFont font, Binocle okulary, Person osoba)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 55),
                new UnitValue(UnitValue.PERCENT, 45)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddCell(new Cell().Add(new Paragraph("Podpis zleceniodawcy").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Podpis i pieczęć zleceniobiorcy").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph(osoba.FirstName + " " + osoba.LastName).SetFont(font)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Oświadczam, że zapoznałem(am) się z instrukcją użytkownika wyrobu i akceptuję warunki umowy. Swoje dane osobowe przekazuję dobrowolnie i jednocześnie zastrzegam sobie prawo do ich sprawdzania i poprawiania.").SetFont(font).SetFontSize(8)));

            document.Add(table);
        }

        private void DrawSection4(Document document, PdfFont font, Binocle okulary)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 15),
                new UnitValue(UnitValue.PERCENT, 65),
                new UnitValue(UnitValue.PERCENT, 5),
                new UnitValue(UnitValue.PERCENT, 15)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddCell(new Cell(4, 1).Add(new Paragraph("Zalecenia i uwagi").SetFont(font)));
            table.AddCell(new Cell(4, 1).Add(new Paragraph(okulary.Description).SetFont(font)));
            table.AddCell(new Cell(4, 1).Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));

            table.AddCell(new Cell().Add(new Paragraph("Zadatek").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.Zadatek.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));
            table.AddCell(new Cell().Add(new Paragraph("Do zapłaty").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_priceHelper.DajDoZaplaty(okulary).ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            document.Add(table);

            document.Add(new Paragraph());
        }

        private void DrawSection3(Document document, PdfFont font, Binocle okulary)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 5),
                new UnitValue(UnitValue.PERCENT, 63),
                new UnitValue(UnitValue.PERCENT, 5),
                new UnitValue(UnitValue.PERCENT, 15)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddCell(new Cell(4, 1).Add(new Paragraph("Rodzaj socz.").SetFont(font)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("1").SetFont(font)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph(okulary.RodzajSoczewek1).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph()).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Refundacja").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph()).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));

            //table.AddCell(new Cell(2, 1).Add(new Paragraph("Rodzaj socz.").SetFont(font)));
            //table.AddCell(new Cell().Add(new Paragraph("2").SetFont(font)));
            //table.AddCell(new Cell().Add(new Paragraph("Opis rodzaju soczewek 2").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.Refundacja.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            table.AddCell(new Cell(2, 1).Add(new Paragraph("2").SetFont(font)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph(okulary.RodzajSoczewek2).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph()).SetFont(font).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Suma").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph()).SetFont(font).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(_priceHelper.DajSume(okulary).ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            document.Add(table);

            document.Add(new Paragraph());
        }

        private void DrawSection2(Document document, PdfFont font, Binocle okulary)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 20),
                new UnitValue(UnitValue.PERCENT, 60),
                new UnitValue(UnitValue.PERCENT, 5),
                new UnitValue(UnitValue.PERCENT, 15)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Cena oprawek").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Oprawki dal").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.RodzajOprawekDal).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.CenaOprawekDal.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            table.AddCell(new Cell().Add(new Paragraph("Oprawki bliż").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.RodzajOprawekBliz).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.CenaOprawekBliz.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Robocizna").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.Robocizna.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            document.Add(table);

            document.Add(new Paragraph());
        }

        private void DrawSection1(Document document, PdfFont font, Binocle okulary, Person osoba)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 34),
                new UnitValue(UnitValue.PERCENT, 33),
                new UnitValue(UnitValue.PERCENT, 33)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddHeaderCell(new Cell().Add(new Paragraph("Data przyjęcia").SetFont(font)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Data odbioru").SetFont(font)));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Numer telefonu").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph(DateTime.Today.ToString(format)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DataOdbioru.ToString(format)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.NumerZlecenia).SetFont(font)));

            document.Add(table);

            document.Add(new Paragraph());

            var table2 = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 100)
            }
            );
            table2.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table2.AddHeaderCell(new Cell().Add(new Paragraph("Imię i nazwisko").SetFont(font)).SetBorderBottom(Border.NO_BORDER));
            table2.AddCell(new Cell().Add(new Paragraph(osoba.FirstName + " " + osoba.LastName).SetFont(font)));
            table2.AddCell(new Cell().Add(new Paragraph("Adres").SetFont(font)));
            table2.AddCell(new Cell().Add(new Paragraph(osoba.Address ?? "Brak").SetFont(font)));

            document.Add(table2);

            document.Add(new Paragraph());
        }

        private void DrawHeader(Document document, PdfFont font, Binocle okulary, Person person)
        {
            PdfFont whiteFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN, PdfEncodings.CP1250, true);
            
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 32),
                new UnitValue(UnitValue.PERCENT, 3),
                new UnitValue(UnitValue.PERCENT, 31),
                new UnitValue(UnitValue.PERCENT, 3),
                new UnitValue(UnitValue.PERCENT, 31)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            //table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)));
            document.Add(new Paragraph(person.FirstName + " " + person.LastName).SetFont(font).SetTextAlignment(TextAlignment.CENTER));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Numer telefonu").SetFont(font)));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Data odbioru").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.NumerZlecenia).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            var dataOdbioru = okulary.IsDataOdbioru ? okulary.DataOdbioru.ToString(format) : "Brak";
            //var fontColor = new ColorConstants.
            if (okulary.IsDataOdbioru)
                table.AddCell(new Cell().Add(new Paragraph(dataOdbioru).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));
            else
                table.AddCell(new Cell().Add(new Paragraph(dataOdbioru).SetFont(font).SetFontColor(ColorConstants.WHITE).SetTextAlignment(TextAlignment.CENTER)));


            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Zadatek").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Do zapłaty").SetFont(font)));

            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph(okulary.Zadatek.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph(_priceHelper.DajDoZaplaty(okulary).ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE)));

            document.Add(table);

            document.Add(new Paragraph());
            document.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------------------"));
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            document.Add(new Paragraph());
            //document.Add(new Paragraph("Test")).SetFixedPosition(10, 150, 100);

            document.Add(new Paragraph());
        }

        private void Draw10x5Table(Document document, PdfFont font, Binocle okulary)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 13),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 7),
                new UnitValue(UnitValue.PERCENT, 5),
                new UnitValue(UnitValue.PERCENT, 15)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Sfera").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Cylinder").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Oś").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Pryzma").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Odl. źr.").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("H").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Cena socz.").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Dal OP").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.DalOP.Sfera)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.DalOP.Cylinder)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOP.Os.ToString(osFormat)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOP.Pryzma.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOP.OdlegloscZrenic.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOP.H).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOP.Cena.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            table.AddCell(new Cell().Add(new Paragraph("Dal OL").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.DalOL.Sfera)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.DalOL.Cylinder)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOL.Os.ToString(osFormat)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOL.Pryzma.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOL.OdlegloscZrenic.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOL.H).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.DalOL.Cena.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            table.AddCell(new Cell().Add(new Paragraph("Bliż OP").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.BlizOP.Sfera)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.BlizOP.Cylinder)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOP.Os.ToString(osFormat)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOP.Pryzma.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOP.OdlegloscZrenic.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOP.H).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOP.Cena.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            table.AddCell(new Cell().Add(new Paragraph("Bliż OL").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.BlizOL.Sfera)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(_mapper.MapujDodatnie(okulary.BlizOL.Cylinder)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOL.Os.ToString(osFormat)).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOL.Pryzma.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOL.OdlegloscZrenic.ToString()).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOL.H).SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph(okulary.BlizOL.Cena.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER)));

            document.Add(table);

            document.Add(new Paragraph());
        }

        public virtual void Process(Table table, String line, PdfFont font, bool isHeader)
        {
            StringTokenizer tokenizer = new StringTokenizer(line, ";");
            while (tokenizer.HasMoreTokens())
            {
                if (isHeader)
                {
                    table.AddHeaderCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)));
                }
                else
                {
                    table.AddCell(new Cell().Add(new Paragraph(tokenizer.NextToken()).SetFont(font)));
                }
            }
        }
    }
}
