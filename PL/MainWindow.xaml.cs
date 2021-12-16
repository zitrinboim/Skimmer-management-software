using System;
using BlApi;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using BO;
using BL;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL blGui;
       
        public MainWindow()
        {
            InitializeComponent();
            blGui = BlFactory.GetBL();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(blGui).Show();
        }
    }
}
