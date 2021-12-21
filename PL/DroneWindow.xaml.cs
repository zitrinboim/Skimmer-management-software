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
using BO;
using BlApi;
using System.Collections.ObjectModel;

namespace PL
{
    public enum WeightCategories { All, easy, medium, heavy };
    public enum DroneStatuses { All, available, maintenance, busy };

    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL blGui;
        ObservableCollection<DroneToList> droneToListsView;
        private Drone drone = new();
        private DroneToList droneToList;
        private int idStation = new();
        private bool? addOrUpdate = null;//נחשוב
        int index;

        public DroneWindow(IBL bL, string action = "")
        {
            blGui = bL;
            droneToListsView = new();
            InitList();
            InitializeComponent();

            switch (action)
            {
                case "List":
                    List.Visibility = Visibility.Visible;
                    Updating.Visibility = Visibility.Hidden;
                    Add.Visibility = Visibility.Hidden;
                    DroneListView.ItemsSource = droneToListsView;
                    StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
                    WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
                    StatusSelector.SelectedIndex = 0;
                    droneToListsView.CollectionChanged += DroneToListsView_CollectionChanged;
                    break;
                case "Updating":
                    int id = 0;
                    UpdatingWindow(id);
                    break;
                case "Add":
                    AddWindow();
                    break;

                default:
                    break;
            }
        }

        private void AddWindow()
        {
            addOrUpdate = true;
            addButton.Content = "הוסף";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Visible;
            drone = new();
            DataContext = drone;
            WeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            WeightSelector.SelectedIndex = -1;
            stations.ItemsSource = blGui.DisplaysIistOfStations(i => i.freeChargeSlots > 0);
        }

        private void UpdatingWindow(int id)
        {
            addOrUpdate = false;
            addButton.Content = "עדכן";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Hidden;
            drone = blGui.GetDrone(id);
            DataContext = drone;
        }

        public void InitList()//
        {
            List<DroneToList> temp = blGui.DisplaysIistOfDrons().ToList();
            foreach (DroneToList item in temp)
            {
                droneToListsView.Add(item);
            }
        }
        private void DroneToListsView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StatusSelectorAndWeightSelector();
        }

        private void StatusSelectorAndWeightSelector()
        {
            if (WeightSelector.SelectedIndex == -1)
            {
                WeightSelector.SelectedIndex = 0;
            }
            WeightCategories weightCategories = (WeightCategories)WeightSelector.SelectedItem;
            DroneStatuses droneStatuses = (DroneStatuses)StatusSelector.SelectedItem;

            if (weightCategories == WeightCategories.All && droneStatuses == DroneStatuses.All)
                DroneListView.ItemsSource = droneToListsView;

            else if (weightCategories != WeightCategories.All && droneStatuses == DroneStatuses.All)
                DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.MaxWeight == (BO.WeightCategories)weightCategories);

            else if (weightCategories == WeightCategories.All && droneStatuses != DroneStatuses.All)
                DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.DroneStatuses == (BO.DroneStatuses)droneStatuses);

            else
                DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.MaxWeight == (BO.WeightCategories)weightCategories && i.DroneStatuses == (BO.DroneStatuses)droneStatuses);
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelectorAndWeightSelector();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelectorAndWeightSelector();
        }
        private void Button_Click_addDrone(object sender, RoutedEventArgs e)
        {
            AddWindow();
        }

        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            droneToList = (DroneToList)DroneListView.SelectedItem;
            int index = DroneListView.SelectedIndex;
            if (droneToList != null)
            {
                UpdatingWindow(droneToList.Id);
            }
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
                            blGui.addDrone(drone, idStation);
                            droneToListsView.Add(blGui.DisplaysIistOfDrons().First(i => i.Id == drone.Id));
                            MessageBox.Show("הרחפן נוצר בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                            //new DroneListWindow(blGui, mainWindow.droneToListsView).Show();
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
                            blGui.updateModelOfDrone(droneToList.Model, droneToList.Id);
                            droneToListsView[index] = blGui.DisplaysIistOfDrons().First(i => i.Id == droneToList.Id);
                            DroneListView.Items.Refresh();
                            // droneListWindow.droneToListsView.Add(BLGui.DisplaysIistOfDrons().First(i => i.Id == droneToList.Id));
                            MessageBox.Show("העדכון בוצע בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                            //new DroneListWindow(BLGui, mainWindow.droneToListsView).Show();
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


        }
    }
}
