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

        public MainWindow()
        {
            InitializeComponent();
            gameManager = new GameManager(1, 2);
            updateLabelsAndImages();
            //subscribe to events in gamemanager
            gameManager.CardDrawn += updateLabelsAndImages;
            gameManager.Bust += ShowBustMessage;
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
            for (int i = 0; i < lstPlayerCards.Items.Count; i++)
            {
                ((Image)tableCanvas.FindName("imgPlayer1Card" + (i + 1))).Visibility = Visibility.Hidden;
            }
            lstPlayerCards.Items.Clear();
            foreach (Card card in gameManager.Players[1].Hand.Cards)
            {
                lstPlayerCards.Items.Add(card);
            }
            for (int i = 0; i < lstPlayerCards.Items.Count; i++)
            {
                ShowImageBasedOnCard((Image)tableCanvas.FindName("imgPlayer1Card" + (i + 1)), (Card)lstPlayerCards.Items[i]);
            }
            if (lstPlayerCards.Items.Count >= 7) btnHit.IsEnabled = false;
            lblHandValue.Content = gameManager.Players[1].Hand.HandValue();

        }

        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Deal();
            btnDeal.IsEnabled = false;
            btnNewhand.IsEnabled = true;
            btnHit.IsEnabled = true;
        }



        private void btnShuffle_Click(object sender, RoutedEventArgs e) => gameManager.Shoe.Shuffle();

        private void btnHit_Click(object sender, RoutedEventArgs e) => gameManager.Hit(gameManager.Players[1]);

        private void btnStand_Click(object sender, RoutedEventArgs e) => gameManager.Stand(1);

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
    }
}
