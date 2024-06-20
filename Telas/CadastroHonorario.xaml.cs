using LawForm.Model;
using System.Windows;

namespace LawForm.Telas
{
    public partial class CadastroHonorario : Window
    {
        private DataContext _context;

        public CadastroHonorario()
        {
            InitializeComponent();
            _context = new DataContext();
            LoadClientes();
        }

        private void LoadClientes()
        {
            var clientesPF = _context.ClientePF.Select(c => new ClienteViewModel
            {
                Id = c.Id,
                DisplayName = $"{c.Nome} - {c.DocumentoCPF}",
                TipoCliente = "PF"
            }).ToList();

            var clientesPJ = _context.ClientePJ.Select(c => new ClienteViewModel
            {
                Id = c.Id,
                DisplayName = $"{c.NomeEmpresa} - {c.Cnpj}",
                TipoCliente = "PJ"
            }).ToList();

            var allClientes = new List<ClienteViewModel>();
            allClientes.AddRange(clientesPF);
            allClientes.AddRange(clientesPJ);

            ClienteComboBox.ItemsSource = allClientes;
        }

        private void CadastrarButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClienteComboBox.SelectedItem is ClienteViewModel selectedCliente &&
                decimal.TryParse(ValorTotalHonorarioTextBox.Text, out decimal valorTotalHonorario) &&
                int.TryParse(NumeroParcelasTextBox.Text, out int numeroParcelas) &&
                DataInicioPicker.SelectedDate.HasValue)
            {
                var dataInicio = DataInicioPicker.SelectedDate.Value;
                decimal valorParcela = valorTotalHonorario / numeroParcelas;

                if (selectedCliente.TipoCliente == "PF" && _context.ClientePF.Any(c => c.Id == selectedCliente.Id))
                {
                    var novoHonorarioPF = new ClientePFHonorario
                    {
                        ClientePFId = selectedCliente.Id,
                        ValorTotalHonorario = valorTotalHonorario,
                        NumeroParcelas = numeroParcelas,
                        DataInicio = dataInicio,
                        Pagamentos = new List<PagamentoHonorario>()
                    };

                    for (int i = 0; i < numeroParcelas; i++)
                    {
                        novoHonorarioPF.Pagamentos.Add(new PagamentoHonorario
                        {
                            DataPagamento = dataInicio.AddMonths(i),
                            Valor = valorParcela,
                            Pago = false
                        });
                    }

                    _context.ClientePFHonorarios.Add(novoHonorarioPF);
                }
                else if (selectedCliente.TipoCliente == "PJ" && _context.ClientePJ.Any(c => c.Id == selectedCliente.Id))
                {
                    var novoHonorarioPJ = new ClientePJHonorario
                    {
                        ClientePJId = selectedCliente.Id,
                        ValorTotalHonorario = valorTotalHonorario,
                        NumeroParcelas = numeroParcelas,
                        DataInicio = dataInicio,
                        Pagamentos = new List<PagamentoHonorario>()
                    };

                    for (int i = 0; i < numeroParcelas; i++)
                    {
                        novoHonorarioPJ.Pagamentos.Add(new PagamentoHonorario
                        {
                            DataPagamento = dataInicio.AddMonths(i),
                            Valor = valorParcela,
                            Pago = false
                        });
                    }

                    _context.ClientePJHonorarios.Add(novoHonorarioPJ);
                }

                _context.SaveChanges();
                MessageBox.Show("Honorário cadastrado com sucesso!");
            }
            else
            {
                MessageBox.Show("Por favor, preencha todos os campos corretamente.");
            }
        }
    }

    public class ClienteViewModel
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string TipoCliente { get; set; }
    }
}
