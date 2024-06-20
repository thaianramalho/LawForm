using System;
using System.Windows;
using LawForm.Model;

namespace LawForm.Telas
{
    public partial class EditarPagamentoHonorario : Window
    {
        private PagamentoHonorario _pagamento;

        public EditarPagamentoHonorario(PagamentoHonorario pagamento)
        {
            InitializeComponent();
            _pagamento = pagamento;
            LoadPagamento();
        }

        private void LoadPagamento()
        {
            ValorPagamentoTextBox.Text = _pagamento.Valor.ToString();
            DataPagamentoPicker.SelectedDate = _pagamento.DataPagamento;
            PagoCheckBox.IsChecked = _pagamento.Pago;
        }

        private void SalvarButton_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(ValorPagamentoTextBox.Text, out decimal valor) &&
                DataPagamentoPicker.SelectedDate.HasValue)
            {
                _pagamento.Valor = valor;
                _pagamento.DataPagamento = DataPagamentoPicker.SelectedDate.Value;
                _pagamento.Pago = PagoCheckBox.IsChecked ?? false;

                using (var context = new DataContext())
                {
                    context.PagamentosHonorarios.Update(_pagamento);
                    context.SaveChanges();
                }

                MessageBox.Show("Pagamento atualizado com sucesso!");
                Close();
            }
            else
            {
                MessageBox.Show("Por favor, preencha todos os campos corretamente.");
            }
        }
    }
}
