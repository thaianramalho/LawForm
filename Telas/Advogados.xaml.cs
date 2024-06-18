using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LawForm.Model;

namespace LawForm
{
    public partial class Advogados : Window
    {
        private int? advogadoIdSelecionado = null;

        public Advogados()
        {
            InitializeComponent();
            CarregarDados();
        }

        private void CarregarDados()
        {
            using (var context = new DataContext())
            {
                var advogados = context.Advogado.ToList();
                if (advogados.Any(a => a == null || string.IsNullOrWhiteSpace(a.Nome) || string.IsNullOrWhiteSpace(a.Cpf) || string.IsNullOrWhiteSpace(a.Email) || string.IsNullOrWhiteSpace(a.Cna)))
                {
                    MessageBox.Show("Existem dados inválidos na lista de advogados.");
                }
                else
                {
                    dataGridAdvogados.ItemsSource = advogados;
                }
            }
        }

        private void Cadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_nome.Text) ||
                string.IsNullOrWhiteSpace(txt_cpf.Text) ||
                string.IsNullOrWhiteSpace(txt_email.Text) ||
                string.IsNullOrWhiteSpace(txt_cna.Text))
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            using (var context = new DataContext())
            {
                var advogado = new Advogado
                {
                    Nome = txt_nome.Text,
                    Cpf = txt_cpf.Text,
                    Email = txt_email.Text,
                    Cna = txt_cna.Text
                };

                context.Advogado.Add(advogado);
                context.SaveChanges();
            }

            LimparCampos();
            CarregarDados();
        }

        private void Atualizar_Click(object sender, RoutedEventArgs e)
        {
            if (advogadoIdSelecionado == null)
            {
                MessageBox.Show("Nenhum advogado selecionado para atualização.");
                return;
            }

            using (var context = new DataContext())
            {
                var advogado = context.Advogado.Find(advogadoIdSelecionado);
                if (advogado != null)
                {
                    advogado.Nome = txt_nome.Text;
                    advogado.Cpf = txt_cpf.Text;
                    advogado.Email = txt_email.Text;
                    advogado.Cna = txt_cna.Text;

                    context.SaveChanges();
                }
            }

            LimparCampos();
            CarregarDados();
            cadastrar.Visibility = Visibility.Visible;
            atualizar.Visibility = Visibility.Collapsed;
            advogadoIdSelecionado = null;
        }

        private void Excluir_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                int id = (int)button.Tag;

                using (var context = new DataContext())
                {
                    var advogado = context.Advogado.Find(id);
                    if (advogado != null)
                    {
                        MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Tem certeza?", "Confirmar exclusão", System.Windows.MessageBoxButton.YesNo);
                        if (messageBoxResult == MessageBoxResult.Yes)
                        {
                            context.Advogado.Remove(advogado);
                            context.SaveChanges();
                            CarregarDados();
                        }
                    }
                }
            }
        }

        private void DataGridAdvogados_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataGridAdvogados.SelectedItem != null)
            {
                var advogado = dataGridAdvogados.SelectedItem as Advogado;
                if (advogado != null)
                {
                    advogadoIdSelecionado = advogado.Id;
                    txt_nome.Text = advogado.Nome;
                    txt_cpf.Text = advogado.Cpf;
                    txt_email.Text = advogado.Email;
                    txt_cna.Text = advogado.Cna;

                    cadastrar.Visibility = Visibility.Collapsed;
                    atualizar.Visibility = Visibility.Visible;
                }
            }
            else
            {
                LimparCampos();
                advogadoIdSelecionado = null;
                cadastrar.Visibility = Visibility.Visible;
                atualizar.Visibility = Visibility.Collapsed;
            }
        }

        private void LimparCampos()
        {
            txt_nome.Clear();
            txt_cpf.Clear();
            txt_email.Clear();
            txt_cna.Clear();
            dataGridAdvogados.SelectedItem = null;
        }
    }
}
