using LawForm.Model;
using LawForm.Pdf;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace LawForm
{
    public partial class MainWindow : Window
    {
        public MainWindow(string userName)
        {
            InitializeComponent();
            // Atualizar os contadores com dados reais
            advogadosCount.Text = GetAdvogadosCount().ToString();
            clientesPFCount.Text = GetClientesPFCount().ToString();
            clientesPJCount.Text = GetClientesPJCount().ToString();
            userNameTextBlock.Text = $"Olá, {userName}";
        }

        private void advogadosScreen_Click(object sender, RoutedEventArgs e)
        {
            Advogados telaAdvogados = new Advogados();
            telaAdvogados.Show();
        }

        private void clientesPFScreen_Click(object sender, RoutedEventArgs e)
        {
            ClientesPF clientesPF = new ClientesPF();
            clientesPF.Show();
        }

        private void clientesPJScreen_Click(object sender, RoutedEventArgs e)
        {
            ClientesPJ clientesPJ = new ClientesPJ();
            clientesPJ.Show();
        }

        private void emitirListaDocumentos_Click(object sender, RoutedEventArgs e)
        {
            var gerarDocumentosNecessarios = new GerarDocumentosNecessarios();
            gerarDocumentosNecessarios.GerarPdf();
             
        }



        private int GetAdvogadosCount()
        {
            using (var context = new DataContext())
            {
                return context.Advogado.Count();
            }
        }

        private int GetClientesPFCount()
        {
            using (var context = new DataContext())
            {
                return context.ClientePF.Count();
            }
        }

        private int GetClientesPJCount()
        {
            using (var context = new DataContext())
            {
                return context.ClientePJ.Count();
            }
        }
    }
}
