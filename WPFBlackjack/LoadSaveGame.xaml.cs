using CardGameLib;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using WPFBlackjackEL;

namespace WPFBlackjack
{
    /// <summary>
    /// Interaction logic for LoadSaveGame.xaml
    /// </summary>
    public partial class LoadSaveGame : Window
    {
        private MainWindow MainWindow;
        public LoadSaveGame(MainWindow mainWindow)
        {
            InitializeComponent();
            UpdateSaveGameList();
            MainWindow = mainWindow;
        }

        private void UpdateSaveGameList()
        {
            lstSaveGames.Items.Clear();
            List<GameState> saveGames = GameManager.GetSaveGamesFromDatabase();
            foreach (GameState saveGame in saveGames)
            {
                lstSaveGames.Items.Add(saveGame);
            }
        }

        private void lstSaveGames_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSaveGames.SelectedItems.Count > 0)
            {
                btnDeleteGame.IsEnabled = true;
                btnLoadGame.IsEnabled = true;
            }
            else
            {
                btnDeleteGame.IsEnabled = false;
                btnLoadGame.IsEnabled = false;
            }
        }

        private void btnDeleteGame_Click(object sender, RoutedEventArgs e)
        {
            if (lstSaveGames.SelectedItem != null)
            {
                GameManager.RemoveSaveFromDatabase((GameState)lstSaveGames.SelectedItem);
            }
        }

        private void btnLoadGame_Click(object sender, RoutedEventArgs e)
        {
            if (lstSaveGames.SelectedItem != null)
            {
                GameManager loadedManager = GameManager.LoadGame((GameState)lstSaveGames.SelectedItem);

                MainWindow.StartGame(loadedManager);

            }
        }
    }
}
