using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for InsertName.xaml
    /// </summary>
    public partial class InsertName : Window
    {
        public InsertName()
        {
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
    }
}
