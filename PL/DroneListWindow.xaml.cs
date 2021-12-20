//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using BO;
//using BlApi;

//namespace PL
//{
//    //    public enum WeightCategories { All, easy, medium, heavy };
//    //  public enum DroneStatuses { All, available, maintenance, busy };
//    /// <summary>
//    /// Interaction logic for DroneListWindow.xaml
//    /// </summary>
//    public partial class DroneListWindow : Window
//    {
//        //private IBL blGui;
//        //ObservableCollection<DroneToList> droneToListsView;
//        public DroneListWindow(IBL bL, ObservableCollection<DroneToList> _droneToListsView)
//        {
//            blGui = bL;
//            droneToListsView = _droneToListsView;
//            InitializeComponent();

//            DroneListView.ItemsSource = droneToListsView;
//            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatuses));
//            WeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
//            StatusSelector.SelectedIndex = 0;
//            droneToListsView.CollectionChanged += DroneToListsView_CollectionChanged;
//        }

//        private void DroneToListsView_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
//        {
//            StatusSelectorAndWeightSelector();
//        }



//        private void StatusSelectorAndWeightSelector()
//        {
//            if (WeightSelector.SelectedIndex == -1)
//            {
//                WeightSelector.SelectedIndex = 0;
//            }
//            WeightCategories weightCategories = (WeightCategories)WeightSelector.SelectedItem;
//            DroneStatuses droneStatuses = (DroneStatuses)StatusSelector.SelectedItem;

//            if (weightCategories == WeightCategories.All && droneStatuses == DroneStatuses.All)
//                DroneListView.ItemsSource = droneToListsView;

//            else if (weightCategories != WeightCategories.All && droneStatuses == DroneStatuses.All)
//                DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.MaxWeight == (BO.WeightCategories)weightCategories);

//            else if (weightCategories == WeightCategories.All && droneStatuses != DroneStatuses.All)
//                DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.DroneStatuses == (BO.DroneStatuses)droneStatuses);

//            else
//                DroneListView.ItemsSource = droneToListsView.ToList().FindAll(i => i.MaxWeight == (BO.WeightCategories)weightCategories && i.DroneStatuses == (BO.DroneStatuses)droneStatuses);
//        }

//        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            StatusSelectorAndWeightSelector();
//        }

//        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            StatusSelectorAndWeightSelector();
//        }

//        private void Button_Click(object sender, RoutedEventArgs e)
//        {
//            new AddDroneWindow(blGui, this).Show();
//        }

//        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            DroneToList droneToList = (DroneToList)DroneListView.SelectedItem;
//            int index = DroneListView.SelectedIndex;
//            if (droneToList != null)
//            {
//                new AddDroneWindow(droneToList, blGui, this, index).Show();
//            }
//        }
//    }
//}
