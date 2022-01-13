using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using BO;
using BlApi;
using System.Collections.ObjectModel;
using System.Windows.Input;

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
        List<ParcelToList> parcels;
        Actions actions;
        string action;
        public ParcelWindow(IBL bL, string _action = "", int id = 0)
        {
            blGui = bL;
            actions = new();
            action = _action;
            parcelToListView = new();
            parcelToList = new();
            parcel = new() { Id = 0, Requested = DateTime.Now, Target = new(), Sender = new() };
            DataContext = parcel;
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
                    parcels = blGui.DisplaysIistOfparcels().ToList();
                    var parcelCombo = from item in parcels
                                      select item.Id;
                    comboID.ItemsSource = parcelCombo;
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
            parcels = blGui.DisplaysIistOfparcels(i => i.parcelStatus == BO.parcelStatus.defined).ToList();
            var parcelCombo = from item in parcels
                              select item.Id;
            comboID.ItemsSource = parcelCombo;
            if (parcels.Count == 0)
            {
                parcelLab.Content = "אין חבילות למחיקה";
                addButton.IsEnabled = false;
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

            weightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            prioritiSelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(parcelStatus));

            weightSelector.SelectedIndex = 0;
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
            List<CustomerToList> customerTos = blGui.DisplaysIistOfCustomers().ToList();
            var customers = from item in customerTos
                            select item.Id;
            comboBoxOfsander.ItemsSource = customerTos;
            comboBoxOftarget.ItemsSource = customerTos;
        }

        private void UpdatingWindow(int id)
        {
            try
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
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
        }

        public void InitList()
        {
            List<ParcelToList> temp = blGui.DisplaysIistOfparcels().ToList();
            foreach (ParcelToList item in temp)
                parcelToListView.Add(item);
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
            try
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
                            parcelToList = parcelToListView.ToList().Find(i => i.Id == parcel.Id);
                            BorderEnterNumber.Visibility = Visibility.Hidden;
                            update.Visibility = Visibility.Visible;
                            UpdatingWindow(parcelToList.Id);
                        }

                        break;
                    case Actions.REMOVE:
                        MessageBoxResult messageBoxResolt = MessageBox.Show("האם ברצונך לאשר מחיקה זו", "אישור", MessageBoxButton.OKCancel);//לשפר סטייל של ההודעה
                        switch (messageBoxResolt)
                        {
                            case MessageBoxResult.OK:
                                if (blGui.remuveParcel(parcel.Id))
                                    MessageBox.Show("הפעולה בוצעה בהצלחה" + "\n מיד תוצג רשימת החבילות", "אישור");
                                else
                                    MessageBox.Show("החבילה לא ניתנת למחיקה" + "\n מיד תוצג רשימת החבילות", "אישור");
                                new ParcelWindow(blGui, "List").Show();
                                Close();
                                break;
                            case MessageBoxResult.Cancel:
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                MessageBoxResult.None, MessageBoxOptions.RightAlign);
            }
            catch (BO.IdExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                MessageBoxResult.None, MessageBoxOptions.RightAlign);
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
            int idDrone = (parcel.droneInParcel).Id;
            new DroneWindow(blGui, "ByParcel", idDrone).Show();
            Close();
        }

        CollectionView myView;
        private void AddGrouping()
        {
            string choise = "sanderName";
            myView = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            if (myView.CanGroup == true)
            {
                PropertyGroupDescription groupDescription = new(choise);
                myView.GroupDescriptions.Clear();
                myView.GroupDescriptions.Add(groupDescription);
            }
            else
                return;
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
