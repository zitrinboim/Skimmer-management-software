﻿using IBL.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDrone.xaml
    /// </summary>

    public partial class AddDroneWindow : Window
    {

        IBL.BO.BL BLGui;
        private DroneToList droneToList;
        private DroneListWindow droneListWindow;
        private Drone drone;
        private int idStation = new();

        public AddDroneWindow(BL bL, DroneListWindow _droneListWindow)
        {
            InitializeComponent();
            droneListWindow = _droneListWindow;
            BLGui = bL;
            drone = new();
            DataContext = drone;

            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            WeightSelector.SelectedIndex = -1;

            stations.ItemsSource = BLGui.DisplaysIistOfStations();
        }

        public AddDroneWindow(DroneToList droneToList, BL bL, DroneListWindow _droneListWindow, int index)
        {
            InitializeComponent();
            droneListWindow = _droneListWindow;
            this.droneToList = droneToList;
            BLGui = bL;
            // BLProgram.updateModelOfDrone(newModel, IdDrone)

        }

        private void addButton_Click(object sender, RoutedEventArgs e)
        {
            if (drone.Id != default && drone.Model != default && drone.MaxWeight != default && idStation != default)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);
                switch (messageBoxResult)
                {

                    case MessageBoxResult.OK:
                        BLGui.addDrone(drone, idStation);
                        droneListWindow.droneToListsView.Add(BLGui.DisplaysIistOfDrons().First(i => i.Id == drone.Id));
                        MessageBox.Show("הרחפן נוצר בהצלחה\n מיד תוחזר לרשימת הרחפנים", "אישור");
                        Close();
                        break;
                    case MessageBoxResult.Cancel:
                        break;
                    default:
                        break;
                }
            }
            else
                MessageBox.Show("נא השלם את השדות החסרים", "אישור");

        }

        private void stations_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            StationToList station = (StationToList)stations.SelectedItem;
            idStation = station.Id;
        }
    }
}
