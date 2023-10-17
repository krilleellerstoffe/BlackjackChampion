using CardGameLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
        private int _shuffleThreshold = 4;

        public int Decks { get => _decks; set => _decks = value; }
        public int Players { get => _players; set => _players = value; }

        public MainWindow()
        {
            InitializeComponent();
            setupWindow = new setup(this);
        }

        public void StartGame()
        {
            gameManager = new GameManager(_decks, _players);
            //subscribe to events in gamemanager
            gameManager.CardDrawn += UpdateInfoLabels;
            gameManager.CardDrawn += UpdateDealerCards;
            gameManager.CardDrawn += UpdatePlayerCards;
            gameManager.Bust += ShowBustMessage;
            gameManager.Bust += UpdateInfoLabels;
            gameManager.Results += ShowWinners;
            gameManager.Results += UpdateInfoLabels;
            btnDeal.IsEnabled = true;
            UpdateInfoLabels(null);
        }

        private async void ShowWinners(List<Player> winners)
        {
            await Task.Delay(2000);
            string winnerString = "Winners:\n";
            foreach (Player player in winners)
            {
                winnerString += player.PlayerName + "\n";
            }
            MessageBox.Show(winnerString);
            btnHit.IsEnabled = false;
            btnStand.IsEnabled = false;
            gameManager.Stand(1);
            btnNewhand.IsEnabled = true;
            btnSurrender.IsEnabled = false;
            //updateLabelsAndImages(null);

        }

        public void ShowBustMessage(Player player)
        {
            //updateLabelsAndImages(player);
            if (player.PlayerNumber == 1) MessageBox.Show(player.PlayerName + " BUST!");
        }


        private void UpdateInfoLabels(object? obj)
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
        //dealer gets a special method as we don't want to reveal more than the first card
        private void UpdateDealerCards(object? obj)
        {
            lstDealerCards.Items.Clear();
            foreach (Card card in gameManager.Players[0].Hand.Cards)
            {
                lstDealerCards.Items.Add(card);
            }

            if (lstDealerCards.Items.Count > 0)
            {
                ShowImageBasedOnCard(imgDealerCardFront, (Card)lstDealerCards.Items[0]);
            }
            else
            {
                imgDealerCardFront.Visibility = Visibility.Hidden;
            }
        }

        private void ShowImageBasedOnCard(Image image, Card card)
        {
            image.Visibility = Visibility.Visible;
            string cardString = card.ToString();
            cardString = cardString.Replace(' ', '_');
            image.Source = new BitmapImage(new Uri("Resources/" + cardString + ".png", UriKind.Relative));

        }
        private void UpdatePlayerCards(object? obj)
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

        private async void btnHit_Click(object sender, RoutedEventArgs e)
        {
            btnSurrender.IsEnabled = false;
            await gameManager.Hit(gameManager.Players[1]);
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

        private void btnSurrender_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Surrender();
        }

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
            UpdateDealerCards(null);
        }


        private void btnNewgame_Click(object sender, RoutedEventArgs e) => setupWindow.Show();

    }
}
