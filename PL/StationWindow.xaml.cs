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

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        public enum SlotsSTatus { הכל,פנוי };
        public StationWindow()
        {
            InitializeComponent();
            SlutsSelector.ItemsSource = Enum.GetValues(typeof(SlotsSTatus));
        }

        private void SlutsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
