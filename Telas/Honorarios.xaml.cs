using LawForm.Model;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace LawForm.Telas
{
    public partial class Honorarios : Window
    {
        private DataContext _context;
        private List<HonorarioViewModel> _allHonorarios;

        public Honorarios()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadHonorarios();
        }

        private void LoadHonorarios()
        {
            var honorariosPF = _context.ClientePFHonorarios
                .Include(h => h.ClientePF)
                .Include(h => h.Pagamentos)
                .ToList();

            var honorariosPJ = _context.ClientePJHonorarios
                .Include(h => h.ClientePJ)
                .Include(h => h.Pagamentos)
                .ToList();

            _allHonorarios = new List<HonorarioViewModel>();

            foreach (var honorarioPF in honorariosPF)
            {
                _allHonorarios.AddRange(honorarioPF.Pagamentos.Select(p => new HonorarioViewModel
                {
                    Nome = honorarioPF.ClientePF.Nome,
                    Documento = honorarioPF.ClientePF.DocumentoCPF,
                    Telefones = honorarioPF.ClientePF.Telefones,
                    ValorHonorario = p.Valor,
                    DataVencimento = p.DataPagamento,
                    Pago = p.Pago,
                    PagamentoId = p.Id
                }));
            }

            foreach (var honorarioPJ in honorariosPJ)
            {
                _allHonorarios.AddRange(honorarioPJ.Pagamentos.Select(p => new HonorarioViewModel
                {
                    Nome = honorarioPJ.ClientePJ.NomeEmpresa,
                    Documento = honorarioPJ.ClientePJ.Cnpj,
                    Telefones = honorarioPJ.ClientePJ.Telefones,
                    ValorHonorario = p.Valor,
                    DataVencimento = p.DataPagamento,
                    Pago = p.Pago,
                    PagamentoId = p.Id
                }));
            }

            _allHonorarios = _allHonorarios
                .OrderBy(h => h.DataVencimento)
                .ThenBy(h => h.Pago)
                .ToList();

            HonorariosDataGrid.ItemsSource = _allHonorarios;
        }

        private void CadastrarHonorarioButton_Click(object sender, RoutedEventArgs e)
        {
            var cadastroHonorarioWindow = new CadastroHonorario();
            cadastroHonorarioWindow.ShowDialog();
            LoadHonorarios();
        }

        private void EditarPagamentoButton_Click(object sender, RoutedEventArgs e)
        {
            if (HonorariosDataGrid.SelectedItem is HonorarioViewModel selectedHonorario)
            {
                var pagamento = _context.PagamentosHonorarios.SingleOrDefault(p => p.Id == selectedHonorario.PagamentoId);
                if (pagamento != null)
                {
                    var editarPagamentoHonorario = new EditarPagamentoHonorario(pagamento);
                    editarPagamentoHonorario.ShowDialog();
                    LoadHonorarios();
                }
            }
        }

        private void HonorariosAReceberButton_Click(object sender, RoutedEventArgs e)
        {
            var honorariosAReceber = _allHonorarios.Where(h => !h.Pago).ToList();
            HonorariosDataGrid.ItemsSource = honorariosAReceber;
            subtituloPagina.Text = "Honorários a Receber";
        }

        private void HonorariosRecebidosButton_Click(object sender, RoutedEventArgs e)
        {
            var honorariosRecebidos = _allHonorarios.Where(h => h.Pago).ToList();
            HonorariosDataGrid.ItemsSource = honorariosRecebidos;
            subtituloPagina.Text = "Honorários Recebidos";
        }
    }

    public class HonorarioViewModel
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public string Telefones { get; set; }
        public decimal ValorHonorario { get; set; }
        public DateTime DataVencimento { get; set; }
        public bool Pago { get; set; }
        public int PagamentoId { get; set; }
    }

    public class DateToPastConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date < DateTime.Now;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
