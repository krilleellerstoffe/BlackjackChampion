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
            gameManager = new GameManager(8, 2);
            updateLabels();
        }

        private void updateLabels()
        {
            //adjust these so that the methods are in gamemanager, limiting exposure of BLL classes
            lblCardsInShoe.Content = gameManager.Shoe.TotalCards.ToString();
            lblDecks.Content = gameManager.Decks.Length.ToString();
            lblPlayerCount.Content = gameManager.Players.Length.ToString();
            lblUsedCards.Content = gameManager.Shoe.UsedCards.ToString();
            lblShuffle.Content = gameManager.Shoe.TimeToShuffle(4).ToString();
            lstPlayerCards.Items.Clear();
            foreach(Card card in gameManager.Players[0].Hand.Cards)
            {
                lstPlayerCards.Items.Add(card);
            }
        }

        private void btnDeal_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Deal();
            updateLabels();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            gameManager.Hit(gameManager.Players[0]);
            updateLabels();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }
    }
}
