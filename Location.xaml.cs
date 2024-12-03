using System;
using System.Windows;

namespace DVD6
{
    public partial class Location : Window
    {
        private LocationController locationController;

        public Location()
        {
            InitializeComponent();
            locationController = new LocationController();
            DataContext = locationController;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            try
            {
                StatusText.Text = "Chargement des données...";
                await locationController.LoadDataFromDatabaseAsync();
                StatusText.Text = "Données chargées avec succès.";
            }
            catch (Exception ex)
            {
                StatusText.Text = "Erreur lors du chargement des données.";
                MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddLocationButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedItem is Client selectedClient && DVDComboBox.SelectedItem is DVD selectedDVD)
            {
                try
                {
                    StatusText.Text = "Ajout de la location...";
                    await locationController.AddNewLocation(selectedClient.ClientId, selectedDVD.DVDId);
                    StatusText.Text = "Location ajoutée avec succès.";
                }
                catch (Exception ex)
                {
                    StatusText.Text = "Erreur lors de l'ajout de la location.";
                    MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client et un DVD.", "Avertissement", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}