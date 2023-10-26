﻿using CardGameLib;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using WPFBlackjackEL;

namespace WPFBlackjack
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameManager _gameManager;
        private setup setupWindow;
        private int _decks;
        private int _players;
        private int _shuffleThreshold = 4; //at what fraction to ask to shuffle

        public int Decks { get => _decks; set => _decks = value; }
        public int Players { get => _players; set => _players = value; }
        protected override void OnClosed(EventArgs e)
        {
            if (_gameManager != null)
            {
                MessageBoxResult result = MessageBox.Show("Save '" + _gameManager.Players[1].PlayerName + "' to database?", "Exit game", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        _gameManager.SavePlayerToDatabase();
                        break;
                    default:
                        break;
                }
            }

            base.OnClosed(e);

            Application.Current.Shutdown();
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public void StartGame()
        {
            //create new game based on setup
            StartGame(new GameManager(_decks, _players));
        }

        public void StartGame(GameManager gamemanager)
        {
            _gameManager = gamemanager;
            //subscribe to events in gamemanager
            _gameManager.CardDrawn += UpdateInfoLabels;
            _gameManager.CardDrawn += ShowHideSingleDealerCard;
            _gameManager.CardDrawn += UpdatePlayerCards;
            _gameManager.Bust += ShowBustMessage;
            _gameManager.Bust += UpdateInfoLabels;
            _gameManager.Standing += ShowStandMessage;
            _gameManager.Standing += UpdateInfoLabels;
            _gameManager.Results += ShowHideAllDealerCards;
            _gameManager.Results += UpdateInfoLabels;
            _gameManager.Results += ShowWinners;
            //now let user deal
            _gameManager.State = StateofPlay.NewGame;
            UpdateButtons();

            UpdateInfoLabels(null);
            ShowHideAllDealerCards(null);
            UpdatePlayerCards(null);
        }
        //winners popup and reset buttons
        private void ShowWinners(List<Player> winners)
        {
            string winnerString = "Winners:\n";
            foreach (Player player in winners)
            {
                winnerString += player.PlayerName + "\n";
            }
            MessageBox.Show(winnerString);
            _gameManager.State = StateofPlay.WinnerDeclared;
            UpdateButtons();

            _gameManager.SplitPotToWinners();
            UpdateInfoLabels(null);

        }
        public void ShowStandMessage(Player player)
        {
            //reveal cards if dealer
            if (player.PlayerName.Equals("Dealer"))
            {
                ShowHideAllDealerCards(player);
            }
            MessageBox.Show(player.PlayerName + " stands with " + player.Hand.HandValue());
        }
        public void ShowBustMessage(Player player)
        {
            //only show popup if user goes bust
            if (player.PlayerNumber == 1) MessageBox.Show(player.PlayerName + " BUST!");
        }
        //refresh labels - overloaded
        private void UpdateInfoLabels(object? obj) => UpdateInfoLabels(obj, null);
        private void UpdateInfoLabels(object? obj, object? obj1)
        {
            lblCardsInShoe.Content = _gameManager.Shoe.Cards.Count;
            lblPlayerCount.Content = _gameManager.Players.Length - 1;
            lblCardsSinceShuffle.Content = _gameManager.Shoe.CardsSinceLastShuffle;
            lblShuffle.Content = _gameManager.Shoe.TimeToShuffle(_shuffleThreshold);
            lblPot.Content = _gameManager.Pot;
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    if (i != 0) ((Label)tableCanvas.FindName("lblFundsPlayer" + i)).Content = _gameManager.Players[i].Funds;
                    ((Label)tableCanvas.FindName("lblStatePlayer" + i)).Content = _gameManager.Players[i].PlayerState;
                }
                catch
                {
                    if (i != 0) ((Label)tableCanvas.FindName("lblFundsPlayer" + i)).Content = "";
                    ((Label)tableCanvas.FindName("lblStatePlayer" + i)).Content = "Not in Play";
                }
            }
        }
        //dealer gets special methods as we don't always want to reveal more than the first card
        //reveal or hide first dealer card
        private void ShowHideSingleDealerCard(object? obj, object? obj1)
        {
            lstDealerCards.Items.Clear();
            foreach (Card card in _gameManager.Players[0].Hand.Cards)
            {
                lstDealerCards.Items.Add(card);
            }

            if (lstDealerCards.Items.Count > 0)
            {
                ShowImageBasedOnCard(imgDealerCard1, (Card)lstDealerCards.Items[0]);
            }
            else
            {
                imgDealerCard1.Visibility = Visibility.Hidden;
            }
        }
        //reveal or hide all dealer cards
        private void ShowHideAllDealerCards(object? obj)
        {
            for (int i = 1; i <= 7; i++)
            {
                ((Image)tableCanvas.FindName("imgDealerCard" + i)).Visibility = Visibility.Hidden;
            }
            lstDealerCards.Items.Clear();
            try
            {
                foreach (Card card in _gameManager.Players[0].Hand.Cards)
                {
                    lstDealerCards.Items.Add(card);
                }
                for (int i = 0; i < lstDealerCards.Items.Count; i++)
                {
                    ShowImageBasedOnCard((Image)tableCanvas.FindName("imgDealerCard" + (i + 1)), (Card)lstDealerCards.Items[i]);
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
        }
        //create image based on card suit/value
        private void ShowImageBasedOnCard(Image image, Card card)
        {
            image.Visibility = Visibility.Visible;
            string cardString = card.ToString();
            cardString = cardString.Replace(' ', '_');
            image.Source = new BitmapImage(new Uri("Resources/" + cardString + ".png", UriKind.Relative));

        }
        //refresh images for each player's cards - overloaded
        private void UpdatePlayerCards(object? obj) => UpdatePlayerCards(obj, null);
        private void UpdatePlayerCards(object? obj, object? obj1)
        {
            for (int j = 1; j < 5; j++)
            {
                ListBox playerCards = (ListBox)playerCanvas.FindName("lstPlayer" + j + "Cards");
                for (int i = 1; i <= 7; i++)
                {
                    ((Image)tableCanvas.FindName("imgPlayer" + j + "Card" + i)).Visibility = Visibility.Hidden;
                }
                try
                {
                    playerCards.Items.Clear();
                    foreach (Card card in _gameManager.Players[j].Hand.Cards)
                    {
                        playerCards.Items.Add(card);
                    }
                    for (int i = 0; i < playerCards.Items.Count; i++)
                    {
                        ShowImageBasedOnCard((Image)tableCanvas.FindName("imgPlayer" + j + "Card" + (i + 1)), (Card)playerCards.Items[i]);
                    }
                }
                catch
                {
                    //do nothing, just catch exception
                }


            }
            if (lstPlayer1Cards.Items.Count >= 7) btnHit.IsEnabled = false;
            lblHandValue.Content = _gameManager.Players[1].Hand.HandValue();
        }
        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            _gameManager.Deal();
            _gameManager.State = StateofPlay.AfterDeal;
            UpdateButtons();

        }

        private void btnShuffle_Click(object sender, RoutedEventArgs e) => _gameManager.Shoe.Shuffle();

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {

            _gameManager.Hit(_gameManager.Players[1]);
            _gameManager.State = StateofPlay.AfterHit;
        }

        private void btnStand_Click(object sender, RoutedEventArgs e)
        {

            _gameManager.Stand(1);
            _gameManager.State = StateofPlay.PlayerStanding;
            UpdateButtons();
            UpdateInfoLabels(sender);
        }

        private void btnSurrender_Click(object sender, RoutedEventArgs e)
        {
            _gameManager.Surrender();
            _gameManager.State = StateofPlay.PlayerStanding;
            UpdateButtons();
        }

        private void btnNewhand_Click(object sender, RoutedEventArgs e)
        {
            _gameManager.NewHand();

            if (_gameManager.Shoe.TimeToShuffle(_shuffleThreshold))
            {
                MessageBoxResult result = MessageBox.Show((_shuffleThreshold - 1) + "/" + _shuffleThreshold + " of the cards in the shoe" +
                    " have been used since you last shuffled, shuffle now?", "Shuffle cards in shoe?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    _gameManager.Shoe.Shuffle();
                }
            }
            _gameManager.State = StateofPlay.NewHand;
            UpdateButtons();
            UpdateInfoLabels(null);
            UpdatePlayerCards(null);
            ShowHideAllDealerCards(null);

        }

        private void UpdateButtons()
        {
            switch (_gameManager.State)
            {
                case StateofPlay.NewGame:

                    btnSave.IsEnabled = true;
                    btnShuffle.IsEnabled = false;
                    btnNewhand.IsEnabled = true;
                    btnDeal.IsEnabled = false;
                    btnHit.IsEnabled = false;
                    btnStand.IsEnabled = false;
                    btnSurrender.IsEnabled = false;
                    break;
                case StateofPlay.NewHand:
                    btnSave.IsEnabled = true;
                    btnShuffle.IsEnabled = true;
                    btnNewhand.IsEnabled = false;
                    btnDeal.IsEnabled = true;
                    btnHit.IsEnabled = false;
                    btnStand.IsEnabled = false;
                    btnSurrender.IsEnabled = false;
                    break;
                case StateofPlay.AfterDeal:
                    btnSave.IsEnabled= true;
                    btnShuffle.IsEnabled = false;
                    btnNewhand.IsEnabled = false;
                    btnDeal.IsEnabled = false;
                    btnHit.IsEnabled = true;
                    btnStand.IsEnabled = true;
                    btnSurrender.IsEnabled = true;
                    break;
                case StateofPlay.AfterHit:
                    btnSave.IsEnabled= true;
                    btnShuffle.IsEnabled = false;
                    btnNewhand.IsEnabled = false;
                    btnDeal.IsEnabled = false;
                    btnHit.IsEnabled = true;
                    btnStand.IsEnabled = true;
                    btnSurrender.IsEnabled = false;
                    break;
                case StateofPlay.PlayerStanding:
                case StateofPlay.WinnerDeclared:
                    btnSave.IsEnabled= true;
                    btnShuffle.IsEnabled= false;
                    btnNewhand.IsEnabled = true;
                    btnDeal.IsEnabled= false;
                    btnHit.IsEnabled = false;
                    btnStand.IsEnabled = false;
                    btnSurrender.IsEnabled = false;
                    break;
                default:
                    btnSave.IsEnabled = false;
                    btnShuffle.IsEnabled = false;
                    btnNewhand.IsEnabled = false;
                    btnDeal.IsEnabled = false;
                    btnHit.IsEnabled = false;
                    btnStand.IsEnabled = false;
                    btnSurrender.IsEnabled = false;
                    break;
            }
        }

        private void btnNewgame_Click(object sender, RoutedEventArgs e)
        {

            setupWindow = new setup(this);
            setupWindow.Show();
        }

        internal void LoadPlayer1(PlayerProfile player)
        {
            Player newPlayer = new Player();
            newPlayer.PlayerName = player.PlayerName;
            newPlayer.Funds = player.Funds;
            _gameManager.insertPlayer(1, newPlayer);
            lblPlayer1.Content = player.PlayerName;
            UpdateInfoLabels(null);
        }

        private void btnResume_Click(object sender, RoutedEventArgs e)
        {
            LoadSaveGame loadWindow = new LoadSaveGame(this);
            loadWindow.Show();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (_gameManager == null) return;
            _gameManager.SaveGame();
            MessageBox.Show("Game saved successfully");
        }
    }
}
