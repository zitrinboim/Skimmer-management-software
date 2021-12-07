using IBL.BO;
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
    /// Interaction logic for AddDrone.xaml
    /// </summary>
   
    public partial class DroneWindow : Window
    {
      //  public enum WeightCategories { All, easy, medium, heavy };

        IBL.BO.BL BLGui;
        private DroneToList droneToList;
        private DroneListWindow droneListWindow;
        private Drone drone;

        public DroneWindow(BL bL, DroneListWindow _droneListWindow)
        {
            InitializeComponent();
            droneListWindow = _droneListWindow;
            BLGui = bL;
            drone = new();
            DataContext = this;

            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            WeightSelector.SelectedIndex = 0;
           
            stations.ItemsSource = BLGui.DisplaysIistOfStations();

            
          
        }

        public DroneWindow(DroneToList droneToList, BL bL, DroneListWindow _droneListWindow,   int index)
        {
            droneListWindow = _droneListWindow;
            this.droneToList = droneToList;
            BLGui = bL;
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void stations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            BLGui.addDrone(drone);
        }
    }
}
