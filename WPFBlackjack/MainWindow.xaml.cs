using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CardGameLib;

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
            gameManager = new GameManager(2, 3);
            updateLabels();
        }

        private void updateLabels()
        {
            //adjust these so that the methods are in gamemanager, limiting exposure of BLL classes?
            lblCardsInShoe.Content = gameManager.Shoe.Cards.Count;
            lblDecks.Content = gameManager.Decks.Length;
            lblPlayerCount.Content = gameManager.Players.Length;
            lblCardsSinceShuffle.Content = gameManager.Shoe.CardsSinceLastShuffle;
            lblShuffle.Content = gameManager.Shoe.TimeToShuffle(4);
            UpdateDealerCards();
            UpdatePlayerCards();

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
                ShowImageBasedOnCard(imgDealerCardBack, (Card)lstDealerCards.Items[0]);
            }
            else
            {
                imgDealerCardBack.Visibility = Visibility.Hidden;
            }
        }
        
        private void ShowImageBasedOnCard(Image image, Card card)
        {
            image.Visibility = Visibility.Visible;
            string cardString = card.IntValueString();
            image.Source = new BitmapImage(new Uri("Resources/" + cardString + ".png", UriKind.Relative));

        }
        private void UpdatePlayerCards()
        {
            lstPlayerCards.Items.Clear();
            foreach (Card card in gameManager.Players[1].Hand.Cards)
            {
                lstPlayerCards.Items.Add(card);
            }
            lblHandValue.Content = gameManager.Players[1].Hand.HandValue();
                        
        }

        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Deal();
            btnDeal.IsEnabled = false;
            btnNewhand.IsEnabled = true;
            btnHit.IsEnabled = true;
            updateLabels();
        }



        private void btnShuffle_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Shoe.Shuffle();
            updateLabels();
        }

        private void btnHit_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Hit(gameManager.Players[1]);
            updateLabels();
        }

        private void btnStand_Click(object sender, RoutedEventArgs e)
        {

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
            updateLabels();
        }
    }
}
