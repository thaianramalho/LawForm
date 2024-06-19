using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LawForm.Model;
using LawForm.Pdf;

namespace LawForm
{
    public partial class ClientesPF : Window
    {
        private int? clientePFIdSelecionado = null;
        private int paginaAtual = 1;
        private int tamanhoPagina = 10;
        private int totalPaginas;
        private string filtroBusca = string.Empty;

        public DateOnly DataNascimento { get; set; }
        public string DataNascimentoFormatada => DataNascimento.ToString("dd/MM/yyyy");


        public ClientesPF()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            using (var context = new DataContext())
            {
                var query = context.ClientePF.AsQueryable();

                var clientes = query
                                .OrderBy(c => c.Nome)
                                .ToList();

                if (!string.IsNullOrWhiteSpace(filtroBusca))
                {
                    clientes = clientes.Where(c =>
                        c.Nome.Contains(filtroBusca) ||
                        c.FiliacaPai.Contains(filtroBusca) ||
                        c.FiliacaoMae.Contains(filtroBusca) ||
                        c.Nacionalidade.Contains(filtroBusca) ||
                        c.EstadoCivil.Contains(filtroBusca) ||
                        c.Profissao.Contains(filtroBusca) ||
                        c.DocumentoCI.Contains(filtroBusca) ||
                        c.DocumentoCPF.Contains(filtroBusca) ||
                        c.DocumentoPIS.Contains(filtroBusca) ||
                        c.DocumentoCTPS.Contains(filtroBusca) ||
                        c.DocumentoSerie.Contains(filtroBusca) ||
                        c.Endereco.Contains(filtroBusca) ||
                        c.Telefones.Contains(filtroBusca) ||
                        c.Naturalidade.Contains(filtroBusca) ||
                        c.Email.Contains(filtroBusca) ||
                        c.Historico.Contains(filtroBusca) ||
                        c.DataNascimentoFormatada.Contains(filtroBusca)).ToList();
                }

                var totalRegistros = clientes.Count();
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina);

                var clientesPaginados = clientes
                                        .Skip((paginaAtual - 1) * tamanhoPagina)
                                        .Take(tamanhoPagina)
                                        .ToList();

