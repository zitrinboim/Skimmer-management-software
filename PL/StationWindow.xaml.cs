using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BO;
using BlApi;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Text.RegularExpressions;
using System.Windows.Input;

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
            station = new();
            station.location = new();
            DataContext = station;
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
                    List<StationToList> stations  = blGui.DisplaysIistOfStations().ToList();
                    var stationCombo = from item in stations
                                        select item.Id;
                    comboID.ItemsSource = stationCombo;
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
            AddGrouping();
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
                case Actions.UPDATING:
                    if (addButton.Content == "הצג")
                    {
                      stationToList = StationsToListView.ToList().Find(i => i.Id == station.Id);
                       
                            BorderEnterNumber.Visibility = Visibility.Hidden;
                            update.Visibility = Visibility.Visible;
                            UpdatingWindow(stationToList.Id);
                       
                        break;
                    }


                    if (station.name != default || (station.freeChargeSlots != default && station.freeChargeSlots >= station.droneInCargeings.Count))//איז אנעבעלד
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר עדכון זה", "אישור", MessageBoxButton.OKCancel);
                        switch (messageBoxResult)
                        {
                            case MessageBoxResult.OK:
                               
                                stationToList.name = station.name;
                                stationToList.freeChargeSlots = station.freeChargeSlots;
                                _ = blGui.updateStationData(station.Id, station.name, station.freeChargeSlots);
                                // droneToListsView[index] = blGui.DisplaysIistOfDrons().First(i => i.Id == droneToList.Id);
                                MessageBox.Show("העדכון בוצע בהצלחה\n מיד תוצג רשימת התחנות", "אישור");
                                StationListView.SelectedItem = null;
                                StationListView.Items.Refresh();

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
                case Actions.REMOVE:
                    break;
                default:
                    break;
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

        private void StationListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            stationToList = (StationToList)StationListView.SelectedItem;
            if (stationToList != null)
            {
                UpdatingWindow(stationToList.Id);
            }
        }

        private void Drones_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idDrone = ((DroneInCargeing)Drones.SelectedItem).Id;
            new DroneWindow(blGui, "ByStation", idDrone).Show();
            Close();
        }
        CollectionView myView;
        private void AddGrouping()
        {
            string choise = "freeChargeSlots";
            myView = (CollectionView)CollectionViewSource.GetDefaultView(StationListView.ItemsSource);
            if (myView.CanGroup == true)
            {
                PropertyGroupDescription groupDescription = new(choise);
                myView.GroupDescriptions.Clear();
                myView.GroupDescriptions.Add(groupDescription);
            }
            else
            {
                return;
            }
        }
        private void onlyNumbersForID(object sender, TextCompositionEventArgs e)
        {

            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }
        private void onlytwoNumbers(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{1,2}$");
            e.Handled = !regex.IsMatch(temp);
        }
        private void lungetudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[2-3]{1,2}[.]{0,1}$");
            Regex regexB = new("^[2-3]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp)|| regexB.IsMatch(temp));
        } private void lattitudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[3-4]{1,2}[.]{0,1}$");
            Regex regexB = new("^[3-4]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }
    }
}
