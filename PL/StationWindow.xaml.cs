using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BO;
using BlApi;
using System.Collections.ObjectModel;

namespace PL
{


    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        private IBL blGui;
        ObservableCollection<StationToList> StationsToListView;
        private Station station;
        private StationToList stationToList;
        Actions actions;
        string action;
        public StationWindow(IBL bL, string _action = "")
        {
            blGui = bL;
            actions = new();
            action = _action;
            StationsToListView = new();
            stationToList = new();
            InitList();
            InitializeComponent();
            switch (action)
            {
                case "List":
                    ListWindow();
                    break;
                case "Updating":
                    BorderEnterNumber.Visibility = Visibility.Visible;
                    update.Visibility = Visibility.Hidden;
                    addButton.Content = "הצג";
                    Close.Content = "סגור";
                    actions = Actions.UPDATING;
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
            actions = Actions.ADD;
            addButton.Content = "הוסף";
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Visible;
            station = new();
            station.location = new();
            DataContext = station;
        }

        private void ListWindow()
        {

            StationListView.Items.Refresh();
            actions = Actions.LIST;
            addButton.Content = "הוסף תחנה";
            Close.Content = "סגור";
            List.Visibility = Visibility.Visible;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Hidden;
            StationListView.ItemsSource = StationsToListView;
            SlutsSelector.ItemsSource = Enum.GetValues(typeof(SlotsSTatus));

            SlutsSelector.SelectedIndex = 0;
            //droneToListsView.CollectionChanged += DroneToListsView_CollectionChanged;
        }
        private void UpdatingWindow(int id)
        {
            //if (action == "Updating")
            //{
            //    station = blGui.GetStation(id);
            //}
           
            
            
            
            actions = Actions.UPDATING;
            addButton.Content = "עדכן";
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Hidden;
            station = blGui.GetStation(id);
            Drones.ItemsSource = station.droneInCargeings.ToList();
            DataContext = station;
        }

        public void InitList()//
        {
            List<StationToList> temp = blGui.DisplaysIistOfStations().ToList();
            foreach (StationToList item in temp)
            {
                StationsToListView.Add(item);
            }
        }
        private void SlutsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SlotsSTatus slotsSTatus = (SlotsSTatus)SlutsSelector.SelectedItem;

            if (slotsSTatus == SlotsSTatus.הכל)
                StationListView.ItemsSource = StationsToListView;

            else if (slotsSTatus == SlotsSTatus.פנוי)
                StationListView.ItemsSource = StationsToListView.ToList().FindAll(i => i.freeChargeSlots > 0);

            else if (slotsSTatus == SlotsSTatus.מלא)
                StationListView.ItemsSource = StationsToListView.ToList().FindAll(i => i.freeChargeSlots <= 0);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            switch (actions)
            {
                case Actions.LIST:
                    AddWindow();
                    break;
                case Actions.ADD:

                    if (station.Id != default && station.name != default && station.location.longitude != default && station.location.latitude != default && station.freeChargeSlots != default)//להעביר את הבדיקה לאיז אנעבלעד 
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);//לשפר סטייל של ההודעה
                        switch (messageBoxResult)
                        {

                            case MessageBoxResult.OK:
                                _ = blGui.addStation(station);
                                StationsToListView.Add(blGui.DisplaysIistOfStations().First(i => i.Id == station.Id));
                                MessageBox.Show("התחנה נוצרה בהצלחה\n מיד תוצג רשימת התחנות", "אישור");
                                ListWindow();
                                break;
                            case MessageBoxResult.Cancel:
                                break;
                            default:
                                break;

                        }
                    }
                    else
                        MessageBox.Show("נא השלם את השדות החסרים", "אישור");
                    break;
                //case Actions.UPDATING:
                    if (addButton.Content == "הצג")
                    {
                        var idFind = StationsToListView.ToList().Find(i => i.Id == int.Parse(TxtBx_ID.Text.ToString()));
                        if (idFind != default)
                        {

                            BorderEnterNumber.Visibility = Visibility.Hidden;
                            update.Visibility = Visibility.Visible;
                            UpdatingWindow(idFind.Id);

                        }
                        else
                        {
                            MessageBox.Show("התחנה המבוקשת לא נמצאה", "אישור");
                            Close();
                        }
                        break;
                    }
                    //if (drone.Model != default)//איז אנעבעלד
                    //{
                    //    MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר עדכון זה", "אישור", MessageBoxButton.OKCancel);
                    //    switch (messageBoxResult)
                    //    {
                    //        case MessageBoxResult.OK:
                    //            if (relase.IsChecked == true)
                    //            {
                    //                relase.IsChecked = false;
                    //                blGui.ReleaseDroneFromCharging(drone.Id, 7);
                    //                BorderStation.Visibility = Visibility.Hidden;
                    //            }
                    //            droneToList.Model = drone.Model;
                    //            _ = blGui.updateModelOfDrone(droneToList.Model, droneToList.Id);
                    //            // droneToListsView[index] = blGui.DisplaysIistOfDrons().First(i => i.Id == droneToList.Id);
                    //            MessageBox.Show("העדכון בוצע בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                    //            ListWindow();
                    //            break;
                    //        case MessageBoxResult.Cancel:
                    //            break;
                    //        default:
                    //            break;
                    //    }
                    //}
                    //else
                    //    MessageBox.Show("נא השלם את השדות החסרים", "אישור");
                    //break;
                    //    case Actions.REMOVE:
                    //        break;
                    //default:
                    //        break;
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            switch (action)
            {
                case "List":
                    if (actions != Actions.LIST)
                    {
                        ListWindow();
                        StationListView.SelectedItem = null;
                    }
                    else
                        Close();
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void Drones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
