using System;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;

namespace DVD6
{
    internal class DVDController
    {
        public ObservableCollection<DVD> ListDVD { get; set; }

        public DVDController()
        {
            ListDVD = GetDVDs();
        }

        private ObservableCollection<DVD> GetDVDs()
        {
            ObservableCollection<DVD> mesDVD = new ObservableCollection<DVD>();
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
            string query = "SELECT * FROM dvd";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    try
                    {
                        connection.Open();
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int DVDId = reader.GetInt32(0);
                                string Title = reader.GetString(1);
                                string Descritption = reader.GetString(2);
                                string Director = reader.GetString(3);
                                string Genre = reader.GetString(4);
                                int ReleaseYear = reader.GetInt32(5);
                                int IsAvailable = reader.GetInt32(6);
                                string CoverImage = reader.GetString(7);
                                DVD dvd = new DVD { Title = Title, Director = Director, DVDId = DVDId, Genre = Genre, ReleaseYear = ReleaseYear, IsAvailable = IsAvailable, CoverImage = CoverImage, Description = Descritption };
                                mesDVD.Add(dvd);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erreur : " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur : " + ex.Message);
            }
            return mesDVD;
        }

        public void DeleteDVD(DVD dvdToDelete)
        {
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
            string query = "DELETE FROM dvd WHERE DVDId = @DVDId";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@DVDId", dvdToDelete.DVDId);

                connection.Open();
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    // Supprimer le DVD de la liste ListDVD
                    ListDVD.Remove(dvdToDelete);
                }
                else
                {
                    throw new Exception("La suppression du DVD a échoué.");
                }
            }
        }

        public void ModifyDVD(DVD dvdToModify, string newTitle, string newDirector, string newDescritption)
        {
            string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
            string query = "UPDATE dvd SET Title = @NewTitle, Director = @NewDirector, Descritption = @NewDescritption WHERE DVDId = @DVDId";
            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@NewTitle", newTitle);
                    command.Parameters.AddWithValue("@NewDirector", newDirector);
                    command.Parameters.AddWithValue("@NewDescritption", newDescritption);
                    command.Parameters.AddWithValue("@DVDId", dvdToModify.DVDId);
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Détails du DVD modifiés avec succès dans la base de données.");
                        // Mettre à jour les détails du DVD dans la liste ListDVD pour refléter les modifications
                        dvdToModify.Title = newTitle;
                        dvdToModify.Director = newDirector;
                        dvdToModify.Description = newDescritption;
                        // Vous pouvez ajouter d'autres propriétés modifiables de la même manière
                    }
                    else
                    {
                        Console.WriteLine("La modification des détails du DVD a échoué.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la modification des détails du DVD : " + ex.Message);
            }
        }

    }
}
