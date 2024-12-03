using System.Windows;

namespace DVD6
{
    public partial class Retour : Window
    {
        private readonly RetourController retourController;

        public Retour()
        {
            InitializeComponent();
            retourController = new RetourController();
            DataContext = retourController;
            LoadDataAsync();
        }

        private async void LoadDataAsync()
        {
            await retourController.LoadDataFromDatabaseAsync();
        }

        private async void CompleteRetourButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is FrameworkElement element && element.DataContext is Retour retour)
            {
                await retourController.CompleteRetourAsync(retour);
            }
            else
            {
                MessageBox.Show("Erreur : Impossible de compléter le retour.");
            }
        }
    }
}