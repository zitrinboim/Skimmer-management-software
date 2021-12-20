using System;
using BlApi;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using BO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;


namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL blGui;
      //  public ObservableCollection<DroneToList> droneToListsView;//
      //  public ObservableCollection<StationToList> stationToListsView;//


        public MainWindow()
        {
            
        //    stationToListsView = new();
            blGui = BlFactory.GetBL();

            InitializeComponent();
        }

       
        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Button_Click_MinimizeWindow(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
        private void Button_Click_MaximizeWindow(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
                SystemCommands.RestoreWindow(this);
            else
                SystemCommands.MaximizeWindow(this);
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tabControl = sender as TabControl;

            if (tabControl.SelectedIndex == 1)
            {
                OfficeMainWindowBorder.Visibility = Visibility.Visible;
                // OfficeStatistics.Visibility = Visibility.Visible;

                //testersCollection = new ObservableCollection<Tester>(bl.GetTestersList());
                //traineeCollection = new ObservableCollection<Trainee>(bl.GetTraineeList());
                //testCollection = new ObservableCollection<Test>(bl.GetTestsList());
                //ListView_Testers.ItemsSource = testersCollection;
            }
        }

        private void Button_Click_AddStation(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_UpdateStationData(object sender, RoutedEventArgs e)
        {
            new GetByIdWindow(blGui, "Station").Show();

        }

        private void Button_Click_RemoveStation(object sender, RoutedEventArgs e)
        {
            new GetByIdWindow(blGui, "Station").Show();

        }

        private void Button_Click_ListOfStation(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_AddDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blGui, "Add").Show();

        }

        private void Button_Click_actionInDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blGui, "Updating").Show();
        }

        private void Button_Click_RemoveDrone(object sender, RoutedEventArgs e)
        {
            //new GetByIdWindow(blGui, "Drone").Show();
        }

        private void Button_Click_ListOfDrones(object sender, RoutedEventArgs e)
        {
            //new DroneListWindow(blGui, droneToListsView).Show();
            new DroneWindow(blGui, "List").Show();
        }

        private void Button_Click_AddNewCostumer(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_UpdateCostumerData(object sender, RoutedEventArgs e)
        {
            new GetByIdWindow(blGui, "Customer").Show();

        }

        private void Button_Click_RemoveCostumer(object sender, RoutedEventArgs e)
        {
            new GetByIdWindow(blGui, "Customer").Show();

        }

        private void Button_Click_ListOfCostumers(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_AddParcel(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_actionUnParcel(object sender, RoutedEventArgs e)
        {
            new GetByIdWindow(blGui, "Parcel").Show();

        }

        private void Button_Click_RemoveParcel(object sender, RoutedEventArgs e)
        {
            new GetByIdWindow(blGui, "Parcel").Show();

        }

        private void Button_Click_ListOfParcels(object sender, RoutedEventArgs e)
        {

        }
    }
}