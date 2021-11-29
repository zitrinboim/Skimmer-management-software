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
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
       private IBL.BO.BL blGui;

        public DroneListWindow(IBL.BO.BL bL)
        {
            blGui = bL;
            InitializeComponent();
            DroneListView.ItemsSource = blGui.DisplaysIistOfDrons();
            StatusSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.DroneStatuses));
            WeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = blGui.DisplaysIistOfDrons(i =>i.DroneStatuses == (IBL.BO.DroneStatuses)StatusSelector.SelectedItem);
        }

        private void WeightSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListView.ItemsSource = blGui.DisplaysIistOfDrons(i => i.MaxWeight == (IBL.BO.WeightCategories)WeightSelector.SelectedItem);
        }
    }
}
