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
        public ParcelWindow(IBL bL, string _action = "")
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
                //case "Updating":
                //    BorderEnterNumber.Visibility = Visibility.Visible;
                //    update.Visibility = Visibility.Hidden;
                //    addButton.Content = "הצג";
                //    Close.Content = "סגור";
                //    actions = Actions.UPDATING;
                //    break;
                //case "Add":
                //    AddWindow();
                //    break;

                default:
                    break;
            }
        }
        private void ListWindow()
        {

            ParcelListView.Items.Refresh();
            actions = Actions.LIST;
            addButton.Content = "הוסף תחנה";
            Close.Content = "סגור";
            List.Visibility = Visibility.Visible;
            //Updating.Visibility = Visibility.Hidden;
            //Add.Visibility = Visibility.Hidden;
            ParcelListView.ItemsSource = parcelToListView;
            weightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            prioritiSelector.ItemsSource = Enum.GetValues(typeof(Priorities));
            StatusSelector.ItemsSource = Enum.GetValues(typeof(parcelStatus));

            weightSelector.SelectedIndex = 0;
            prioritiSelector.SelectedIndex = 0;
            StatusSelector.SelectedIndex = 0;
            //droneToListsView.CollectionChanged += DroneToListsView_CollectionChanged;
            SearchBy();
        }

        public void InitList()//
        {
            IEnumerable<ParcelToList> temp = blGui.DisplaysIistOfparcels();
            parcelToListGroping = (from parcel in temp
                                   group parcel by new parcelStatus_WeightCategories_Priorities { parcelStatus = parcel.parcelStatus, priority = parcel.priority, weight = parcel.weight })
                                   .ToDictionary(X => X.Key, X => X.ToList());

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

            ParcelListView.ItemsSource = parcelToListGroping.Values.SelectMany(x => x);

            ParcelListView.ItemsSource = parcelToListGroping.Where(x => x.Key.parcelStatus == BO.parcelStatus.associated)  ;
        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
