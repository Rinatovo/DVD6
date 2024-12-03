using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DVD6
{
    public class RetourController
    {
        public ObservableCollection<Retour> ListRetours { get; set; }

        public RetourController()
        {
            ListRetours = new ObservableCollection<Retour>();
        }

        public async Task LoadDataFromDatabaseAsync()
        {
            ListRetours.Clear();

            try
            {
                string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    string query = @"SELECT l.LocationId, l.LeClient, l.LeDVD, l.DateRented, l.DateReturned, 
                                            c.FirstName, c.LastName, d.Title 
                                     FROM location l 
                                     JOIN client c ON l.LeClient = c.ClientId 
                                     JOIN dvd d ON l.LeDVD = d.DVDId 
                                     WHERE l.DateReturned IS NULL";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    using (MySqlDataReader reader = (MySqlDataReader)await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Retour retour = new Retour
                            {
                                LocationId = reader.GetInt32(0),
                                ClientId = reader.GetInt32(1),
                                DVDId = reader.GetInt32(2),
                                DateRented = reader.GetDateTime(3),
                                ClientName = $"{reader.GetString(5)} {reader.GetString(6)}",
                                DVDTitle = reader.GetString(7)
                            };
                            ListRetours.Add(retour);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors du chargement des retours : {ex.Message}");
            }
        }

        public async Task CompleteRetourAsync(Retour retour)
        {
            if (retour == null)
            {
                throw new ArgumentNullException(nameof(retour));
            }

            try
            {
                string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string updateQuery = @"UPDATE location 
                                           SET DateReturned = @dateReturned 
                                           WHERE LocationId = @locationId";

                    using (MySqlCommand command = new MySqlCommand(updateQuery, connection))
                    {
                        command.Parameters.AddWithValue("@dateReturned", DateTime.Now);
                        command.Parameters.AddWithValue("@locationId", retour.LocationId);
                        await command.ExecuteNonQueryAsync();
                    }

                    string updateDVDQuery = @"UPDATE dvd 
                                              SET IsAvailable = IsAvailable + 1 
                                              WHERE DVDId = @dvdId";

                    using (MySqlCommand command = new MySqlCommand(updateDVDQuery, connection))
                    {
                        command.Parameters.AddWithValue("@dvdId", retour.DVDId);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                ListRetours.Remove(retour);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise à jour du retour : {ex.Message}");
            }
        }
    }
}