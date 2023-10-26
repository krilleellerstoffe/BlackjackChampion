using CardGameLib;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.Linq;
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
            for (int i = 1; i <= 5; i++)
            {
                cBoxPlayers.Items.Add(i);
            }
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
                UpdateSaveGameList();
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

        private void txtBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBoxName.Text))
            {
                UpdateSaveGameList();
                return;
            }
            lstSaveGames.Items.Clear();
            List<GameState> saveGames = GameManager.GetSaveGamesFromDatabase();
            saveGames = saveGames.Where(game => game.Players.Any(player => player.PlayerName.Contains(txtBoxName.Text))).ToList();
            foreach (GameState saveGame in saveGames)
            {
                lstSaveGames.Items.Add(saveGame);
            }
        }

        private void cBoxPlayers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (cBoxPlayers.SelectedItem == null)
            {
                UpdateSaveGameList();
                return;
            }
            lstSaveGames.Items.Clear();
            List<GameState> saveGames = GameManager.GetSaveGamesFromDatabase();
            saveGames = saveGames.Where(game => game.Players.Count == (int)cBoxPlayers.SelectedItem + 1).ToList();
            foreach (GameState saveGame in saveGames)
            {
                lstSaveGames.Items.Add(saveGame);
            }
        }
    }
}
