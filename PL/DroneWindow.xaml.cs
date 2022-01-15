using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BO;
using BlApi;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Text.RegularExpressions;
using System.Windows.Data;
using System.ComponentModel;
using System.Threading;

namespace PL
{


    /// <summary>
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        private IBL blGui;
        ObservableCollection<DroneToList> droneToListsView;
        private Drone drone;
        private DroneToList droneToList;
        private int idStation;
        int droneId;
        Actions actions;
        string action;
        Station station;

        public DroneWindow(IBL bL, string _action = "", int id = 0)
        {
            blGui = bL;
            actions = new();
            action = _action;
            droneToListsView = new();
            droneToList = new();
            drone = new();
            DataContext = drone;
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
                    List<DroneToList> drones = blGui.DisplaysIistOfDrons().ToList();
                    var droneCombo = from item in drones
                                     select item.Id;
                    comboID.ItemsSource = droneCombo;
                    break;
                case "Add":
                    AddWindow();
                    break;
                case "ByStation" :
                    if (id != 0)
                    {
                        UpdatingWindow(id);
                    }
                    break;
                case "ByDrone":
                    if (id != 0)
                    {
                        UpdatingWindow(id);
                    }
                    break;
                case "ByParcel":
                    if (id != 0)
                    {
                        UpdatingWindow(id);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ListWindow()
        {
            DroneListView.Items.Refresh();
            actions = Actions.LIST;
            addButton.Content = "הוסף רחפן";
            Close.Content = "סגור";
            List.Visibility = Visibility.Visible;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Hidden;
            DroneListView.ItemsSource = droneToListsView;
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            StatusSelector.SelectedIndex = 0;
            droneToListsView.CollectionChanged += DroneToListsView_CollectionChanged;
        }

        private void AddWindow()
        {
            actions = Actions.ADD;
            addButton.Content = "הוסף";
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Visible;
            MaxWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            WeightSelector.SelectedIndex = -1;
            stations.ItemsSource = blGui.DisplaysIistOfStations(i => i.freeChargeSlots > 0);
        }

        private void UpdatingWindow(int id)
        {

            try
            {
                 if (action == "ByStation" || action == "ByParcel"|| action == "Updating" || action == " ByDrone")
                {
                    drone = blGui.GetDrone(id);
                    droneToList = blGui.DisplaysIistOfDrons().First(i => i.Id == id);
                }

                if (droneToList.DroneStatuses == BO.DroneStatuses.maintenance)
                {
                    BorderStation.Visibility = Visibility.Visible;
                    sandToStation.Visibility = Visibility.Hidden;
                    parcelToDrone.Visibility = Visibility.Hidden;
                    ActionInParcel.Visibility = Visibility.Hidden;
                    droneMaintenance.Visibility = Visibility.Visible;
                    packageAssociated.Text = "הרחפן בתחזוקה";
                    drone = blGui.GetDrone(droneToList.Id);
                    station = blGui.GetStation(blGui.GetTheIdOfCloseStation(drone.Id));
                    stationIdltextBlock.Text = station.Id.ToString();
                    stationLoctionltextBlock.Text = station.location.ToString();

                }
                if (droneToList.DroneStatuses == BO.DroneStatuses.available)
                {
                    BorderStation.Visibility = Visibility.Hidden;
                    sandToStation.Visibility = Visibility.Visible;
                    parcelToDrone.Visibility = Visibility.Visible;
                    droneMaintenance.Visibility = Visibility.Hidden;
                    ActionInParcel.Visibility = Visibility.Hidden;
                    packageAssociated.Text = "אין חבילה משוייכת לרחפן זה כרגע";
                }
                if (droneToList.DroneStatuses == BO.DroneStatuses.busy)
                {
                    BorderStation.Visibility = Visibility.Hidden;
                    sandToStation.Visibility = Visibility.Hidden;
                    ActionInParcel.Visibility = Visibility.Visible;
                    if (drone.packageInTransfer.packageInTransferStatus == PackageInTransferStatus.awaitingCollection)
                        ActionParcelButton.Content = "איסוף חבילה";
                    else
                        ActionParcelButton.Content = "אספקת חבילה";
                    NoParcel.Visibility = Visibility.Hidden;
                    YesParcel.Visibility = Visibility.Visible;
                }
                else
                {
                    NoParcel.Visibility = Visibility.Visible;
                    YesParcel.Visibility = Visibility.Hidden;
                    ActionInParcel.Visibility = Visibility.Hidden;

                }
                actions = Actions.UPDATING;
                addButton.Content = "עדכן";
                Close.Content = "סגור";
                List.Visibility = Visibility.Hidden;
                Updating.Visibility = Visibility.Visible;
                Add.Visibility = Visibility.Hidden;
                drone = blGui.GetDrone(id);

                DataContext = drone;
            }
            catch (invalidValueForChargeSlots ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        public void InitList()
        {
            List<DroneToList> temp = blGui.DisplaysIistOfDrons().ToList();
            foreach (DroneToList item in temp)
                droneToListsView.Add(item);
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
            AddGrouping();
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelectorAndWeightSelector();
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StatusSelectorAndWeightSelector();
        }


        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                droneToList = (DroneToList)DroneListView.SelectedItem;
                if (droneToList != null)
                {
                    new DroneWindow(blGui, "ByDrone", droneToList.Id).Show();
                }
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }
        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch (actions)
                {
                    case Actions.LIST:
                        AddWindow();
                        break;
                    case Actions.ADD:

                        if (drone.Id != default && drone.Model != default && drone.MaxWeight != default && idStation != default) 
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);
                            switch (messageBoxResult)
                            {

                                case MessageBoxResult.OK:
                                    try
                                    {
                                        _ = blGui.addDrone(drone, idStation);
                                        droneToListsView.Add(blGui.DisplaysIistOfDrons().First(i => i.Id == drone.Id));
                                        MessageBox.Show("הרחפן נוצר בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                                        ListWindow();
                                    }
                                    catch (BO.IdExistExeptions ex)
                                    {
                                        MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                                            MessageBoxResult.None, MessageBoxOptions.RightAlign);
                                        Close();
                                    }
                                    catch (BO.IdNotExistExeptions ex)
                                    {
                                        MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                                            MessageBoxResult.None, MessageBoxOptions.RightAlign);
                                        Close();
                                    }
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
                            BorderEnterNumber.Visibility = Visibility.Hidden;
                            update.Visibility = Visibility.Visible;
                            UpdatingWindow(drone.Id);
                            break;
                        }
                        if (drone.Model != default)
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר עדכון זה", "אישור", MessageBoxButton.OKCancel);
                            switch (messageBoxResult)
                            {
                                case MessageBoxResult.OK:
                                    if (relase.IsChecked == true)
                                    {
                                        relase.IsChecked = false;
                                        blGui.ReleaseDroneFromCharging(drone.Id, 7);
                                        BorderStation.Visibility = Visibility.Hidden;
                                    }
                                    droneToList.Model = drone.Model;
                                    _ = blGui.updateModelOfDrone(droneToList.Model, droneToList.Id);
                                    MessageBox.Show("העדכון בוצע בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                                    DroneListView.SelectedItem = null;
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
            catch (BO.IdExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            } 
            catch (BO.ChargingExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }
        private void stations_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            StationToList station = (StationToList)stations.SelectedItem;
            idStation = station.Id;
        }
        private void parcelToDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לשייך חבילה ", "אישור", MessageBoxButton.OKCancel);
                switch (messageBoxResult)
                {
                    case MessageBoxResult.OK:
                        bool test = blGui.AssignPackageToDrone(drone.Id);
                        if (test)
                        {
                            NoParcel.Visibility = Visibility.Hidden;
                            YesParcel.Visibility = Visibility.Visible;
                            drone = blGui.GetDrone(drone.Id);
                            UpdatingWindow(drone.Id);
                        }
                        else
                        {
                            MessageBox.Show("לא נמצאה חבילה מתאימה", "אישור");
                        }
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    default:
                        break;
                }
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            } 
            catch (BO.IdExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.PackageAssociationExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        /* 
         if(((button)sender)
         */
        private void CloseButton_Click(object sender, RoutedEventArgs e)//***********************
        {
            BackgroundWorker backgroundWorker=new();
            if (backgroundWorker != null && backgroundWorker.IsBusy == true)
            {
                //s = true;
                MessageBox.Show("יש להפסיק את פעולת הסימולטור טרם סגירה");
            }
            //listOfIdDrons.remove(droneId);
            switch (action)
            {
                case "List":
                    if (actions != Actions.LIST)
                    {
                        ListWindow();
                        DroneListView.SelectedItem = null;
                    }
                    else
                        Close();
                    break;
                case "ByStation":
                    new StationWindow(blGui, "List").Show();
                    Close();
                    break;
                case "ByParcel":
                    new ParcelWindow(blGui, "List").Show();
                    Close();
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void sand_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!blGui.SendDroneForCharging(drone.Id))
                {
                    MessageBox.Show("שליחה לטעינה נכשלה", "אישור");
                }
                else
                {
                    UpdatingWindow(drone.Id);
                }
            }
            catch (BO.ChargingExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                                  MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.IdExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }

        private void ActionParcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ActionParcelButton.Content == "איסוף חבילה")
                {

                    if (!blGui.PackageCollectionByDrone(drone.Id))
                    {
                        MessageBox.Show("איסוף חבילה נכשל", "אישור");
                    }
                    else
                    {
                        drone = blGui.GetDrone(drone.Id);
                        UpdatingWindow(drone.Id);
                    }
                }
                else if (ActionParcelButton.Content == "אספקת חבילה")
                {
                    if (!blGui.DeliveryPackageToCustomer(drone.Id))
                    {
                        MessageBox.Show("איסוף חבילה נכשל", "אישור");
                    }
                    else
                    {
                        UpdatingWindow(drone.Id);

                    }
                }
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.IdExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.PackageAssociationExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }
        CollectionView myView;
        private void AddGrouping()
        {
            string choise = "DroneStatuses";
            myView = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
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
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
