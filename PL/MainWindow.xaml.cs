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
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace PL
{
    public enum SlotsSTatus { הכל, פנוי, מלא };
    public enum WeightCategories { All, easy, medium, heavy };
    public enum Actions { LIST, ADD, UPDATING, REMOVE };
    public enum DroneStatuses { All, available, maintenance, busy };
    public enum Priorities { הכל, רגיל, מהיר, דחוף };
    public enum parcelStatus { הכל, הוגדר, שוייך, נאסף, סופק };

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL blGui;
        Customer customer;

        public MainWindow()
        {
            blGui = BlFactory.GetBL();
            InitializeComponent();
            customer = new();
            customer.location = new();
            DataContext = customer;
        }

        private void Button_Click_CloseWindow(object sender, RoutedEventArgs e)
        {

            Application.Current.Shutdown();//לבדוק שזה מסתדר עם הסימולטור שלא ייסגר באמצע פעולה.
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
                OfficeMainWindowBorder.Visibility = Visibility.Visible;
        }

        private void Button_Click_AddStation(object sender, RoutedEventArgs e)
        {
            new StationWindow(blGui, "Add").Show();
        }

        private void Button_Click_UpdateStationData(object sender, RoutedEventArgs e)
        {
            new StationWindow(blGui, "Updating").Show();
        }

        private void Button_Click_ListOfStation(object sender, RoutedEventArgs e)
        {
            new StationWindow(blGui, "List").Show();
        }

        private void Button_Click_AddDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blGui, "Add").Show();
        }

        private void Button_Click_actionInDrone(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blGui, "Updating").Show();
        }

        private void Button_Click_ListOfDrones(object sender, RoutedEventArgs e)
        {
            new DroneWindow(blGui, "List").Show();
        }

        private void Button_Click_AddNewCostumer(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blGui, "Add").Show();
        }

        private void Button_Click_UpdateCostumerData(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blGui, "Updating").Show();
        }

        private void Button_Click_ListOfCostumers(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(blGui, "List").Show();
        }

        private void Button_Click_AddParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blGui, "Add").Show();
        }

        private void Button_Click_actionUnParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blGui, "Updating").Show();
        }

        private void Button_Click_RemoveParcel(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blGui, "Remove").Show();
        }

        private void Button_Click_ListOfParcels(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(blGui, "List").Show();
        }
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }

        private void ButtonEntrance_Click(object sender, RoutedEventArgs e)
        {
            if (blGui.Password() == int.Parse(TextBoxPassword.Text.ToString()))
            {
                GridEntrance.Visibility = Visibility.Hidden;
                GridSystemAdministrator.Visibility = Visibility.Visible;
            }
            else
                MessageBox.Show("הסיסמא אינה נכונה\n אנא נסה שוב", "אישור");
        }
        private void onlyNumbers(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }

        private void ButtonID_Custumer_Click(object sender, RoutedEventArgs e)
        {
            List<CustomerToList> customers = blGui.DisplaysIistOfCustomers().ToList();
            var customerCombo = from item in customers
                                select item.Id;
           int find = customerCombo.FirstOrDefault(i => i == int.Parse(TextBox_ID_Custumer.Text.ToString()));
            if(find != default)
            {
                int ID_Customer = int.Parse(find.ToString());
                new CustomerLoginWindow(blGui, "ByCustomer", ID_Customer).Show();
            }
            else
                MessageBox.Show("מספר המשתמש אינו קיים\n אנא נסה שוב", "אישור");
        }
        private void onlyNumbersForID(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }
        private void phonePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0][0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }
        private void onlyAlphaBeta(object sender, TextCompositionEventArgs e)
        {

            Regex regex = new("[^א-ת]$");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void lungetudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[2-3]{1,2}[.]{0,1}$");
            Regex regexB = new("^[2-3]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }
        private void lattitudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[3-4]{1,2}[.]{0,1}$");
            Regex regexB = new("^[3-4]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }

        private void ButtonNewClient_Click(object sender, RoutedEventArgs e)
        {
           
            if (customer.Id != default && customer.name != default && customer.location.longitude != default && customer.location.latitude != default && customer.phone != default)
            {
                MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);//לשפר סטייל של ההודעה
                switch (messageBoxResult)
                {

                    case MessageBoxResult.OK:
                        _ = blGui.addCustomer(customer);
                        MessageBox.Show("הלקוח נוצר  בהצלחה\n מיד יוצגו פרטי לקוח", "אישור");
                        new CustomerLoginWindow(blGui, "ByCustomer", customer.Id).Show();
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
    }
}