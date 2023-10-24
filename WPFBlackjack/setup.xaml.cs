using CardGameLib;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;
using WPFBlackjackEL;

namespace WPFBlackjack
{
    /// <summary>
    /// Interaction logic for setup.xaml
    /// </summary>
    public partial class setup : Window
    {
        MainWindow parentWindow;
        public setup(MainWindow parentWindow)
        {
            InitializeComponent();
            this.parentWindow = parentWindow;
            for (int i = 1; i <= 5; i++)
            {
                cBoxPlayers.Items.Add(i);
            }
            for (int i = 1; i <= 8; i++)
            {
                cBoxDecks.Items.Add(i);
            }
            List<Player> savedPlayers = GameManager.GetPlayersFromDatabase();
            foreach (Player player in savedPlayers)
            {
                lstSavedPlayers.Items.Add(player.PlayerName + ": " + player.Funds + " gold");
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.Decks = (int)cBoxDecks.SelectedItem;
            parentWindow.Players = (int)cBoxPlayers.SelectedItem;
            parentWindow.StartGame();
            if (lstSavedPlayers.SelectedItems.Count > 0)
            {
                parentWindow.LoadPlayer1((Player)lstSavedPlayers.SelectedItem);
            }
            else
            {
                InsertName insert = new InsertName();
                insert.Show();
            }
            this.Hide();
        }
    }
}
