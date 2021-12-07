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
        private int idStation;

        public DroneWindow(BL bL, DroneListWindow _droneListWindow)
        {
            InitializeComponent();
            droneListWindow = _droneListWindow;
            BLGui = bL;
            drone = new();
            DataContext = drone;

            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            WeightSelector.SelectedIndex = -1;

            stations.ItemsSource = BLGui.DisplaysIistOfStations();
        }

        public DroneWindow(DroneToList droneToList, BL bL, DroneListWindow _droneListWindow, int index)
        {
            droneListWindow = _droneListWindow;
            this.droneToList = droneToList;
            BLGui = bL;
            InitializeComponent();
        }

        private void stations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationToList station = (StationToList)stations.SelectedItem;
            idStation = station.Id;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult messageBoxResult = MessageBox.Show("hhhh", "gggg", MessageBoxButton.OK);
            switch (messageBoxResult)
            {
                case MessageBoxResult.None:
                    break;
                case MessageBoxResult.OK:
                    BLGui.addDrone(drone, idStation);
                    droneListWindow.droneToListsView.Add(BLGui.DisplaysIistOfDrons().First(i => i.Id == drone.Id));
                    MessageBoxResult messageBoxResult1 = MessageBox.Show("hhhh", "gggg", MessageBoxButton.OK);

                    switch (messageBoxResult1)
                    {
                        case MessageBoxResult.None:
                            break;
                        case MessageBoxResult.OK:
                            Close();
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        case MessageBoxResult.Yes:
                            break;
                        case MessageBoxResult.No:
                            break;
                        default:
                            break;
                    }
                    break;
                case MessageBoxResult.Cancel:
                    break;
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                    break;
                default:
                    break;
            }
        }
    }
}
