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
using LawForm.Model;

namespace LawForm
{
    /// <summary>
    /// Lógica interna para Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void entrar_Click(object sender, RoutedEventArgs e)
        {
            var Username = login_user.Text;
            var Password = pass_user.Password;

            using (DataContext context = new DataContext())
            {
                bool userFound = context.User.Any(user => user.Nome == Username && user.Senha == Password);

                if (userFound)
                {

                    GrantAccess();
                    Close();

                }
                else
                {
                    MessageBox.Show("Usuário ou senha incorretos.");
                }
            }

        }

        public void GrantAccess()
        {
            MainWindow main = new MainWindow();
            main.Show();
        }

    }
}
