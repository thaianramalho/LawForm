using LawForm.Model;
using System.Windows;

namespace LawForm.Telas
{
    /// <summary>
    /// Lógica interna para Cadastrar.xaml
    /// </summary>
    public partial class Cadastrar : Window
    {
        public Cadastrar()
        {
            InitializeComponent();
        }

        private void cadastrar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_usuario.Text) ||
                string.IsNullOrWhiteSpace(txt_senha.Password))
            {
                MessageBox.Show("Por favor, preencha todos os campos.");
                return;
            }

            using (var context = new DataContext())
            {
                var user = new User
                {
                    Nome = txt_usuario.Text,
                    Senha = txt_senha.Password,
                };

                context.User.Add(user);
                context.SaveChanges();
                MessageBox.Show("Usuário cadastrado com sucesso.");
                loginScreen_Click(sender, e);

            }
        }

        private void loginScreen_Click(object sender, RoutedEventArgs e)
        {

            Login telaLogin = new Login();
            this.Close();
            telaLogin.Show();

        }
    }
}
