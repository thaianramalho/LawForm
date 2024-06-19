using System;
using System.Linq;
using System.Windows;
using LawForm.Model;
using LawForm.Telas;

namespace LawForm
{
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void entrar_Click(object sender, RoutedEventArgs e)
        {
            var username = login_user.Text;
            var password = pass_user.Password;

            try
            {
                using (DataContext context = new DataContext())
                {
                    var user = context.User.FirstOrDefault(u => u.Nome == username && u.Senha == password);

                    if (user != null)
                    {
                        GrantAccess(user.Nome);
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Usuário ou senha incorretos.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao acessar o banco de dados: " + ex.Message);
            }
        }

        private void GrantAccess(string userName)
        {
            MainWindow main = new MainWindow(userName);
            main.Show();
        }

        private void cadastrar_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Cadastrar telaCadastro = new Cadastrar();
            telaCadastro.Show();
            telaCadastro.Closed += (s, args) => this.Show();
        }
    }
}
