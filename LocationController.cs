using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace DVD6
{
    public class LocationController
    {
        public ObservableCollection<Client> Clients { get; private set; }
        public ObservableCollection<DVD> DVDs { get; private set; }
        public ObservableCollection<LocationItem> Locations { get; private set; }

        private readonly string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";

        public LocationController()
        {
            Clients = new ObservableCollection<Client>();
            DVDs = new ObservableCollection<DVD>();
            Locations = new ObservableCollection<LocationItem>();
        }

        public async Task LoadDataFromDatabaseAsync()
        {
            await LoadClientsAsync();
            await LoadDVDsAsync();
            await LoadLocationsAsync();
        }

        private async Task LoadClientsAsync()
        {
            Clients.Clear();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT ClientId, FirstName, LastName FROM client";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Clients.Add(new Client
                        {
                            ClientId = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2)
                        });
                    }
                }
            }
        }

        private async Task LoadDVDsAsync()
        {
            DVDs.Clear();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT DVDId, Title, IsAvailable FROM dvd WHERE IsAvailable > 0";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        DVDs.Add(new DVD
                        {
                            DVDId = reader.GetInt32(0),
                            Title = reader.GetString(1),
                            IsAvailable = reader.GetInt32(2)
                        });
                    }
                }
            }
        }

        private async Task LoadLocationsAsync()
        {
            Locations.Clear();
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                string query = @"SELECT c.FirstName, c.LastName, d.Title, l.DateRented 
                                 FROM location l
                                 JOIN client c ON l.LeClient = c.ClientId
                                 JOIN dvd d ON l.LeDVD = d.DVDId
                                 WHERE l.DateReturned IS NULL
                                 ORDER BY l.DateRented DESC";
                using (var command = new MySqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Locations.Add(new LocationItem
                        {
                            ClientName = $"{reader.GetString(0)} {reader.GetString(1)}",
                            DVDTitle = reader.GetString(2),
                            DateRented = reader.GetDateTime(3)
                        });
                    }
                }
            }
        }

        public async Task AddNewLocation(int clientId, int dvdId)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var transaction = await connection.BeginTransactionAsync())
                {
                    try
                    {
                        // Vérifier la disponibilité du DVD
                        string checkQuery = "SELECT IsAvailable FROM dvd WHERE DVDId = @dvdId FOR UPDATE";
                        int isAvailable;
                        using (var command = new MySqlCommand(checkQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@dvdId", dvdId);
                            isAvailable = Convert.ToInt32(await command.ExecuteScalarAsync());
                        }

                        if (isAvailable <= 0)
                        {
                            throw new Exception("Ce DVD n'est plus disponible.");
                        }

                        // Ajouter la location
                        string insertQuery = "INSERT INTO location (LeClient, LeDVD, DateRented) VALUES (@clientId, @dvdId, @dateRented)";
                        using (var command = new MySqlCommand(insertQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@clientId", clientId);
                            command.Parameters.AddWithValue("@dvdId", dvdId);
                            command.Parameters.AddWithValue("@dateRented", DateTime.Now);
                            await command.ExecuteNonQueryAsync();
                        }

                        // Mettre à jour la disponibilité
                        string updateQuery = "UPDATE dvd SET IsAvailable = IsAvailable - 1 WHERE DVDId = @dvdId";
                        using (var command = new MySqlCommand(updateQuery, connection, transaction))
                        {
                            command.Parameters.AddWithValue("@dvdId", dvdId);
                            await command.ExecuteNonQueryAsync();
                        }

                        await transaction.CommitAsync();
                    }
                    catch
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }

            // Recharger les données après l'ajout
            await LoadDataFromDatabaseAsync();
        }
    }

    public class LocationItem
    {
        public string ClientName { get; set; }
        public string DVDTitle { get; set; }
        public DateTime DateRented { get; set; }
    }
}