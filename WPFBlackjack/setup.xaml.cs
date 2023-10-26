using CardGameLib;
using System;
using System.Collections.Generic;
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
        //populate combo boxes
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
        //load list of saved player profiles
        private void ShowSavedPlayers()
        {
            lstSavedPlayers.Items.Clear();
            List<PlayerProfile> savedPlayers = GameManager.GetPlayersFromDatabase();
            foreach (PlayerProfile player in savedPlayers)
            {
                lstSavedPlayers.Items.Add(player);
            }
        }
        //retrive the parent's window
        public MainWindow ParentWindow { get => parentWindow; }
        //Setup game with either selected player or ask for new name
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            ParentWindow.Decks = (int)cBoxDecks.SelectedItem;
            ParentWindow.Players = (int)cBoxPlayers.SelectedItem;
            ParentWindow.StartGame();
            if (lstSavedPlayers.SelectedItems.Count > 0)
            {
                ParentWindow.LoadPlayer1((PlayerProfile)lstSavedPlayers.SelectedItem);
            }
            else
            {
                InsertName insert = new InsertName(this);
                insert.Show();
            }
            this.Hide();
        }
        //ask to remove player from database if delete key pressed
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
                        GameManager.RemovePlayerFromDatabase((Player)lstSavedPlayers.SelectedItem);
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
