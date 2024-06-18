using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LawForm.Model;
using LawForm.Pdf;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace LawForm
{
    public partial class ClientesPF : Window
    {
        private int? clientePFIdSelecionado = null;
        private int paginaAtual = 1;
        private int tamanhoPagina = 10;
        private int totalPaginas;
        private string filtroBusca = string.Empty;

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

                if (!string.IsNullOrWhiteSpace(filtroBusca))
                {
                    query = query.Where(c =>
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
                        c.DataNascimento.ToString().Contains(filtroBusca));
                }

                var totalRegistros = query.Count();
                totalPaginas = (int)Math.Ceiling((double)totalRegistros / tamanhoPagina);

                var clientes = query
                                .OrderBy(c => c.Nome)
                                .Skip((paginaAtual - 1) * tamanhoPagina)
                                .Take(tamanhoPagina)
                                .ToList();

                dataGridClientesPF.ItemsSource = clientes;
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
                    DataNascimento = DateOnly.Parse(txt_dataNascimento.Text),
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
                    clientePF.DataNascimento = DateOnly.Parse(txt_dataNascimento.Text);
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
            if (dataGridClientesPF.SelectedItem is ClientePF clientePF)
            {
                clientePFIdSelecionado = clientePF.Id;
                txt_nome.Text = clientePF.Nome ?? string.Empty;
                txt_filiacaoPai.Text = clientePF.FiliacaPai ?? string.Empty;
                txt_filiacaoMae.Text = clientePF.FiliacaoMae ?? string.Empty;
                txt_nacionalidade.Text = clientePF.Nacionalidade ?? string.Empty;
                txt_estadoCivil.Text = clientePF.EstadoCivil ?? string.Empty;
                txt_profissao.Text = clientePF.Profissao ?? string.Empty;
                txt_documentoCI.Text = clientePF.DocumentoCI ?? string.Empty;
                txt_documentoCPF.Text = clientePF.DocumentoCPF ?? string.Empty;
                txt_documentoPIS.Text = clientePF.DocumentoPIS ?? string.Empty;
                txt_documentoCTPS.Text = clientePF.DocumentoCTPS ?? string.Empty;
                txt_documentoSerie.Text = clientePF.DocumentoSerie ?? string.Empty;
                txt_endereco.Text = clientePF.Endereco ?? string.Empty;
                txt_telefones.Text = clientePF.Telefones ?? string.Empty;
                txt_naturalidade.Text = clientePF.Naturalidade ?? string.Empty;
                txt_dataNascimento.Text = clientePF.DataNascimento.ToString("yyyy-MM-dd");
                txt_email.Text = clientePF.Email ?? string.Empty;
                txt_historico.Text = clientePF.Historico ?? string.Empty;

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
            Buscar_Click(sender, e);
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
