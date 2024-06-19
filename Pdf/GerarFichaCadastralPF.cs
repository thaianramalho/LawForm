using System.Diagnostics;
using System.IO;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Layout.Borders;
using iText.Kernel.Geom;
using iText.Kernel.Events;
using LawForm.Model;
using iText.Kernel.Pdf.Canvas;

namespace LawForm.Pdf
{
    public class GerarFichaCadastralPF
    {
        private readonly ClientePF _clientePF;

        public GerarFichaCadastralPF(ClientePF clientePF)
        {
            _clientePF = clientePF ?? throw new ArgumentNullException(nameof(clientePF));
        }

        public void GerarPdf()
        {
            Debug.WriteLine("GerarPdf method called");

            string filename = $@".\FichaCadastral_{_clientePF.Nome.Replace(" ", "_")}.pdf";
            using (PdfWriter writer = new PdfWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
            {
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4);
                document.SetMargins(160, 25, 100, 25);

                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new HeaderFooterEventHandler(_clientePF.Nome));

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

                Paragraph subtitle = new Paragraph("Pessoa Física")
                    .SetFontSize(18)
                    .SetFontColor(titleColor)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(subtitle);

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Dados Pessoais", backgroundColor, whiteColor);

                Table dadosPessoaisTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3 })).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                AddClienteInfo(dadosPessoaisTable, "Nome", _clientePF.Nome, textColor);
                AddClienteInfo(dadosPessoaisTable, "Filiação Pai", _clientePF.FiliacaPai, textColor);
                AddClienteInfo(dadosPessoaisTable, "Filiação Mãe", _clientePF.FiliacaoMae, textColor);
                AddClienteInfo(dadosPessoaisTable, "Nacionalidade", _clientePF.Nacionalidade, textColor);
                AddClienteInfo(dadosPessoaisTable, "Estado Civil", _clientePF.EstadoCivil, textColor);
                AddClienteInfo(dadosPessoaisTable, "Profissão", _clientePF.Profissao, textColor);
                AddClienteInfo(dadosPessoaisTable, "Documento CI", _clientePF.DocumentoCI, textColor);
                AddClienteInfo(dadosPessoaisTable, "Documento CPF", _clientePF.DocumentoCPF, textColor);
                document.Add(dadosPessoaisTable);

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Documentos", backgroundColor, whiteColor);

                Table documentosTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3 })).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                AddClienteInfo(documentosTable, "Documento PIS", _clientePF.DocumentoPIS, textColor);
                AddClienteInfo(documentosTable, "Documento CTPS", _clientePF.DocumentoCTPS, textColor);
                AddClienteInfo(documentosTable, "Documento Série", _clientePF.DocumentoSerie, textColor);
                document.Add(documentosTable);

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Contato", backgroundColor, whiteColor);

                Table contatoTable = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3 })).UseAllAvailableWidth().SetBorder(Border.NO_BORDER);
                AddClienteInfo(contatoTable, "Endereço", _clientePF.Endereco, textColor);
                AddClienteInfo(contatoTable, "Telefones", _clientePF.Telefones, textColor);
                AddClienteInfo(contatoTable, "Naturalidade", _clientePF.Naturalidade, textColor);
                AddClienteInfo(contatoTable, "Data de Nascimento", _clientePF.DataNascimento.ToString("dd/MM/yyyy"), textColor);
                AddClienteInfo(contatoTable, "Email", _clientePF.Email, textColor);
                AddHistoricoInfo(contatoTable, "Histórico", _clientePF.Historico, textColor);
                document.Add(contatoTable);

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
            private readonly string _nomeCliente;

            public HeaderFooterEventHandler(string nomeCliente)
            {
                _nomeCliente = nomeCliente;
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
                    logo.ScaleToFit(300f, 300f);
                    logo.SetFixedPosition(xHeader - 150, yHeader - 100);
                    canvas.Add(logo);
                }

                canvas.ShowTextAligned(new Paragraph("______________________________"), xFooter, yFooter, TextAlignment.CENTER);
                canvas.ShowTextAligned(new Paragraph(_nomeCliente), xFooter, yFooter - 15, TextAlignment.CENTER);

                canvas.Close();
            }
        }
    }
}
