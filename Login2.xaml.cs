using System;
using System.Linq;
using System.Windows;
using DVD6;

using MySql.Data.MySqlClient;

namespace DVD6
{
    public partial class Login2 : Window
    {
        public Login2()
        {
            InitializeComponent();
        }

        public class User
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public bool IsAdmin { get; set; }
        }


        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Veuillez saisir un nom d'utilisateur et un mot de passe.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (AuthenticateUser(username, password))
            {
                User userWindow = new User();
              //  userWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Nom d'utilisateur ou mot de passe incorrect.", "Erreur de connexion", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM user WHERE Username = @Username AND Password = @Password";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        return reader.HasRows;
                    }
                }
            }
        }
    }
}
