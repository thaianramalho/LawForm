using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using LawForm.Model;
using Microsoft.EntityFrameworkCore;

namespace LawForm.Telas
{
    public partial class Honorarios : Window
    {
        private DataContext _context;

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

            var allHonorarios = new List<HonorarioViewModel>();

            foreach (var honorarioPF in honorariosPF)
            {
                allHonorarios.AddRange(honorarioPF.Pagamentos.Select(p => new HonorarioViewModel
                {
                    Nome = honorarioPF.ClientePF.Nome,
                    Documento = honorarioPF.ClientePF.DocumentoCPF,
                    ValorHonorario = p.Valor,
                    DataVencimento = p.DataPagamento.ToString("dd/MM/yyyy"),
                    Pago = p.Pago,
                    PagamentoId = p.Id
                }));
            }

            foreach (var honorarioPJ in honorariosPJ)
            {
                allHonorarios.AddRange(honorarioPJ.Pagamentos.Select(p => new HonorarioViewModel
                {
                    Nome = honorarioPJ.ClientePJ.NomeEmpresa,
                    Documento = honorarioPJ.ClientePJ.Cnpj,
                    ValorHonorario = p.Valor,
                    DataVencimento = p.DataPagamento.ToString("dd/MM/yyyy"),
                    Pago = p.Pago,
                    PagamentoId = p.Id
                }));
            }

            HonorariosDataGrid.ItemsSource = allHonorarios;
        }

        private void CadastrarHonorarioButton_Click(object sender, RoutedEventArgs e)
        {
            var cadastroHonorarioWindow = new CadastroHonorario();
            cadastroHonorarioWindow.ShowDialog();
            LoadHonorarios(); // Recarrega a lista após fechar a tela de cadastro
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
    }

    public class HonorarioViewModel
    {
        public string Nome { get; set; }
        public string Documento { get; set; }
        public decimal ValorHonorario { get; set; }
        public string DataVencimento { get; set; }
        public bool Pago { get; set; }
        public int PagamentoId { get; set; }
    }
}
