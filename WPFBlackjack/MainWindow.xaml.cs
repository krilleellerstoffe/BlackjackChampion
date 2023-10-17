using CardGameLib;
using System;
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
            gameManager.CardDrawn += updateLabelsAndImages;
            gameManager.Bust += ShowBustMessage;
            btnDeal.IsEnabled = true;
            updateLabelsAndImages();
        }
        public void ShowBustMessage(Player player)
        {
            MessageBox.Show("BUST!");
        }
        private void updateLabelsAndImages()
        {
            UpdateInfoLabels();
            UpdateDealerCards();
            UpdatePlayerCards();

        }

        private void UpdateInfoLabels()
        {
            lblCardsInShoe.Content = gameManager.Shoe.Cards.Count;
            lblDecks.Content = gameManager.Decks.Length;
            lblPlayerCount.Content = gameManager.Players.Length-1;
            lblCardsSinceShuffle.Content = gameManager.Shoe.CardsSinceLastShuffle;
            lblShuffle.Content = gameManager.Shoe.TimeToShuffle(4);
        }
        private void UpdateDealerCards()
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
        private void UpdatePlayerCards()
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
                    ShowImageBasedOnCard((Image)tableCanvas.FindName("imgPlayer" + j +"Card" + (i + 1)), (Card)playerCards.Items[i]);
                }
            }
            if (lstPlayer1Cards.Items.Count >= 7) btnHit.IsEnabled = false;
            lblHandValue.Content = gameManager.Players[1].Hand.HandValue();

        }

        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Deal();
            btnDeal.IsEnabled = false;
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
        }

        private void btnSurrender_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnNewhand_Click(object sender, RoutedEventArgs e)
        {
            gameManager.NewHand();
            //maybe create game states that take care of active/inactive buttons?
            btnDeal.IsEnabled = true;
            btnNewhand.IsEnabled = false;
            btnHit.IsEnabled = false;
            updateLabelsAndImages();
        }


        private void btnNewgame_Click(object sender, RoutedEventArgs e) => setupWindow.Show();
        
    }
}
