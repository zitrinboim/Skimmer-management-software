using BlApi;
using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for CustomerLoginWindow.xaml
    /// </summary>
    public partial class CustomerLoginWindow : Window
    {
        private IBL blGui;
        private Customer customer;
        /// <summary>
        /// c-tor.
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_action"></param>
        /// <param name="id"></param>
        public CustomerLoginWindow(IBL bL, string _action = "", int id = 0)
        {
            InitializeComponent();
            blGui = bL;
            customer = new();
            customer.location = new();
            DataContext = customer;
            try
            {
                customer = blGui.GetCustomer(id);
                fromCustomer.ItemsSource = customer.fromCustomer.ToList();
                toCustomer.ItemsSource = customer.toCustomer.ToList();
                DataContext = customer;
            }
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.IdExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }
        /// <summary>
        /// This function defines the actions behind the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void toCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idParcel = ((ParcelInCustomer)toCustomer.SelectedItem).Id;
            new CustomerPackageWindow(blGui, idParcel).Show();
            Close();
        }

        private void fromCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idParcel = ((ParcelInCustomer)fromCustomer.SelectedItem).Id;
            new CustomerPackageWindow(blGui, idParcel).Show();
            Close();
        }
        /// <summary>
        /// This function defines the actions behind the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (customer.name != default && customer.phone != default)
                {
                    MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר עדכון זה", "אישור", MessageBoxButton.OKCancel);
                    switch (messageBoxResult)
                    {
                        case MessageBoxResult.OK:
                            _ = blGui.updateCustomerData(customer.Id, customer.name, customer.phone);
                            MessageBox.Show("העדכון בוצע בהצלחה", "אישור");
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
            catch (BO.IdNotExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
            catch (BO.IdExistExeptions ex)
            {
                MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error,
                    MessageBoxResult.None, MessageBoxOptions.RightAlign);
                Close();
            }
        }
    }
}
