using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Events;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System.Diagnostics;
using System.IO;

namespace LawForm.Pdf
{
    public class GerarDocumentosNecessarios
    {
        public void GerarPdf()
        {
            Debug.WriteLine("GerarPdf method called");

            string filename = @".\relacao_documentos.pdf";
            using (PdfWriter writer = new PdfWriter(new FileStream(filename, FileMode.Create, FileAccess.Write)))
            {
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4);
                document.SetMargins(120, 25, 100, 25);

                pdf.AddEventHandler(PdfDocumentEvent.END_PAGE, new HeaderFooterEventHandler());

                Color titleColor = new DeviceRgb(0, 0, 0);
                Color textColor = new DeviceRgb(60, 60, 60);  // Cinza mais escuro
                Color backgroundColor = new DeviceRgb(128, 128, 128);  // Cinza médio para o fundo
                Color whiteColor = new DeviceRgb(255, 255, 255);  // Branco para o texto

                Paragraph title = new Paragraph("Relação de Documentos")
                    .SetFontSize(22)
                    .SetFontColor(titleColor)
                    .SetTextAlignment(TextAlignment.CENTER);
                document.Add(title);

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Documentos de Identificação", backgroundColor, whiteColor);
                document.Add(CreateDocumentList(new[]
                {
                    "RG ou CNH",
                    "CPF",
                    "Comprovante de residência (atualizado)"
                }, textColor));

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Informações Pessoais", backgroundColor, whiteColor);
                document.Add(CreateDocumentList(new[]
                {
                    "Dados de contato (telefone, e-mail)",
                    "Estado civil",
                    "Nome dos pais"
                }, textColor));

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Documentos Relacionados ao Caso", backgroundColor, whiteColor);
                document.Add(CreateDocumentList(new[]
                {
                    "Contratos ou acordos (se houver)",
                    "Recibos ou comprovantes de pagamento",
                    "Correspondências ou e-mails relevantes",
                    "Fotos, vídeos ou outros tipos de evidências"
                }, textColor));

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Documentos Específicos ao Tipo de Caso", backgroundColor, whiteColor);
                document.Add(CreateDocumentList(new[]
                {
                    "Para Processos Trabalhistas:",
                    "   - Carteira de Trabalho (CTPS)",
                    "   - Contrato de trabalho",
                    "   - Holerites",
                    "   - Aviso prévio (se aplicável)",
                    "   - Extrato do FGTS",
                    "Para Processos de Família (Divórcio, Guarda, etc.):",
                    "   - Certidão de casamento",
                    "   - Certidão de nascimento dos filhos",
                    "   - Comprovantes de renda de ambos os cônjuges",
                    "   - Declaração de bens e dívidas",
                    "Para Processos de Imóveis:",
                    "   - Escritura do imóvel",
                    "   - Contrato de compra e venda",
                    "   - IPTU",
                    "   - Comprovantes de pagamento de aluguel (se aplicável)",
                    "Para Processos de Consumidor:",
                    "   - Nota fiscal ou recibo de compra",
                    "   - Contratos de prestação de serviço",
                    "   - Provas de comunicação com o fornecedor (e-mails, mensagens)",
                    "Para Processos de Saúde:",
                    "   - Relatórios médicos",
                    "   - Receitas e exames",
                    "   - Comprovantes de despesas médicas"
                }, textColor));

                document.Add(new Paragraph("\n"));

                AddSectionTitle(document, "Outras Informações", backgroundColor, whiteColor);
                document.Add(CreateDocumentList(new[]
                {
                    "Testemunhas (nome, endereço, telefone)",
                    "Histórico detalhado dos fatos e eventos relacionados ao caso"
                }, textColor));

                document.Close();
                writer.Close();
            }

            Debug.WriteLine($"PDF saved to {filename}");
            Process.Start(new ProcessStartInfo { FileName = filename, UseShellExecute = true });
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

        private iText.Layout.Element.List CreateDocumentList(string[] items, Color textColor)
        {
            var list = new iText.Layout.Element.List().SetSymbolIndent(12).SetListSymbol("\u2022");
            foreach (var item in items)
            {
                ListItem listItem = new ListItem(item);
                listItem.SetFontColor(textColor).SetFontSize(12);
                list.Add(listItem);
            }
            return list;
        }

        private class HeaderFooterEventHandler : IEventHandler
        {
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
                canvas.Close();
            }
        }
    }
}
