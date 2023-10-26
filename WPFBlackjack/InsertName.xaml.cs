using System;
using System.IO;
using System.Windows;
using WPFBlackjackEL;

namespace WPFBlackjack
{
    /// <summary>
    /// Interaction logic for InsertName.xaml
    /// </summary>
    public partial class InsertName : Window
    {
        private setup _parent;
        public InsertName(setup parent)
        {
            _parent = parent;
            InitializeComponent();
        }

        private void btnRandom_Click(object sender, RoutedEventArgs e)
        {
            var uri = new Uri("pack://application:,,,/Resources/RandomNames.txt");
            var info = Application.GetResourceStream(uri);

            if (info != null)
            {
                using (var reader = new StreamReader(info.Stream))
                {
                    string text = reader.ReadToEnd();

                    // Split the text into lines
                    string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                    string newRandomName = lines[new Random().Next(lines.Length)];
                    txtName.Text = newRandomName.Trim(); ;
                }
            }

        }

        private void btnNewPlayer_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Length > 0)
            {
                PlayerProfile newPlayer = new PlayerProfile(txtName.Text, 100);
                _parent.ParentWindow.LoadPlayer1(newPlayer);
                this.Close();
            }
        }
    }
}
