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
    /// Interaction logic for CustomerPackageWindow.xaml
    /// </summary>
    public partial class CustomerPackageWindow : Window
    {
        private IBL blGui;

        private Parcel parcel;
        private ParcelToList parcelToList;
        /// <summary>
        /// c-tor
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="id"></param>
        public CustomerPackageWindow(IBL bL, int id = 0)
        {
            InitializeComponent();
            blGui = bL;
            try
            {
                parcelToList = blGui.DisplaysIistOfparcels(i => i.Id == id).First();
                parcel = blGui.GetParcel(id); DataContext = parcel;
                DataContext = parcel;
                switch (parcelToList.parcelStatus)
                {
                    case BO.parcelStatus.defined:
                        ScheduledTextBox.Text = "לא שוייך";
                        PickedUpTextBox.Text = "לא נאסף";
                        DeliveredTextBox.Text = "לא סופק";
                        TextBlockdroneInParcel.Text = "החבילה לא שויכה לרחפן";
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
        /// <summary>
        /// This function defines the actions behind the button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
