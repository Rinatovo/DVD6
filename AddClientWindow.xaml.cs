using System;
using System.Windows;
using System.Windows.Controls;

namespace DVD6
{
    public partial class AddClientWindow : Window
    {
        public AddClientWindow()
        {
            InitializeComponent();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Créer un nouvel objet Client avec les valeurs saisies dans les champs de texte
            Client newClient = new Client
            {
                FirstName = FirstNameTextBox.Text,
                LastName = LastNameTextBox.Text,
                Adresse = AdresseTextBox.Text,
                PhoneNumber = PhoneNumberTextBox.Text
            };

            // Appeler la méthode AddClient du contrôleur pour ajouter le nouveau client à la base de données
            ClientController clientController = new ClientController();
            clientController.AddClient(newClient);

            // Fermer la fenêtre après l'ajout du client
            Close();
        }
    }
}
