using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Borders;
using iText.Layout.Element;
using iText.Layout.Properties;
using LawForm.Model;
using System.Diagnostics;
using System.IO;

namespace LawForm.Pdf
{
    public class GerarFichaCadastralPJ
    {
        private readonly ClientePJ _clientePJ;

        public GerarFichaCadastralPJ(ClientePJ clientePJ)
        {
            _clientePJ = clientePJ ?? throw new ArgumentNullException(nameof(clientePJ));
        }

        public void GerarPdf()
        {
            Debug.WriteLine("GerarPdf method called");

            string filename = $@".\FichaCadastral_{_clientePJ.NomeEmpresa.Replace(" ", "_")}.pdf";
            using (PdfWriter writer = new PdfWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
            {
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4);
                document.SetMargins(120, 25, 100, 25);

                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new HeaderFooterEventHandler(_clientePJ.NomeResponsavel));

                Color titleColor = new DeviceRgb(0, 0, 0);
                Color headerColor = new DeviceRgb(90, 90, 90);  // Cinza médio
                Color textColor = new DeviceRgb(60, 60, 60);  // Cinza mais escuro
                Color backgroundColor = new DeviceRgb(128, 128, 128);  // Cinza médio para o fundo
                Color whiteColor = new DeviceRgb(255, 255, 255);  // Branco para o texto

                Paragraph title = new Paragraph("Ficha Cadastral de Clientes")
                    .SetFontSize(22)
                    .SetFontColor(titleColor)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);

                Paragraph subtitle = new Paragraph("Pessoa Jurídica")
                    .SetFontSize(18)
                    .SetFontColor(titleColor)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(subtitle);

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Dados da Empresa", backgroundColor, whiteColor);

