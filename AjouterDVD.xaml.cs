using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace DVD6
{
    public partial class AjouterDVDWindow : Window
    {
        public AjouterDVDWindow()
        {
            InitializeComponent();
        }

        private void Ajouter_Click(object sender, RoutedEventArgs e)
        {
            string title = txtTitle.Text;
            string director = txtDirector.Text;
            string description = txtDescription.Text;
            string genre = txtGenre.Text;
            int releaseYear;
            if (!int.TryParse(txtReleaseYear.Text, out releaseYear))
            {
                MessageBox.Show("Veuillez saisir une année de sortie valide.");
                return;
            }

            int isAvailable = 1; // Stock initial de 1

            string selectedImagePath = lblImagePath.Content.ToString();
            string coverImageFileName = "";

            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                coverImageFileName = "C:/Users/rinas/source/repos/DVD6/CoverImage/" + Path.GetFileName(selectedImagePath);
                string destinationFolder = @"C:\Users\rinas\source\repos\DVD6\CoverImage\";

                try
                {
                    string destinationFilePath = Path.Combine(destinationFolder, Path.GetFileName(selectedImagePath));
                    File.Copy(selectedImagePath, destinationFilePath, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erreur lors de la copie du fichier : " + ex.Message);
                    return;
                }
            }

            string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
            string query = "INSERT INTO dvd (Title, Director, Description, Genre, ReleaseYear, IsAvailable, CoverImage) VALUES (@Title, @Director, @Description, @Genre, @ReleaseYear, @IsAvailable, @CoverImage)";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Title", title);
                    command.Parameters.AddWithValue("@Director", director);
                    command.Parameters.AddWithValue("@Description", description);
                    command.Parameters.AddWithValue("@Genre", genre);
                    command.Parameters.AddWithValue("@ReleaseYear", releaseYear);
                    command.Parameters.AddWithValue("@IsAvailable", isAvailable);
                    command.Parameters.AddWithValue("@CoverImage", coverImageFileName);

                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Le DVD a été ajouté avec succès avec un stock initial de 1.");
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("L'ajout du DVD a échoué.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }

        private void btnBrowse_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            if (openFileDialog.ShowDialog() == true)
            {
                string selectedImagePath = openFileDialog.FileName;
                lblImagePath.Content = selectedImagePath;
            }
        }
    }
}