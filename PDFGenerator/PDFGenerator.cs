using System;
using System.IO;
using iText.IO.Font;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.IO.Util;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using Okulary.Model;

namespace PDFGenerator
{
    public class PDFGenerator
    {
        public const String DEST = "results/chapter01/hello_world.pdf";

        public void Generate(Binocle okulary)
        {
            FileInfo file = new FileInfo(DEST);
            file.Directory.Create();
            CreatePdf(DEST);
        }

        public virtual void CreatePdf(String dest)
        {
            //Initialize PDF writer
            PdfWriter writer = new PdfWriter(dest);
            //Initialize PDF document
            PdfDocument pdf = new PdfDocument(writer);
            // Initialize document
            //Document document = new Document(pdf, PageSize.A4);
            Document document = new Document(pdf, new PageSize(623, 1058));

            //document.SetMargins(20, 20, 20, 50);
            document.SetMargins(0, 0, 0, 0);

            //BaseFont courier = BaseFont.createFont(BaseFont.COURIER, BaseFont.CP1252, BaseFont.EMBEDDED);
            //Font font = new Font(courier, 12, Font.NORMAL);

            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN, PdfEncodings.CP1250, true);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD);

            var uri = new Uri("c:\\koperta.jpg");

            Image img = new Image((ImageDataFactory.Create(uri)));
            document.Add(img.SetFixedPosition(0, 0));


            document.Add(new Paragraph("Testing").SetFixedPosition(20, 100, 50));
            document.Add(new Paragraph("Testing").SetFixedPosition(70, 100, 50));

            //DrawHeader(document, font);

            //DrawSection1(document, font);

            //DrawSection2(document, font);

            //Draw10x5Table(document, font);

            //DrawSection3(document, font);

            //DrawSection4(document, font);

            //DrawSection5(document, font);

            //Close document
            document.Close();
        }

        private void DrawSection5(Document document, PdfFont font)
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

            table.AddCell(new Cell().Add(new Paragraph("Paweł Test").SetFont(font)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("Rafał Test").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Oświadczam, że zapoznałem(am) się z instrukcją użytkownika wyrobu i akceptuję warunki umowy. Swoje dane osobowe przekazuję dobrowolnie i jednocześnie zastrzegam sobie prawo do ich sprawdzania i poprawiania.").SetFont(font).SetFontSize(8)));

            document.Add(table);
        }

        private void DrawSection4(Document document, PdfFont font)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 15),
                new UnitValue(UnitValue.PERCENT, 70),
                new UnitValue(UnitValue.PERCENT, 15)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddCell(new Cell(4, 1).Add(new Paragraph("Zalecenia i uwagi").SetFont(font)));
            table.AddCell(new Cell(4, 1).Add(new Paragraph("Do komputera").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Zadatek").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("70").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Do zapłaty").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("70").SetFont(font)));

            document.Add(table);

            document.Add(new Paragraph());
        }

        private void DrawSection3(Document document, PdfFont font)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 5),
                new UnitValue(UnitValue.PERCENT, 68),
                new UnitValue(UnitValue.PERCENT, 15)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddCell(new Cell(4, 1).Add(new Paragraph("Rodzaj socz.").SetFont(font)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("1").SetFont(font).SetFontSize(20)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("Opis rodzaju soczewek").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Refundacja").SetFont(font)));

            //table.AddCell(new Cell(2, 1).Add(new Paragraph("Rodzaj socz.").SetFont(font)));
            //table.AddCell(new Cell().Add(new Paragraph("2").SetFont(font)));
            //table.AddCell(new Cell().Add(new Paragraph("Opis rodzaju soczewek 2").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("70").SetFont(font)));

            table.AddCell(new Cell(2, 1).Add(new Paragraph("2").SetFont(font)));
            table.AddCell(new Cell(2, 1).Add(new Paragraph("Opis rodzaju soczewek 2").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Suma").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("50").SetFont(font)));

            document.Add(table);

            document.Add(new Paragraph());
        }

        private void DrawSection2(Document document, PdfFont font)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 20),
                new UnitValue(UnitValue.PERCENT, 65),
                new UnitValue(UnitValue.PERCENT, 15)
            }
            );
            table.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Cena oprawek").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Oprawki dal").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Opis oprawek dal").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("100").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Oprawki bliż").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Opis oprawek bliż").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("100").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Robocizna").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderRight(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER).SetBorderLeft(Border.NO_BORDER).SetBorderTop(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("50").SetFont(font)));

            document.Add(table);

            document.Add(new Paragraph());
        }

        private void DrawSection1(Document document, PdfFont font)
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
            table.AddHeaderCell(new Cell().Add(new Paragraph("Numer zlecenia").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("2018/06/06").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("2018/06/07").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("Zlecenie 666").SetFont(font)));

            document.Add(table);

            document.Add(new Paragraph());

            var table2 = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 100)
            }
            );
            table2.SetWidth(new UnitValue(UnitValue.PERCENT, 100));

            table2.AddHeaderCell(new Cell().Add(new Paragraph("Imię i nazwisko").SetFont(font)).SetBorderBottom(Border.NO_BORDER));
            table2.AddCell(new Cell().Add(new Paragraph("Paweł Test").SetFont(font)));
            table2.AddCell(new Cell().Add(new Paragraph("Adres").SetFont(font)));
            table2.AddCell(new Cell().Add(new Paragraph("ul. Grunwaldzka 111, 36-065 Dynów").SetFont(font)));

            document.Add(table2);

            document.Add(new Paragraph());
        }

        private void DrawHeader(Document document, PdfFont font)
        {
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
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderBottom(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Numer zlecenia").SetFont(font)));
            table.AddHeaderCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddHeaderCell(new Cell().Add(new Paragraph("Data odbioru").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Zlecenie 666").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("2018/06/06").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Zadatek").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().Add(new Paragraph("Do zapłaty").SetFont(font)));

            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph("100").SetFont(font)));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph().SetFont(font)).SetBorderTop(Border.NO_BORDER).SetBorderBottom(Border.NO_BORDER));
            table.AddCell(new Cell().SetHeight(20).Add(new Paragraph("200").SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE)));

            document.Add(table);

            document.Add(new Paragraph());
            document.Add(new Paragraph("-----------------------------------------------------------------------------------------------------------------------------------"));

            document.Add(new Paragraph("Test")).SetFixedPosition(10, 150, 100);

            document.Add(new Paragraph());
        }

        private void Draw10x5Table(Document document, PdfFont font)
        {
            var table = new Table(new UnitValue[]
            {
                new UnitValue(UnitValue.PERCENT, 13),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
                new UnitValue(UnitValue.PERCENT, 12),
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
            table.AddCell(new Cell().Add(new Paragraph("Cena socz.").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Dal OP").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("3").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("60").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Dal OL").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("3").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("60").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Bliż OP").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("3").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("60").SetFont(font)));

            table.AddCell(new Cell().Add(new Paragraph("Bliż OL").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("12").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("3").SetFont(font)));
            table.AddCell(new Cell().Add(new Paragraph("60").SetFont(font)));

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
