using System.Windows;

namespace DVD6
{
    public partial class ModifyDVDWindow : Window
    {
        // Propriétés pour stocker les valeurs modifiées
        public string ModifiedTitle { get; set; }
        public string ModifiedDirector { get; set; }
        public string ModifiedDescription { get; set; }
        // Ajoutez d'autres propriétés modifiables si nécessaire

        // Indique si des modifications ont été apportées
        public bool ModificationMade { get; private set; }

        // DVD d'origine avant modification
        private DVD originalDVD;

        public ModifyDVDWindow(DVD dvd)
        {
            InitializeComponent();
            // Stocke le DVD d'origine
            originalDVD = dvd;

            // Initialise les propriétés modifiées avec les valeurs du DVD d'origine
            ModifiedTitle = originalDVD.Title;
            ModifiedDirector = originalDVD.Director;
            ModifiedDescription = originalDVD.Description;
            // Initialisez d'autres propriétés modifiables si nécessaire

            // Associe les données au DataContext pour permettre la liaison des données dans XAML
            DataContext = this;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Met à jour le DVD d'origine avec les nouvelles valeurs modifiées
            originalDVD.Title = ModifiedTitle;
            originalDVD.Director = ModifiedDirector;
            originalDVD.Description = ModifiedDescription;
            // Mettez à jour d'autres propriétés modifiables si nécessaire

            // Appelez la méthode ModifyDVD du DVDController pour enregistrer les modifications dans la base de données
            DVDController dvdController = new DVDController();
            dvdController.ModifyDVD(originalDVD, ModifiedTitle, ModifiedDirector, ModifiedDescription);

            // Indique que des modifications ont été apportées
            ModificationMade = true;

            // Ferme la fenêtre de modification
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Ferme la fenêtre de modification sans sauvegarder les modifications
            Close();
        }
    }
}