                Table dadosEmpresaTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3 })).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                AddClienteInfo(dadosEmpresaTable, "Nome da Empresa", _clientePJ.NomeEmpresa, textColor);
                AddClienteInfo(dadosEmpresaTable, "CNPJ", _clientePJ.Cnpj, textColor);
                AddClienteInfo(dadosEmpresaTable, "Inscrição Estadual", _clientePJ.InscricaoEstadual, textColor);
                AddClienteInfo(dadosEmpresaTable, "Natureza Jurídica", _clientePJ.NaturezaJuridica, textColor);
                AddClienteInfo(dadosEmpresaTable, "Endereço", _clientePJ.EnderecoEmpresa, textColor);
                AddClienteInfo(dadosEmpresaTable, "Telefones", _clientePJ.TelefonesEmpresa, textColor);
                AddClienteInfo(dadosEmpresaTable, "Email", _clientePJ.EmailEmpresa, textColor);
                document.Add(dadosEmpresaTable);

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Dados do Responsável", backgroundColor, whiteColor);

                Table dadosResponsavelTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3 })).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                AddClienteInfo(dadosResponsavelTable, "Nome do Responsável", _clientePJ.NomeResponsavel, textColor);
                AddClienteInfo(dadosResponsavelTable, "Filiação Pai", _clientePJ.FiliacaPai, textColor);
                AddClienteInfo(dadosResponsavelTable, "Filiação Mãe", _clientePJ.FiliacaoMae, textColor);
                AddClienteInfo(dadosResponsavelTable, "Nacionalidade", _clientePJ.Nacionalidade, textColor);
                AddClienteInfo(dadosResponsavelTable, "Estado Civil", _clientePJ.EstadoCivil, textColor);
                AddClienteInfo(dadosResponsavelTable, "Profissão", _clientePJ.Profissao, textColor);
                AddClienteInfo(dadosResponsavelTable, "Documento CI", _clientePJ.DocumentoCI, textColor);
                AddClienteInfo(dadosResponsavelTable, "Documento CPF", _clientePJ.DocumentoCPF, textColor);
                AddClienteInfo(dadosResponsavelTable, "Documento PIS", _clientePJ.DocumentoPIS, textColor);
                AddClienteInfo(dadosResponsavelTable, "Documento CTPS", _clientePJ.DocumentoCTPS, textColor);
                AddClienteInfo(dadosResponsavelTable, "Documento Série", _clientePJ.DocumentoSerie, textColor);
                AddClienteInfo(dadosResponsavelTable, "Endereço", _clientePJ.Endereco, textColor);
                AddClienteInfo(dadosResponsavelTable, "Telefones", _clientePJ.Telefones, textColor);
                AddClienteInfo(dadosResponsavelTable, "Naturalidade", _clientePJ.Naturalidade, textColor);
                AddClienteInfo(dadosResponsavelTable, "Data de Nascimento", _clientePJ.DataNascimentoFormatada, textColor);
                AddClienteInfo(dadosResponsavelTable, "Email", _clientePJ.Email, textColor);
                AddHistoricoInfo(dadosResponsavelTable, "Histórico", _clientePJ.Historico, textColor);
                document.Add(dadosResponsavelTable);

                document.Close();
                writer.Close();
            }

            Debug.WriteLine($"PDF saved to {filename}");
            Process.Start(new ProcessStartInfo { FileName = filename, UseShellExecute = true });
        }

        private void AddClienteInfo(Table table, string label, string value, Color textColor)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                table.AddCell(new Cell().Add(new Paragraph(label)).SetBorder(Border.NO_BORDER).SetPaddingBottom(5).SetPaddingTop(5).SetFontColor(textColor).SetFontSize(12).SetBold());
                table.AddCell(new Cell().Add(new Paragraph(value)).SetBorder(Border.NO_BORDER).SetPaddingBottom(5).SetPaddingTop(5).SetFontColor(textColor));
            }
        }

        private void AddHistoricoInfo(Table table, string label, string value, Color textColor)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                table.AddCell(new Cell(1, 1)
                    .Add(new Paragraph(label))
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingBottom(5)
                    .SetPaddingTop(5)
                    .SetFontColor(textColor)
                    .SetFontSize(12)
                    .SetBold());

                table.AddCell(new Cell(1, 1)
                    .SetBorder(Border.NO_BORDER));

                Cell valueCell = new Cell(1, table.GetNumberOfColumns())
                    .Add(new Paragraph(value))
                    .SetBorder(Border.NO_BORDER)
                    .SetPaddingBottom(5)
                    .SetPaddingTop(5)
                    .SetFontColor(textColor);

                table.AddCell(valueCell);
            }
        }

        private void AddSectionTitle(Document document, string titleText, Color backgroundColor, Color textColor)
        {
            Paragraph titleParagraph = new Paragraph(titleText)
                .SetFontSize(16)
                .SetBold()
                .SetFontColor(textColor)
                .SetBackgroundColor(backgroundColor)
                .SetPadding(5)
                .SetMarginBottom(10);
            document.Add(titleParagraph);
        }

        private class HeaderFooterEventHandler : IEventHandler
        {
            private readonly string _nomeResponsavel;

            public HeaderFooterEventHandler(string nomeResponsavel)
            {
                _nomeResponsavel = nomeResponsavel;
            }

            public void HandleEvent(Event @event)
            {
                PdfDocumentEvent docEvent = (PdfDocumentEvent)@event;
                PdfDocument pdfDoc = docEvent.GetDocument();
                PdfPage page = docEvent.GetPage();
                Rectangle pageSize = page.GetPageSize();
                PdfCanvas pdfCanvas = new PdfCanvas(page.NewContentStreamAfter(), page.GetResources(), pdfDoc);

                float xHeader = pageSize.GetWidth() / 2;
                float yHeader = pageSize.GetTop() - 50;

                float xFooter = pageSize.GetWidth() / 2;
                float yFooter = pageSize.GetBottom() + 30;

                Canvas canvas = new Canvas(pdfCanvas, new Rectangle(0, 0, pageSize.GetWidth(), pageSize.GetHeight()));
                canvas.SetFontSize(12);

                string imagePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\img\varejao-doc-logo.png");
                if (File.Exists(imagePath))
                {
                    ImageData imageData = ImageDataFactory.Create(imagePath);
                    Image logo = new Image(imageData);
                    logo.ScaleToFit(200f, 200f);
                    logo.SetFixedPosition(xHeader - 100, yHeader - 50);
                    canvas.Add(logo);
                }

                canvas.ShowTextAligned(new Paragraph("______________________________"), xFooter, yFooter, TextAlignment.CENTER);
                canvas.ShowTextAligned(new Paragraph(_nomeResponsavel), xFooter, yFooter - 15, TextAlignment.CENTER);

                canvas.Close();
            }
        }
    }
}
