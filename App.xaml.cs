using System.Windows;

namespace DVD6
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Créer et afficher la fenêtre de connexion
            Login loginWindow = new Login();
            loginWindow.ShowDialog();

            // Si la connexion est réussie, la fenêtre principale sera ouverte par la fenêtre de connexion
            // Si la connexion échoue ou si l'utilisateur ferme la fenêtre de connexion, l'application se fermera
            if (!loginWindow.IsLoggedIn)
            {
                Shutdown();
            }
        }
    }
}