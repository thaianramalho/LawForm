using LawForm.Model;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.IO;

namespace LawForm.Pdf
{
    public class GerarFichaCadastralPF
    {
        private readonly ClientePF _clientePF;

        public GerarFichaCadastralPF(ClientePF clientePF)
        {
            _clientePF = clientePF;
        }

        public void GerarPdf()
        {
            Debug.WriteLine("GerarPdf method called");

            Document document = new Document(PageSize.A4, 50, 50, 100, 50);
            string filename = $@".\FichaCadastral_{_clientePF.Nome.Replace(" ", "_")}.pdf";
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
            writer.PageEvent = new PdfPageEvents();

            document.Open();

            // Cabeçalho
            string imagePath = "img/varejao-doc-logo.png";
            if (File.Exists(imagePath))
            {
                Image logo = Image.GetInstance(imagePath);
                logo.ScaleToFit(140f, 120f);
                logo.Alignment = Element.ALIGN_CENTER;
                document.Add(logo);
            }

            Font titleFont = FontFactory.GetFont("Arial", 20, Font.BOLD);
            Paragraph title = new Paragraph("Ficha Cadastral", titleFont)
            {
                Alignment = Element.ALIGN_CENTER
            };
            document.Add(title);

            document.Add(new Chunk("\n"));

            // Dados do Cliente
            Font headerFont = FontFactory.GetFont("Arial", 14, Font.BOLD);
            Font normalFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            document.Add(new Paragraph("Dados Pessoais", headerFont));
            AddClienteInfo(document, "Nome", _clientePF.Nome, normalFont);
            AddClienteInfo(document, "Filiação Pai", _clientePF.FiliacaPai, normalFont);
            AddClienteInfo(document, "Filiação Mãe", _clientePF.FiliacaoMae, normalFont);
            AddClienteInfo(document, "Nacionalidade", _clientePF.Nacionalidade, normalFont);
            AddClienteInfo(document, "Estado Civil", _clientePF.EstadoCivil, normalFont);
            AddClienteInfo(document, "Profissão", _clientePF.Profissao, normalFont);
            AddClienteInfo(document, "Documento CI", _clientePF.DocumentoCI, normalFont);
            AddClienteInfo(document, "Documento CPF", _clientePF.DocumentoCPF, normalFont);

            document.Add(new Paragraph("\nDocumentos", headerFont));
            AddClienteInfo(document, "Documento PIS", _clientePF.DocumentoPIS, normalFont);
            AddClienteInfo(document, "Documento CTPS", _clientePF.DocumentoCTPS, normalFont);
            AddClienteInfo(document, "Documento Série", _clientePF.DocumentoSerie, normalFont);

            document.Add(new Paragraph("\nContato", headerFont));
            AddClienteInfo(document, "Endereço", _clientePF.Endereco, normalFont);
            AddClienteInfo(document, "Telefones", _clientePF.Telefones, normalFont);
            AddClienteInfo(document, "Naturalidade", _clientePF.Naturalidade, normalFont);
            AddClienteInfo(document, "Data de Nascimento", _clientePF.DataNascimento.ToString("dd/MM/yyyy"), normalFont);
            AddClienteInfo(document, "Email", _clientePF.Email, normalFont);
            AddClienteInfo(document, "Histórico", _clientePF.Historico, normalFont);

            document.Close();
            writer.Close();

            Debug.WriteLine($"PDF saved to {filename}");
            Process.Start(new ProcessStartInfo { FileName = filename, UseShellExecute = true });
        }

        private void AddClienteInfo(Document document, string label, string value, Font font)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                document.Add(new Paragraph($"{label}: {value}", font));
            }
        }

        private class PdfPageEvents : PdfPageEventHelper
        {
            public override void OnEndPage(PdfWriter writer, Document document)
            {
                PdfPTable footerTable = new PdfPTable(1);
                footerTable.TotalWidth = document.PageSize.Width - 100;
                footerTable.DefaultCell.Border = 0;
                footerTable.AddCell(new PdfPCell(new Phrase("Assinatura do Cliente:", FontFactory.GetFont("Arial", 12, Font.NORMAL)))
                {
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingTop = 30
                });
                footerTable.AddCell(new PdfPCell(new Phrase("__________________________________________", FontFactory.GetFont("Arial", 12, Font.NORMAL)))
                {
                    Border = 0,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    PaddingBottom = 30
                });
                footerTable.WriteSelectedRows(0, -1, 50, 50, writer.DirectContent);
            }
        }
    }
}
