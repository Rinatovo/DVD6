using System;
using System.Windows;
using System.Windows.Controls;

namespace DVD6
{
    public partial class ClientWindow : Window
    {
        private ClientController clientController;

        public ClientWindow()
        {
            InitializeComponent();
            clientController = new ClientController();
            DataContext = clientController;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvrir une nouvelle fenêtre de dialogue pour ajouter un nouveau client
            AddClientWindow addClientWindow = new AddClientWindow();
            addClientWindow.ShowDialog();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir le client sélectionné dans la ListView
            Client selectedClient = (Client)ListView.SelectedItem;

            if (selectedClient != null)
            {
                // Ouvrir une nouvelle fenêtre de dialogue pour mettre à jour le client sélectionné
                UpdateClientWindow updateClientWindow = new UpdateClientWindow(selectedClient);
                updateClientWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à mettre à jour.");
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            // Obtenir le client sélectionné dans la ListView
            Client selectedClient = (Client)ListView.SelectedItem;

            if (selectedClient != null)
            {
                // Appeler la méthode DeleteClient de ClientController pour supprimer le client sélectionné
                clientController.DeleteClient(selectedClient);
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.");
            }
        }
    }
}
