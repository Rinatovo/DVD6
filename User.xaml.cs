using MySql.Data.MySqlClient;
using System;
using System.Windows;

namespace DVD6
{
    public partial class User : Window
    {
        public User()
        {
            InitializeComponent();
        }

        private void btnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPassword = txtOldPassword.Password;
            string newPassword = txtNewPassword.Password;

            // Vérifier si les champs ne sont pas vides
            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Veuillez saisir l'ancien et le nouveau mot de passe.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Remplacer le mot de passe dans la base de données
            if (ChangePassword(oldPassword, newPassword))
            {
                MessageBox.Show("Mot de passe modifié avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            else
            {
                MessageBox.Show("Ancien mot de passe incorrect ou une erreur s'est produite.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool ChangePassword(string oldPassword, string newPassword)
        {
            // Connexion à la base de données
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Requête SQL pour mettre à jour le mot de passe
                    string query = "UPDATE user SET Password = @NewPassword WHERE Password = @OldPassword";
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@NewPassword", newPassword);
                        command.Parameters.AddWithValue("@OldPassword", oldPassword);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la modification du mot de passe : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }
    }
}
