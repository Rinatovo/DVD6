using DVD6;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class ClientController
{
    public ObservableCollection<Client> ListClient { get; set; }

    public ClientController()
    {
        ListClient = GetClients();
    }

    private ObservableCollection<Client> GetClients()
    {
        ObservableCollection<Client> clients = new ObservableCollection<Client>();
        string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
        string query = "SELECT * FROM client"; // Assurez-vous que le nom de la table est correct

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                connection.Open();
                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int clientId = reader.GetInt32(0);
                        string firstName = reader.GetString(1);
                        string lastName = reader.GetString(2);
                        string adresse = reader.GetString(3);
                        string phoneNumber = reader.GetString(4);

                        Client client = new Client
                        {
                            ClientId = clientId,
                            FirstName = firstName,
                            LastName = lastName,
                            Adresse = adresse,
                            PhoneNumber = phoneNumber
                            // Ajoutez d'autres propriétés si nécessaire
                        };

                        clients.Add(client);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur : " + ex.Message);
        }

        return clients;
    }



public void AddClient(Client client)
    {
        string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
        string query = "INSERT INTO client (FirstName, LastName, Adresse, PhoneNumber) VALUES (@FirstName, @LastName, @Adresse, @PhoneNumber)";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", client.FirstName);
                command.Parameters.AddWithValue("@LastName", client.LastName);
                command.Parameters.AddWithValue("@Adresse", client.Adresse);
                command.Parameters.AddWithValue("@PhoneNumber", client.PhoneNumber);

                connection.Open();
                command.ExecuteNonQuery();

                // Update the client list after adding a new client
                ListClient.Add(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur : " + ex.Message);
        }
    }

    public void UpdateClient(Client client)
    {
        string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
        string query = "UPDATE client SET FirstName = @FirstName, LastName = @LastName, Adresse = @Adresse, PhoneNumber = @PhoneNumber WHERE ClientId = @ClientId";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@FirstName", client.FirstName);
                command.Parameters.AddWithValue("@LastName", client.LastName);
                command.Parameters.AddWithValue("@Adresse", client.Adresse);
                command.Parameters.AddWithValue("@PhoneNumber", client.PhoneNumber);
                command.Parameters.AddWithValue("@ClientId", client.ClientId);

                connection.Open();
                command.ExecuteNonQuery();

                // Update the client list after updating a client
                int index = ListClient.IndexOf(ListClient.FirstOrDefault(c => c.ClientId == client.ClientId));
                if (index != -1)
                {
                    ListClient[index] = client;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur : " + ex.Message);
        }
    }

    public void DeleteClient(Client client)
    {
        string connectionString = "server=localhost;database=dvd;uid=root;pwd=root;";
        string query = "DELETE FROM client WHERE ClientId = @ClientId";

        try
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            using (MySqlCommand command = new MySqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@ClientId", client.ClientId);

                connection.Open();
                command.ExecuteNonQuery();

                // Update the client list after deleting a client
                ListClient.Remove(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Erreur : " + ex.Message);
        }
    }
}
