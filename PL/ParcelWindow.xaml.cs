using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
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
        //Dictionary<parcelStatus_WeightCategories_Priorities, List<ParcelToList>> parcelToListGroping;
        private IBL blGui;
        ObservableCollection<ParcelToList> parcelToListView;
        private Parcel parcel;
        private ParcelToList parcelToList;
        Actions actions;
        string action;
        public ParcelWindow(IBL bL, string _action = "", int id = 0)
        {
            //parcelToListGroping = new();

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
                    List.Visibility = Visibility.Hidden;
                    Updating.Visibility = Visibility.Visible;
                    Add.Visibility = Visibility.Hidden;
                    addButton.Content = "הצג";
                    Close.Content = "סגור";
                    actions = Actions.UPDATING;
                    break;
                case "Add":
                    AddWindow();
                    break;
                case "Remove":
                    RemoveWindow();
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

        private void RemoveWindow()
        {
            BorderEnterNumber.Visibility = Visibility.Visible;
            update.Visibility = Visibility.Hidden;
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Visible;
            Add.Visibility = Visibility.Hidden;
            addButton.Content = "מחק";
            Close.Content = "סגור";
            actions = Actions.REMOVE;
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

            weightSelector.SelectedIndex = 0;
            // prioritiSelector.SelectedIndex = 0;
            // StatusSelector.SelectedIndex = 0;

            parcelToListView.CollectionChanged += ParcelToListView_CollectionChanged;
            AddGrouping();
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
            parcel = new() { Id = 0, Requested = DateTime.Now, Target = new(), Sender = new() };
            DataContext = parcel;
        }
        private void UpdatingWindow(int id)
        {
           
            actions = Actions.UPDATING;
            addButton.Visibility = Visibility.Hidden;
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
                    droneInParcelButton.Content = "החבילה לא שויכה לרחפן";
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
        private void ParcelToListView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Selectors();
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selectors();
        }

        private void prioritiSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selectors();
        }

        private void weightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Selectors();
        }
        private void Selectors()
        {
            if (prioritiSelector.SelectedIndex == -1)
            {
                prioritiSelector.SelectedIndex = 0;
            }
            if (StatusSelector.SelectedIndex == -1)
            {
                StatusSelector.SelectedIndex = 0;
            }
            WeightCategories weightCategories = (WeightCategories)weightSelector.SelectedItem;
            parcelStatus parcelStatus = (parcelStatus)StatusSelector.SelectedItem;
            Priorities priorities = (Priorities)prioritiSelector.SelectedItem;

            if ((weightCategories == WeightCategories.All) && (parcelStatus == parcelStatus.הכל) && (priorities == Priorities.הכל))
                ParcelListView.ItemsSource = parcelToListView;


            else if ((weightCategories != WeightCategories.All) && (parcelStatus == parcelStatus.הכל) && (priorities == Priorities.הכל))
                ParcelListView.ItemsSource = parcelToListView.ToList().FindAll(i => i.weight == (BO.WeightCategories)weightCategories);

            else if ((weightCategories == WeightCategories.All) && (parcelStatus != parcelStatus.הכל) && (priorities == Priorities.הכל))
                ParcelListView.ItemsSource = parcelToListView.ToList().FindAll(i => i.parcelStatus == (BO.parcelStatus)parcelStatus);

            else if ((weightCategories == WeightCategories.All) && (parcelStatus == parcelStatus.הכל) && (priorities != Priorities.הכל))
                ParcelListView.ItemsSource = parcelToListView.ToList().FindAll(i => i.priority == (BO.Priorities)priorities);


            else if ((weightCategories != WeightCategories.All) && (parcelStatus != parcelStatus.הכל) && (priorities == Priorities.הכל))
                ParcelListView.ItemsSource = parcelToListView.ToList().FindAll(i => i.weight == (BO.WeightCategories)weightCategories &&
                i.parcelStatus == (BO.parcelStatus)parcelStatus);

            else if ((weightCategories != WeightCategories.All) && (parcelStatus == parcelStatus.הכל) && (priorities != Priorities.הכל))
                ParcelListView.ItemsSource = parcelToListView.ToList().FindAll(i => i.weight == (BO.WeightCategories)weightCategories &&
                i.priority == (BO.Priorities)priorities);

            else if ((weightCategories == WeightCategories.All) && (parcelStatus != parcelStatus.הכל) && (priorities != Priorities.הכל))
                ParcelListView.ItemsSource = parcelToListView.ToList().FindAll(i => i.priority == (BO.Priorities)priorities &&
                i.parcelStatus == (BO.parcelStatus)parcelStatus);

            else
                ParcelListView.ItemsSource = parcelToListView.ToList().FindAll(i => i.priority == (BO.Priorities)priorities &&
                i.parcelStatus == (BO.parcelStatus)parcelStatus && i.weight == (BO.WeightCategories)weightCategories);
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
                                parcelToListView.Add(blGui.DisplaysIistOfparcels().First(i => i.Id == idParcel));
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
                    MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר מחיקה זו", "אישור", MessageBoxButton.OKCancel);//לשפר סטייל של ההודעה
                    switch (messageBoxResult)
                    {

                        case MessageBoxResult.OK:
                            blGui.r
                            parcelToListView.Add(blGui.DisplaysIistOfparcels().First(i => i.Id == idParcel));
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
            new CustomerWindow(blGui, "ByParcel", parcel.Target.Id).Show();
            Close();
        }

        private void sanderButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blGui, "ByParcel", parcel.Sender.Id).Show();
            Close();
        }

        private void droneInParcelButton_Click(object sender, RoutedEventArgs e)
        {
            int idDrone = ((DroneInParcel)parcel.droneInParcel).Id;
            new DroneWindow(blGui, "ByParcel", idDrone).Show();
            Close();
        }


        CollectionView myView;
        private void AddGrouping()
        {
            string choise = "parcelStatus";
            myView = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
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

        private void RemoveGrouping(object sender, RoutedEventArgs e)
        {
            myView = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            myView.GroupDescriptions.Clear();
        }
    }
}
