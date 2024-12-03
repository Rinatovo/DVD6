using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DVD6
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DVDController dController;
        RetourController rController; // Ajout du contrôleur de retours

        public MainWindow()
        {
            InitializeComponent();
            // Connexion à la liste
            dController = new DVDController();
            rController = new RetourController(); // Initialisation du contrôleur de retours

            DataContext = dController;
        }

        private void Border_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {

        }

        // Event handlers for menu buttons
        private void ClientButton_Click(object sender, RoutedEventArgs e)
        {
            ClientWindow clientWindow = new ClientWindow();
            clientWindow.ShowDialog();
        }

        private void DVDButton_Click(object sender, RoutedEventArgs e)
        {
            // Handle DVD button click event
        }

        private void LocationButton_Click(object sender, RoutedEventArgs e)
        {
            Location locationPage = new Location();
            locationPage.ShowDialog();
        }

        private void RetourButton_Click(object sender, RoutedEventArgs e)
        {
            // Ouvre la fenêtre Retour lorsque le bouton est cliqué
            Retour retourPage = new Retour();
            retourPage.ShowDialog();
        }

        private void UserButton_Click(object sender, RoutedEventArgs e)
        {
            User userPage = new User();
            userPage.Show();
        }

        private void AdminButton_Click(object sender, RoutedEventArgs e)
        {
            Login loginPage = new Login();
            loginPage.ShowDialog();
        }

        // Action button event handlers
        private void AddDVD_Click(object sender, RoutedEventArgs e)
        {
            AjouterDVDWindow ajouterDVDWindow = new AjouterDVDWindow();
            ajouterDVDWindow.Show();
        }

        private void ModifyDVD_Click(object sender, RoutedEventArgs e)
        {
            // Récupère le DVD sélectionné
            Button btn = sender as Button;
            DVD dvdToModify = btn.DataContext as DVD;

            // Ouvre la fenêtre de modification de DVD en passant le DVD à modifier
            ModifyDVDWindow modifyDVDWindow = new ModifyDVDWindow(dvdToModify);
            modifyDVDWindow.ShowDialog();

            // Vérifie si des modifications ont été apportées après la fermeture de la fenêtre de modification
            if (modifyDVDWindow.ModificationMade)
            {
                // Met à jour les détails du DVD dans la liste ListDVD pour refléter les modifications
                // Pas besoin de mettre à jour la liste, les modifications sont directement appliquées au DVD original
            }
        }



        private void DeleteDVD_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer le DVD sélectionné dans la liste
            DVD selectedDVD = (DVD)((Button)sender).DataContext;

            // Vérifier si un DVD est sélectionné
            if (selectedDVD != null)
            {
                // Demander confirmation à l'utilisateur
                MessageBoxResult result = MessageBox.Show($"Êtes-vous sûr de vouloir supprimer le DVD \"{selectedDVD.Title}\" ?", "Confirmation de suppression", MessageBoxButton.YesNo, MessageBoxImage.Question);

                // Vérifier si l'utilisateur a confirmé la suppression
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        // Appeler la méthode pour supprimer le DVD de la base de données
                        dController.DeleteDVD(selectedDVD);

                        // Rafraîchir la liste des DVD après la suppression
                        dController.ListDVD.Remove(selectedDVD);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Erreur lors de la suppression du DVD : " + ex.Message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un DVD à supprimer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
