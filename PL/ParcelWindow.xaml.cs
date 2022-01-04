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
    public struct parcelStatus_WeightCategories_Priorities
    {
        public BO.parcelStatus parcelStatus { get; set; }
        public BO.WeightCategories weight { get; set; }
        public BO.Priorities priority { get; set; }

    }
    /// <summary>
    /// Interaction logic for PercelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        Dictionary<parcelStatus_WeightCategories_Priorities, List<ParcelToList>> parcelToListGroping;
        private IBL blGui;
        ObservableCollection<ParcelToList> parcelToListView;
        private Parcel parcel;
        private ParcelToList parcelToList;
        Actions actions;
        string action;
        public ParcelWindow(IBL bL, string _action = "", int id = 0)
        {
            parcelToListGroping = new();

            blGui = bL;
            actions = new();
            action = _action;
            parcelToListView = new();
            parcelToList = new();
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
                case "ByCustomer":
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
            ParcelListView.Items.Refresh();
            actions = Actions.LIST;
            addButton.Content = "הוסף חבילה";
            Close.Content = "סגור";
            List.Visibility = Visibility.Visible;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Hidden;

            //ParcelListView.ItemsSource = parcelToListView;
            weightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            prioritiSelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(parcelStatus));

            //weightSelector.SelectedIndex = 0;
            //prioritiSelector.SelectedIndex = 0;
            //StatusSelector.SelectedIndex = 0;

            //droneToListsView.CollectionChanged += DroneToListsView_CollectionChanged;
            SearchBy();
        }

        private void AddWindow()
        {
            actions = Actions.ADD;
            addButton.Content = "הוסף";
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Visible;
            prioritiCombo.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            weightCombo.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            parcel = new() { Id = 0, Requested = DateTime.Now, Target=new(), Sender=new() };
            DataContext = parcel;
        }
        private void UpdatingWindow(int id)
        {
            //if (action == "Updating")
            //{
            //    station = blGui.GetStation(id);
            //}
            actions = Actions.UPDATING;
            addButton.Visibility= Visibility.Hidden;
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Hidden;
            parcel = blGui.GetParcel(id);
            DataContext = parcel;
            switch (parcelToList.parcelStatus)
            {
                case BO.parcelStatus.defined:
                    droneInParcelButton.IsEnabled = false;
                    ScheduledTextBox.Text = "לא שוייך";
                    PickedUpTextBox.Text = "לא נאסף";
                    DeliveredTextBox.Text = "לא סופק";
                    break;
                case BO.parcelStatus.associated:
                    PickedUpTextBox.Text = "לא נאסף";
                    DeliveredTextBox.Text = "לא סופק";
                    break;
                case BO.parcelStatus.collected:
                    DeliveredTextBox.Text = "לא סופק";
                    break;
                default:
                    break;
            }
        }

        public void InitList()//
        {
            List<ParcelToList> temp = blGui.DisplaysIistOfparcels().ToList();
            foreach (ParcelToList item in temp)
            {
                parcelToListView.Add(item);
            }
            //parcelToListGroping = (from parcel in temp
            //                       group parcel by new parcelStatus_WeightCategories_Priorities
            //                       { parcelStatus = parcel.parcelStatus, priority = parcel.priority, weight = parcel.weight })
            //                       .ToDictionary(X => X.Key, X => X.ToList());

        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBy();
        }

        private void prioritiSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBy();
        }

        private void weightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SearchBy();
        }
        private void SearchBy()
        {
            //WeightCategories weight = (WeightCategories)weightSelector.SelectedItem;
            //parcelStatus parcelStatus = (parcelStatus)StatusSelector.SelectedItem;
            //Priorities priorities = (Priorities)prioritiSelector.SelectedItem;

            //if (weight == WeightCategories.All && parcelStatus == parcelStatus.הכל && priorities == Priorities.הכל)
            if (weightSelector.SelectedIndex == -1 && prioritiSelector.SelectedIndex == -1 && StatusSelector.SelectedIndex == -1)
               // ParcelListView.ItemsSource = parcelToListGroping.Values.SelectMany(x => x);
                ParcelListView.ItemsSource = parcelToListView;

            // ParcelListView.ItemsSource = parcelToListGroping.Where(x => x.Key.parcelStatus == BO.parcelStatus.associated);
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            switch (actions)
            {
                case Actions.LIST:
                    AddWindow();
                    break;
                case Actions.ADD:

                    if (parcel.Sender.Id != default && parcel.Target.Id != default && parcel.weight != default && parcel.priority != default)//להעביר את הבדיקה לאיז אנעבלעד 
                    {
                        MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);//לשפר סטייל של ההודעה
                        switch (messageBoxResult)
                        {

                            case MessageBoxResult.OK:
                                int idParcel = blGui.addParsel(parcel);
                                parcelToListView.Add(blGui.DisplaysIistOfparcels().First(i => i.Id == parcel.Id));
                                MessageBox.Show("החבילה נוצרה בהצלחה\n מספר החבילה הוא:" + idParcel.ToString() + "\n מיד תוצג רשימת החבילות", "אישור");
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
                        parcelToList = parcelToListView.ToList().Find(i => i.Id == int.Parse(TxtBx_ID.Text.ToString()));
                        if (parcelToList.Id != 0)
                        {

                            BorderEnterNumber.Visibility = Visibility.Hidden;
                            update.Visibility = Visibility.Visible;
                            UpdatingWindow(parcelToList.Id);

                        }
                        else
                        {
                            MessageBox.Show("החבילה המבוקשת לא נמצאה", "אישור");
                            Close();
                        }
                    }

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
                        ParcelListView.SelectedItem = null;
                    }
                    else
                        Close();
                    break;
                case "ByCustomer":
                    new CustomerWindow(blGui, "List").Show();
                    Close();
                    break;

                default:
                    Close();
                    break;
            }
        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            parcelToList = (ParcelToList)ParcelListView.SelectedItem;
            if (parcelToList != null)
            {
                 UpdatingWindow(parcelToList.Id);
            }
        }

        private void targetButton_Click(object sender, RoutedEventArgs e)
        {
            //פתיחה של חלון לקוח עם פרטי לקוח מקבל
        }

        private void sanderButton_Click(object sender, RoutedEventArgs e)
        {
            //פתיחה של חלון לקוח עם פרטי לקוח שולח

        }

        private void droneInParcelButton_Click(object sender, RoutedEventArgs e)
        {
            int idDrone = ((DroneInParcel)parcel.droneInParcel).Id;
            new DroneWindow(blGui, "ByParcel", idDrone).Show();
            Close();
        }
    }
}
