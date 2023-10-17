using System.Windows;

namespace WPFBlackjack
{
    /// <summary>
    /// Interaction logic for setup.xaml
    /// </summary>
    public partial class setup : Window
    {
        MainWindow parentWindow;
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
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            parentWindow.Decks = (int)cBoxDecks.SelectedItem;
            parentWindow.Players = (int)cBoxPlayers.SelectedItem;
            parentWindow.StartGame();
            this.Hide();
        }
    }
}
