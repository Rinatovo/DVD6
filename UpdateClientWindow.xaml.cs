using System;
using System.Windows;
using System.Windows.Controls;

namespace DVD6
{
    public partial class UpdateClientWindow : Window
    {
        private Client selectedClient;

        public UpdateClientWindow(Client client)
        {
            InitializeComponent();
            selectedClient = client;

            // Initialiser les contrôles de la fenêtre avec les données du client sélectionné
            FirstNameTextBox.Text = selectedClient.FirstName;
            LastNameTextBox.Text = selectedClient.LastName;
            AdresseTextBox.Text = selectedClient.Adresse;
            PhoneNumberTextBox.Text = selectedClient.PhoneNumber;
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Mettre à jour les propriétés du client avec les nouvelles valeurs saisies dans les champs de texte
            selectedClient.FirstName = FirstNameTextBox.Text;
            selectedClient.LastName = LastNameTextBox.Text;
            selectedClient.Adresse = AdresseTextBox.Text;
            selectedClient.PhoneNumber = PhoneNumberTextBox.Text;

            // Appeler la méthode UpdateClient du contrôleur pour mettre à jour le client dans la base de données
            ClientController clientController = new ClientController();
            clientController.UpdateClient(selectedClient);

            // Fermer la fenêtre après la mise à jour
            Close();
        }
    }
}