                dataGridClientesPF.ItemsSource = clientesPaginados;
                AtualizarTextoPaginaAtual();
            }
        }



        private void AtualizarTextoPaginaAtual()
        {
            txtPaginaAtual.Text = $"Página {paginaAtual} de {totalPaginas}";
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_nome.Text) ||
                string.IsNullOrWhiteSpace(txt_nacionalidade.Text) ||
                string.IsNullOrWhiteSpace(txt_estadoCivil.Text) ||
                string.IsNullOrWhiteSpace(txt_documentoCPF.Text) ||
                string.IsNullOrWhiteSpace(txt_endereco.Text) ||
                string.IsNullOrWhiteSpace(txt_telefones.Text) ||
                string.IsNullOrWhiteSpace(txt_naturalidade.Text) ||
                string.IsNullOrWhiteSpace(txt_dataNascimento.Text) ||
                string.IsNullOrWhiteSpace(txt_historico.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos obrigatórios.");
                return;
            }

            DateOnly dataNascimento;
            if (!DateOnly.TryParseExact(txt_dataNascimento.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataNascimento))
            {
                MessageBox.Show("Data de Nascimento inválida. Use o formato dd/MM/yyyy.");
                return;
            }

            using (var context = new DataContext())
            {
                var clientePF = new ClientePF
                {
                    Nome = txt_nome.Text,
                    FiliacaPai = txt_filiacaoPai.Text,
                    FiliacaoMae = txt_filiacaoMae.Text,
                    Nacionalidade = txt_nacionalidade.Text,
                    EstadoCivil = txt_estadoCivil.Text,
                    Profissao = txt_profissao.Text,
                    DocumentoCI = txt_documentoCI.Text,
                    DocumentoCPF = txt_documentoCPF.Text,
                    DocumentoPIS = txt_documentoPIS.Text,
                    DocumentoCTPS = txt_documentoCTPS.Text,
                    DocumentoSerie = txt_documentoSerie.Text,
                    Endereco = txt_endereco.Text,
                    Telefones = txt_telefones.Text,
                    Naturalidade = txt_naturalidade.Text,
                    DataNascimento = dataNascimento,
                    Email = txt_email.Text,
                    Historico = txt_historico.Text
                };

                context.ClientePF.Add(clientePF);
                context.SaveChanges();
            }

            LimparCampos();
            CarregarDados();
        }

        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            if (clientePFIdSelecionado == null)
            {
                MessageBox.Show("Nenhum cliente selecionado para atualização.");
                return;
            }

            DateOnly dataNascimento;
            if (!DateOnly.TryParseExact(txt_dataNascimento.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dataNascimento))
            {
                MessageBox.Show("Data de Nascimento inválida. Use o formato dd/MM/yyyy.");
                return;
            }

            using (var context = new DataContext())
            {
                var clientePF = context.ClientePF.Find(clientePFIdSelecionado);
                if (clientePF != null)
                {
                    clientePF.Nome = txt_nome.Text;
                    clientePF.FiliacaPai = txt_filiacaoPai.Text;
                    clientePF.FiliacaoMae = txt_filiacaoMae.Text;
                    clientePF.Nacionalidade = txt_nacionalidade.Text;
                    clientePF.EstadoCivil = txt_estadoCivil.Text;
                    clientePF.Profissao = txt_profissao.Text;
                    clientePF.DocumentoCI = txt_documentoCI.Text;
                    clientePF.DocumentoCPF = txt_documentoCPF.Text;
                    clientePF.DocumentoPIS = txt_documentoPIS.Text;
                    clientePF.DocumentoCTPS = txt_documentoCTPS.Text;
                    clientePF.DocumentoSerie = txt_documentoSerie.Text;
                    clientePF.Endereco = txt_endereco.Text;
                    clientePF.Telefones = txt_telefones.Text;
                    clientePF.Naturalidade = txt_naturalidade.Text;
                    clientePF.DataNascimento = dataNascimento;
                    clientePF.Email = txt_email.Text;
                    clientePF.Historico = txt_historico.Text;

                    context.SaveChanges();
                }
            }

            LimparCampos();
            CarregarDados();
            cadastrar.Visibility = Visibility.Visible;
            atualizar.Visibility = Visibility.Collapsed;
            clientePFIdSelecionado = null;
        }


        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int id = (int)button.Tag;

                using (var context = new DataContext())
                {
                    var clientePF = context.ClientePF.Find(id);
                    if (clientePF != null)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem certeza?", "Confirmar exclusão", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            context.ClientePF.Remove(clientePF);
                            context.SaveChanges();
                            CarregarDados();
                        }
                    }
                }
            }
        }

        private void DataGridClientesPF_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridClientesPF.SelectedItem is ClientePF clienteSelecionado)
            {
                clientePFIdSelecionado = clienteSelecionado.Id;
                txt_nome.Text = clienteSelecionado.Nome ?? string.Empty;
                txt_filiacaoPai.Text = clienteSelecionado.FiliacaPai ?? string.Empty;
                txt_filiacaoMae.Text = clienteSelecionado.FiliacaoMae ?? string.Empty;
                txt_nacionalidade.Text = clienteSelecionado.Nacionalidade ?? string.Empty;
                txt_estadoCivil.Text = clienteSelecionado.EstadoCivil ?? string.Empty;
                txt_profissao.Text = clienteSelecionado.Profissao ?? string.Empty;
                txt_documentoCI.Text = clienteSelecionado.DocumentoCI ?? string.Empty;
                txt_documentoCPF.Text = clienteSelecionado.DocumentoCPF ?? string.Empty;
                txt_documentoPIS.Text = clienteSelecionado.DocumentoPIS ?? string.Empty;
                txt_documentoCTPS.Text = clienteSelecionado.DocumentoCTPS ?? string.Empty;
                txt_documentoSerie.Text = clienteSelecionado.DocumentoSerie ?? string.Empty;
                txt_endereco.Text = clienteSelecionado.Endereco ?? string.Empty;
                txt_telefones.Text = clienteSelecionado.Telefones ?? string.Empty;
                txt_naturalidade.Text = clienteSelecionado.Naturalidade ?? string.Empty;
                txt_dataNascimento.Text = clienteSelecionado.DataNascimentoFormatada ?? string.Empty; 
                txt_email.Text = clienteSelecionado.Email ?? string.Empty;
                txt_historico.Text = clienteSelecionado.Historico ?? string.Empty;

                cadastrar.Visibility = Visibility.Collapsed;
                atualizar.Visibility = Visibility.Visible;
            }
            else
            {
                LimparCampos();
                clientePFIdSelecionado = null;
                cadastrar.Visibility = Visibility.Visible;
                atualizar.Visibility = Visibility.Collapsed;
            }
        }




        private void LimparCampos()
        {
            txt_nome.Clear();
            txt_filiacaoPai.Clear();
            txt_filiacaoMae.Clear();
            txt_nacionalidade.Clear();
            txt_estadoCivil.Clear();
            txt_profissao.Clear();
            txt_documentoCI.Clear();
            txt_documentoCPF.Clear();
            txt_documentoPIS.Clear();
            txt_documentoCTPS.Clear();
            txt_documentoSerie.Clear();
            txt_endereco.Clear();
            txt_telefones.Clear();
            txt_naturalidade.Clear();
            txt_dataNascimento.Clear();
            txt_email.Clear();
            txt_historico.Clear();
            dataGridClientesPF.SelectedItem = null;
        }

        private void PrimeiraPagina_Click(object sender, RoutedEventArgs e)
        {
            paginaAtual = 1;
            CarregarDados();
        }

        private void PaginaAnterior_Click(object sender, RoutedEventArgs e)
        {
            if (paginaAtual > 1)
            {
                paginaAtual--;
                CarregarDados();
            }
        }

        private void ProximaPagina_Click(object sender, RoutedEventArgs e)
        {
            if (paginaAtual < totalPaginas)
            {
                paginaAtual++;
                CarregarDados();
            }
        }

        private void UltimaPagina_Click(object sender, RoutedEventArgs e)
        {
            paginaAtual = totalPaginas;
            CarregarDados();
        }

        private void Buscar_Click(object sender, RoutedEventArgs e)
        {
            filtroBusca = txtBuscar.Text.Trim();
            paginaAtual = 1;
            CarregarDados();
        }

        private void LimparBusca_Click(object sender, RoutedEventArgs e)
        {
            txtBuscar.Clear();
            filtroBusca = string.Empty;
            paginaAtual = 1;
            CarregarDados();
        }


        private void EmitirFicha_Click(object sender, RoutedEventArgs e)
        {
            if (clientePFIdSelecionado == null)
            {
                MessageBox.Show("Nenhum cliente selecionado.");
                return;
            }

            using (var context = new DataContext())
            {
                var clientePF = context.ClientePF.Find(clientePFIdSelecionado);
                if (clientePF != null)
                {
                    var gerarFicha = new GerarFichaCadastralPF(clientePF);
                    gerarFicha.GerarPdf();
                }
                else
                {
                    Debug.WriteLine("ClientePF não encontrado.");
                }
            }
        }






    }
}
