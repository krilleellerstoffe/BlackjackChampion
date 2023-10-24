using CardGameLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPFBlackjackEL;

namespace WPFBlackjack
{
    /// <summary>
    /// Interaction logic for setup.xaml
    /// </summary>
    public partial class setup : Window
    {
        private MainWindow parentWindow;
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
            ShowSavedPlayers();
        }

        private void ShowSavedPlayers()
        {
            lstSavedPlayers.Items.Clear();
            List<Player> savedPlayers = GameManager.GetPlayersFromDatabase();
            foreach (Player player in savedPlayers)
            {
                lstSavedPlayers.Items.Add(player);
            }
        }

        public MainWindow ParentWindow { get => parentWindow; }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Decks = (int)cBoxDecks.SelectedItem;
            ParentWindow.Players = (int)cBoxPlayers.SelectedItem;
            ParentWindow.StartGame();
            if (lstSavedPlayers.SelectedItems.Count > 0)
            {
                ParentWindow.LoadPlayer1((Player)lstSavedPlayers.SelectedItem);
            }
            else
            {
                InsertName insert = new InsertName(this);
                insert.Show();
            }
            this.Hide();
        }

        private void lstSavedPlayers_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                Logger.LogMessage("delete key");
                if (lstSavedPlayers.SelectedItems.Count > 0)
            {
                    MessageBoxResult result = MessageBox.Show("Delete '" + ((Player)lstSavedPlayers.SelectedItem).PlayerName + "' from database?", "Delete player", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No) return;
                try
                {
                    GameManager.RemoveFromDatabase((Player)lstSavedPlayers.SelectedItem);
                    ShowSavedPlayers();
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex.Message);
                }
            }
            }
        }
    }
}
