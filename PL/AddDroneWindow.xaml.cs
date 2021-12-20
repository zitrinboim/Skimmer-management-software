using BO;
using BlApi;
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

    public partial class AddDroneWindow : Window
    {

        IBL BLGui;
        private DroneToList droneToList;
        private DroneListWindow droneListWindow;
        private MainWindow mainWindow;
        private Drone drone;
        private int idStation = new();
        private bool? addOrUpdate = null;
        int index;
        public AddDroneWindow(IBL bL, DroneListWindow _droneListWindow)
        {
            InitializeComponent();
            addOrUpdate = true;
            add.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Hidden;
            droneListWindow = _droneListWindow;
            BLGui = bL;
            drone = new();
            DataContext = drone;

            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            WeightSelector.SelectedIndex = -1;

            stations.ItemsSource = BLGui.DisplaysIistOfStations();
        }
        public AddDroneWindow(IBL bL, MainWindow _mainWindow )
        {
            InitializeComponent();
            addOrUpdate = true;
            add.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Hidden;
            mainWindow = _mainWindow;
            BLGui = bL;
            drone = new();
            DataContext = drone;

            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            WeightSelector.SelectedIndex = -1;

            stations.ItemsSource = BLGui.DisplaysIistOfStations();
        }

        public AddDroneWindow(DroneToList droneToList, IBL bL, DroneListWindow _droneListWindow, int _index)
        {
            InitializeComponent();
            addOrUpdate = false;
            addButton.Content = "עדכן";
            add.Visibility = Visibility.Hidden;
            update.Visibility = Visibility.Visible;
            index = _index;
            droneListWindow = _droneListWindow;
            this.droneToList = droneToList;
            BLGui = bL;
            drone = BLGui.GetDrone(droneToList.Id);
            DataContext = drone;

            // BLGui.Dron
            // BLProgram.updateModelOfDrone(newModel, IdDrone)

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (addOrUpdate == true)
            {
                if (drone.Id != default && drone.Model != default && drone.MaxWeight != default && idStation != default)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);
                    switch (messageBoxResult)
                    {

                        case MessageBoxResult.OK:
                            BLGui.addDrone(drone, idStation);
                            mainWindow.droneToListsView.Add(BLGui.DisplaysIistOfDrons().First(i => i.Id == drone.Id));
                            MessageBox.Show("הרחפן נוצר בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                            new DroneListWindow(BLGui, mainWindow.droneToListsView).Show();
                            Close();
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        default:
                            break;



                    }
                }
                else
                    MessageBox.Show("נא השלם את השדות החסרים", "אישור");
            }
            else
            {
                if (drone.Model != default)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר עדכון זה", "אישור", MessageBoxButton.OKCancel);
                    switch (messageBoxResult)
                    {
                        case MessageBoxResult.OK:
                            // droneListWindow.droneToListsView.Remove(BLGui.DisplaysIistOfDrons().First(i => i.Id == drone.Id));
                            droneToList.Model = drone.Model;
                            //   droneListWindow.droneToListsView[index] = droneToList;
                            BLGui.updateModelOfDrone(droneToList.Model, droneToList.Id);
                            mainWindow.droneToListsView[index] = BLGui.DisplaysIistOfDrons().First(i => i.Id == droneToList.Id);
                            droneListWindow.DroneListView.Items.Refresh();
                            // droneListWindow.droneToListsView.Add(BLGui.DisplaysIistOfDrons().First(i => i.Id == droneToList.Id));
                            MessageBox.Show("העדכון בוצע בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                            new DroneListWindow(BLGui, mainWindow.droneToListsView).Show();
                            Close();
                            break;
                        case MessageBoxResult.Cancel:
                            break;
                        default:
                            break;

                    }
                }
                else
                    MessageBox.Show("נא השלם את השדות החסרים", "אישור");
            }

        }

        private void stations_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            StationToList station = (StationToList)stations.SelectedItem;
            idStation = station.Id;
        }

        private void parcelToDrone_Click(object sender, RoutedEventArgs e)
        {
           
            ////bool b = BLGui.AssignPackageToDrone(droneToList.Id);
            //if (b == true)
            //{
            //    BO.Drone drone1 = BLGui.GetDrone(droneToList.Id);
            //    DataContext = drone1.packageInTransfer;

            //}           //איך גורמים לשינוי להופיע במיידי
            //else
            //{
            //    MessageBox.Show("לא נמצאה חבילה מתאימה עבור רחפן זה", "אישור");

            //}
        }
    }
}
