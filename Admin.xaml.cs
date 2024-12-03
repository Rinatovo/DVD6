using MySql.Data.MySqlClient;
using System;
using System.Linq;
using System.Windows;

namespace DVD6
{
    public partial class Admin : Window
    {
        public Admin()
        {
            InitializeComponent();
        }

        private void btnCreateUser_Click(object sender, RoutedEventArgs e)
        {
            string username = txtNewUsername.Text;
            string password = GenerateRandomPassword(); // Utilisation de la méthode GenerateRandomPassword
            bool isAdmin = chkIsAdmin.IsChecked ?? false;

            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Veuillez saisir un nom d'utilisateur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CreateNewUser(username, password, isAdmin))
            {
                MessageBox.Show("Utilisateur créé avec succès.\nMot de passe : " + password, "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Une erreur s'est produite lors de la création de l'utilisateur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CreateNewUser(string username, string password, bool isAdmin)
        {
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO user (Username, Password, IsAdmin) VALUES (@Username, @Password, @IsAdmin)";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);
                        command.Parameters.AddWithValue("@IsAdmin", isAdmin);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la création de l'utilisateur : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        // Méthode pour générer un mot de passe aléatoire
        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
