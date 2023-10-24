using CardGameLib;
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
using WPFBlackjackEL;

namespace WPFBlackjack
{
    /// <summary>
    /// Interaction logic for LoadSaveGame.xaml
    /// </summary>
    public partial class LoadSaveGame : Window
    {
        public LoadSaveGame()
        {
            InitializeComponent();
            UpdateSaveGameList();
        }

        private void UpdateSaveGameList()
        {
            lstSaveGames.Items.Clear();
            List<GameState> saveGames = GameManager.GetSaveGamesFromDatabase();
            foreach (GameState saveGame in saveGames)
            {
                lstSaveGames.Items.Add(saveGame);
            }
        }
    }
}
