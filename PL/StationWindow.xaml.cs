﻿using System;
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
        /// <summary>
        /// c-tor.
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_action"></param>
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
                    List<StationToList> stations = blGui.DisplaysIistOfStations().ToList();
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
        /// <summary>
        /// This function determines the display of the window according to the position of the selected view status.
        /// </summary>
        private void AddWindow()
        {
            actions = Actions.ADD;
            addButton.Content = "הוסף";
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// This function determines the display of the window according to the position of the selected view status.
        /// </summary>
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
        }
        /// <summary>
        /// This function determines the display of the window according to the position of the selected view status.
        /// </summary>
        /// <param name="id"></param>
        private void UpdatingWindow(int id)
        {
            actions = Actions.UPDATING;
            addButton.Content = "עדכן";
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Hidden;
            try
            {
                station = blGui.GetStation(id);
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                return;
            }
            Drones.ItemsSource = station.droneInCargeings.ToList();
            DataContext = station;
        }

        public void InitList()
        {
            List<StationToList> temp = blGui.DisplaysIistOfStations().ToList();
            foreach (StationToList item in temp)
                StationsToListView.Add(item);
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
        /// <summary>
        /// This function defines the actions behind the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void addButton_Click(object sender, RoutedEventArgs e)
        {

            switch (actions)
            {
                case Actions.LIST:
                    AddWindow();
                    break;
                case Actions.ADD:

                    if (station.Id != default && station.name != default && station.location.longitude != default &&
                        station.location.latitude != default && station.freeChargeSlots != default)
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);
                        switch (messageBoxResult)
                        {

                            case MessageBoxResult.OK:
                                try
                                {
                                    _ = blGui.addStation(station);
                                }
                                catch (BO.IdExistExeptions ex)
                                {
                                    MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                                        MessageBoxResult.None, MessageBoxOptions.RightAlign);
                                    Close();
                                    break;
                                }
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
                                try
                                {
                                    _ = blGui.updateStationData(station.Id, station.name, station.freeChargeSlots);
                                }
                                catch (invalidValueForChargeSlots ex)
                                {
                                    MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                                        MessageBoxResult.None, MessageBoxOptions.RightAlign);
                                    Close();
                                    break;
                                }
                                catch (BO.IdNotExistExeptions ex)
                                {
                                    MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                                        MessageBoxResult.None, MessageBoxOptions.RightAlign);
                                    Close();
                                    break;
                                }
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
        /// <summary>
        /// This function defines the actions behind the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// grouping functaion by freeChargeSlots.
        /// </summary>
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
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onlyNumbersForID(object sender, TextCompositionEventArgs e)
        {

            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onlytwoNumbers(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{1,2}$");
            e.Handled = !regex.IsMatch(temp);
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lungetudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[2-3]{1,2}[.]{0,1}$");
            Regex regexB = new("^[2-3]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lattitudePattren(object sender, TextCompositionEventArgs e)
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
