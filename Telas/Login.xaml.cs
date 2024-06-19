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
                var user = context.User.FirstOrDefault(u => u.Nome == Username && u.Senha == Password);

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

        public void GrantAccess(string userName)
        {
            MainWindow main = new MainWindow(userName);
            main.Show();
        }
    }
}
