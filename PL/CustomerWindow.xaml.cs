﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BO;
using BlApi;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        private IBL blGui;
        ObservableCollection<CustomerToList> CustomersToListView;
        private Customer customer;
        private CustomerToList customerToList;
        Actions actions;
        string action;
        /// <summary>
        /// c-tor
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="_action"></param>
        /// <param name="id"></param>
        public CustomerWindow(IBL bL, string _action = "", int id = 0)
        {
            blGui = bL;
            actions = new();
            action = _action;
            CustomersToListView = new();
            customerToList = new();
            customer = new();
            customer.location = new();
            DataContext = customer;
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
                    addButton.Content = "הצג";
                    Close.Content = "סגור";
                    actions = Actions.UPDATING;
                    List<CustomerToList> customers   = blGui.DisplaysIistOfCustomers().ToList();
                    var customerCombo = from item in customers
                                     select item.Id;
                    comboID.ItemsSource = customerCombo;
                    break;
                case "Add":
                    AddWindow();
                    break;
                case "ByParcel":
                    if (id != 0)
                        UpdatingWindow(id);
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// This function determines the display of the window according to the position of the selected view status.
        /// </summary>
        private void AddWindow()
        {
            actions = Actions.ADD;
            addButton.Visibility = Visibility.Visible;
            addButton.Content = "הוסף";
            Close.Content = "סגור";
            List.Visibility = Visibility.Hidden;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// This function determines the display of the window according to the position of the selected view status.
        /// </summary>
        private void ListWindow()
        {
            addButton.Visibility = Visibility.Visible;
            CustomerListView.Items.Refresh();
            actions = Actions.LIST;
            addButton.Content = "הוסף לקוח";
            Close.Content = "סגור";
            List.Visibility = Visibility.Visible;
            Updating.Visibility = Visibility.Hidden;
            Add.Visibility = Visibility.Hidden;
            CustomerListView.ItemsSource = CustomersToListView;
        }
        /// <summary>
        /// This function determines the display of the window according to the position of the selected view status.
        /// </summary>
        /// <param name="id"></param>
        private void UpdatingWindow(int id)
        {
            try
            {
                if (action == "ByParcel")
                {
                    customer = blGui.GetCustomer(id);
                }
                actions = Actions.UPDATING;
                addButton.Content = "עדכן";
                Close.Content = "סגור";
                List.Visibility = Visibility.Hidden;
                Updating.Visibility = Visibility.Visible;
                Add.Visibility = Visibility.Hidden;
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

        public void InitList()
        {
            List<CustomerToList> temp = blGui.DisplaysIistOfCustomers().ToList();
            foreach (CustomerToList item in temp)
            {
                CustomersToListView.Add(item);
            }
        }
        /// <summary>
        /// This function defines the actions behind the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

                        if (customer.Id != default && customer.name != default && customer.location.longitude != default && customer.location.latitude != default && customer.phone != default) 
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר הוספה זו", "אישור", MessageBoxButton.OKCancel);//לשפר סטייל של ההודעה
                            switch (messageBoxResult)
                            {

                                case MessageBoxResult.OK:
                                    _ = blGui.addCustomer(customer);
                                    CustomersToListView.Add(blGui.DisplaysIistOfCustomers().First(i => i.Id == customer.Id));
                                    MessageBox.Show("הלקוח נוצר  בהצלחה\n מיד תוצג רשימת הלקוחות", "אישור");
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
                            customerToList = CustomersToListView.ToList().Find(i => i.Id == customer.Id);

                            BorderEnterNumber.Visibility = Visibility.Hidden;
                            update.Visibility = Visibility.Visible;
                            UpdatingWindow(customerToList.Id);
                            break;
                        }
                        if (customer.name != default&&customer.phone!=default)
                        {
                            MessageBoxResult messageBoxResult = MessageBox.Show("האם ברצונך לאשר עדכון זה", "אישור", MessageBoxButton.OKCancel);
                            switch (messageBoxResult)
                            {
                                case MessageBoxResult.OK:
                                   
                                    customerToList.name = customer.name;
                                    customerToList.phone = customer.phone;
                                    _ = blGui.updateCustomerData(customerToList.Id, customerToList.name, customerToList.phone);
                                    MessageBox.Show("העדכון בוצע בהצלחה\n מיד תוצג רשימת הרחפנים", "אישור");
                                    CustomerListView.SelectedItem = null;
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
                    case Actions.REMOVE:
                        break;
                    default:
                        break;
                }
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
            switch (action)
            {
                case "List":
                    if (actions != Actions.LIST)
                    {
                        ListWindow();
                        CustomerListView.SelectedItem = null;
                    }
                    else
                        Close();
                    break;
                case "ByParcel":
                    new ParcelWindow(blGui, "List").Show();
                    Close();
                    break;
                default:
                    Close();
                    break;
            }
        }

        private void fromCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idParcel = ((ParcelInCustomer)fromCustomer.SelectedItem).Id;
            new ParcelWindow(blGui, "ByCustomer", idParcel).Show();
            Close();
        }

        private void toCustomer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int idParcel = ((ParcelInCustomer)toCustomer.SelectedItem).Id;
            new ParcelWindow(blGui, "ByCustomer", idParcel).Show();
            Close();
        }

        private void CustomerListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            customerToList = (CustomerToList)CustomerListView.SelectedItem;
            if (customerToList != null)
            {
                UpdatingWindow(customerToList.Id);
            }
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onlyNumbersForID(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void phonePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regex = new("^[0][0-9]{0,9}$");
            e.Handled = !regex.IsMatch(temp);
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void onlyAlphaBeta(object sender, TextCompositionEventArgs e)
        {
            
            Regex regex = new("[^א-ת]$");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lungetudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[2-3]{1,2}[.]{0,1}$");
            Regex regexB = new("^[2-3]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }
        /// <summary>
        /// regular expration funcation.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lattitudePattren(object sender, TextCompositionEventArgs e)
        {
            string temp = ((TextBox)sender).Text + e.Text;
            Regex regexA = new("^[3-4]{1,2}[.]{0,1}$");
            Regex regexB = new("^[3-4]{1,2}[.][0-9]{0,9}$");
            e.Handled = !(regexA.IsMatch(temp) || regexB.IsMatch(temp));
        }
        /// <summary>
        /// MouseDown event.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {

            if (e.ChangedButton == MouseButton.Left)
                DragMove();
        }
    }
}
