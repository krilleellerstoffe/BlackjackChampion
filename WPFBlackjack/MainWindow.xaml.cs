using CardGameLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Xml.Linq;
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
                if (result == MessageBoxResult.Yes) _gameManager.SavePlayerToDatabase();
            }
            base.OnClosed(e);
        }
        public MainWindow()
        {
            InitializeComponent();
        }

        public void StartGame()
        {
            string[] randomNames = new string[_players + 1];
            var uri = new Uri("pack://application:,,,/Resources/RandomNames.txt");
            var info = Application.GetResourceStream(uri);
            if (info != null)
            {
                using (var reader = new StreamReader(info.Stream))
                {
                    string text = reader.ReadToEnd();

                    // Split the text into lines
                    string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    for (int i = 1; i < randomNames.Length; i++)
                    {
                        string newRandomName = lines[new Random().Next(lines.Length)];
                        randomNames[i] = newRandomName.Trim();
                    }
                }
            }
            //create new game based on setup
            StartGame(new GameManager(_decks, _players, randomNames));
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
            _gameManager.State = StateofPlay.WinnerDeclared;
            UpdateButtons();
            MessageBox.Show(winnerString);
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
            for (int i = 0; i <= 5; i++)
            {
                try
                {
                    if (i != 0)
                    {
                        ((Label)tableCanvas.FindName("lblFundsPlayer" + i)).Content = _gameManager.Players[i].Funds;
                        ((Label)tableCanvas.FindName("lblPlayer" + i)).Content = _gameManager.Players[i].PlayerName;

                    }
                    ((Label)tableCanvas.FindName("lblStatePlayer" + i)).Content = _gameManager.Players[i].PlayerState;
                }
                catch
                {
                    if (i != 0)
                    {
                        ((Label)tableCanvas.FindName("lblFundsPlayer" + i)).Content = "";
                    ((Label)tableCanvas.FindName("lblPlayer" + i)).Content = "Player " + i;
                    }
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
            for (int j = 1; j <= 5; j++)
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
                case StateofPlay.WinnerDeclared:
                    SetButtons(new int[] { 1, 0, 1, 0, 0, 0, 0 });
                    break;
                case StateofPlay.NewHand:
                    SetButtons(new int[] { 1, 1, 0, 1, 0, 0, 0 });
                    break;
                case StateofPlay.AfterDeal:
                    SetButtons(new int[] { 1, 0, 0, 0, 1, 1, 1 });
                    break;
                case StateofPlay.AfterHit:
                    SetButtons(new int[] { 1, 0, 0, 0, 1, 1, 0 });
                    break;
                case StateofPlay.PlayerStanding:
                    SetButtons(new int[] { 1, 0, 1, 0, 0, 0, 0 });
                    break;
                default:
                    SetButtons(new int[] { 0, 0, 0, 0, 0, 0, 0 });
                    break;
            }
        }

        private void SetButtons(int[] integerArray)
        {
            bool[] enabledButtons = integerArray.Select(i => i != 0).ToArray();

            btnSave.IsEnabled = enabledButtons[0];
            btnShuffle.IsEnabled = enabledButtons[1];
            btnNewhand.IsEnabled = enabledButtons[2];
            btnDeal.IsEnabled = enabledButtons[3];
            btnHit.IsEnabled = enabledButtons[4];
            btnStand.IsEnabled = enabledButtons[5];
            btnSurrender.IsEnabled = enabledButtons[6];
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
