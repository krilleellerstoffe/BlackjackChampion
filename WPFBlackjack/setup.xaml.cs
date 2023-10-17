using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

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
