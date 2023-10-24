using CardGameLib;
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
        private GameManager gameManager;
        private setup setupWindow;
        private int _decks;
        private int _players;
        private int _shuffleThreshold = 4; //at what fraction to ask to shuffle

        public int Decks { get => _decks; set => _decks = value; }
        public int Players { get => _players; set => _players = value; }
        protected override void OnClosed(EventArgs e)
        {
            if (gameManager != null)
            {
                MessageBoxResult result = MessageBox.Show("Save '" + gameManager.Players[1].PlayerName + "' to database?", "Exit game", MessageBoxButton.YesNo, MessageBoxImage.Question);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        gameManager.SavePlayerToDatabase();
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
            gameManager = new GameManager(_decks, _players);
            StartGame(gameManager);
        }

        public void StartGame(GameManager gameManager)
        {
            //subscribe to events in gamemanager
            gameManager.CardDrawn += UpdateInfoLabels;
            gameManager.CardDrawn += ShowHideSingleDealerCard;
            gameManager.CardDrawn += UpdatePlayerCards;
            gameManager.Bust += ShowBustMessage;
            gameManager.Bust += UpdateInfoLabels;
            gameManager.Standing += ShowStandMessage;
            gameManager.Standing += UpdateInfoLabels;
            gameManager.Results += ShowHideAllDealerCards;
            gameManager.Results += UpdateInfoLabels;
            gameManager.Results += ShowWinners;
            //now let user deal
            btnNewhand.IsEnabled = true;
            btnSave.IsEnabled = true;
            UpdateInfoLabels(null);
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
            btnHit.IsEnabled = false;
            btnStand.IsEnabled = false;
            //gameManager.Stand(1);
            btnNewhand.IsEnabled = true;
            btnSurrender.IsEnabled = false;
            gameManager.SplitPotToWinners();
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
            lblCardsInShoe.Content = gameManager.Shoe.Cards.Count;
            lblDecks.Content = gameManager.Decks.Length;
            lblPlayerCount.Content = gameManager.Players.Length - 1;
            lblCardsSinceShuffle.Content = gameManager.Shoe.CardsSinceLastShuffle;
            lblShuffle.Content = gameManager.Shoe.TimeToShuffle(_shuffleThreshold);
            lblPot.Content = gameManager.Pot;
            for (int i = 0; i < gameManager.Players.Length; i++)
            {
                if (i != 0) ((Label)tableCanvas.FindName("lblFundsPlayer" + i)).Content = gameManager.Players[i].Funds;
                ((Label)tableCanvas.FindName("lblStatePlayer" + i)).Content = gameManager.Players[i].PlayerState;
            }
        }
        //dealer gets special methods as we don't always want to reveal more than the first card
        //reveal or hide first dealer card
        private void ShowHideSingleDealerCard(object? obj, object? obj1)
        {
            lstDealerCards.Items.Clear();
            foreach (Card card in gameManager.Players[0].Hand.Cards)
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
            for (int i = 0; i < lstDealerCards.Items.Count; i++)
            {
                ((Image)tableCanvas.FindName("imgDealerCard" + (i + 1))).Visibility = Visibility.Hidden;
            }
            lstDealerCards.Items.Clear();
            foreach (Card card in gameManager.Players[0].Hand.Cards)
            {
                lstDealerCards.Items.Add(card);
            }
            for (int i = 0; i < lstDealerCards.Items.Count; i++)
            {
                ShowImageBasedOnCard((Image)tableCanvas.FindName("imgDealerCard" + (i + 1)), (Card)lstDealerCards.Items[i]);
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
            for (int j = 1; j < gameManager.Players.Length; j++)
            {
                ListBox playerCards = (ListBox)playerCanvas.FindName("lstPlayer" + j + "Cards");
                for (int i = 0; i < playerCards.Items.Count; i++)
                {
                    ((Image)tableCanvas.FindName("imgPlayer" + j + "Card" + (i + 1))).Visibility = Visibility.Hidden;
                }
                playerCards.Items.Clear();
                foreach (Card card in gameManager.Players[j].Hand.Cards)
                {
                    playerCards.Items.Add(card);
                }
                for (int i = 0; i < playerCards.Items.Count; i++)
                {
                    ShowImageBasedOnCard((Image)tableCanvas.FindName("imgPlayer" + j + "Card" + (i + 1)), (Card)playerCards.Items[i]);
                }
            }
            if (lstPlayer1Cards.Items.Count >= 7) btnHit.IsEnabled = false;
            lblHandValue.Content = gameManager.Players[1].Hand.HandValue();
        }
        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Deal();
            btnDeal.IsEnabled = false;
            btnShuffle.IsEnabled = false;
            btnHit.IsEnabled = true;
            btnStand.IsEnabled = true;
            btnSurrender.IsEnabled = true;
        }

        private void btnShuffle_Click(object sender, RoutedEventArgs e) => gameManager.Shoe.Shuffle();

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            btnSurrender.IsEnabled = false;
            gameManager.Hit(gameManager.Players[1]);
        }

        private void btnStand_Click(object sender, RoutedEventArgs e)
        {
            btnHit.IsEnabled = false;
            btnStand.IsEnabled = false;
            gameManager.Stand(1);
            btnNewhand.IsEnabled = true;
            btnSurrender.IsEnabled = false;
            UpdateInfoLabels(sender);
        }

        private void btnSurrender_Click(object sender, RoutedEventArgs e) => gameManager.Surrender();

        private void btnNewhand_Click(object sender, RoutedEventArgs e)
        {
            gameManager.NewHand();
            btnDeal.IsEnabled = true;
            btnShuffle.IsEnabled = true;
            btnNewhand.IsEnabled = false;
            btnHit.IsEnabled = false;
            if (gameManager.Shoe.TimeToShuffle(_shuffleThreshold))
            {
                MessageBoxResult result = MessageBox.Show((_shuffleThreshold - 1) + "/" + _shuffleThreshold + " of the cards in the shoe" +
                    " have been used since you last shuffled, shuffle now?", "Shuffle cards in shoe?", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    gameManager.Shoe.Shuffle();
                }
            }
            UpdateInfoLabels(null);
            UpdatePlayerCards(null);
            ShowHideAllDealerCards(null);

        }

        private void btnNewgame_Click(object sender, RoutedEventArgs e)
        {

            setupWindow = new setup(this);
            setupWindow.Show();
        }

        internal void LoadPlayer1(Player player)
        {
            gameManager.insertPlayer(1, player);
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
            if (gameManager == null) return;
            gameManager.SaveGame();
        }
    }
}
