using LawForm.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
                loginScreen(sender, e);

            }
        }

        private void loginScreen(object sender, RoutedEventArgs e)
        {

            Login telaLogin = new Login();
            this.Close();
            telaLogin.Show();
          
        }
    }
}
