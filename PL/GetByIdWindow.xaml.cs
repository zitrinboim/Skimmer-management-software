using System;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using BO;
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
    /// Interaction logic for GetByIdWindow.xaml
    /// </summary>
    public partial class GetByIdWindow : Window
    {
        IBL BLGui;
        string type;
        public bool IsClosedByButton { get; set; }

        /// <summary>
        /// default ctor. 
        /// </summary>
        /// <param name="type">type of person. trainee/tester</param>
        public GetByIdWindow(IBL bL, string type = "")
        {
            BLGui = bL;
            IsClosedByButton = false;
            this.FlowDirection = FlowDirection.RightToLeft;
            this.type = type;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            InitializeComponent();
        }

        //events
        //---------------------------------------------------------------------------------
        
        private void TxtBx_ID_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBx_ID.Background != Brushes.White)
                TxtBx_ID.Background = Brushes.White;
        }
        private void TxtBx_ID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                Button_Click_OK(sender, new RoutedEventArgs());
            }
        }
        static bool CheckIDNo(string strID)
        {
            int[] id_12_digits = { 1, 2, 1, 2, 1, 2, 1, 2, 1 };
            int count = 0;

            if (strID == null)
                return false;

            strID = strID.PadLeft(9, '0');

            for (int i = 0; i < 9; i++)
            {
                int num = Int32.Parse(strID.Substring(i, 1)) * id_12_digits[i];

                if (num > 9)
                    num = (num / 10) + (num % 10);

                count += num;
            }

            return (count % 10 == 0);
        }
        //---------------------------------------------------------------------------------

        //click events. OK and Cancel
        //---------------------------------------------------------------------------------
        private void Button_Click_OK(object sender, RoutedEventArgs e)
        {
           
                switch (this.type)
                {
                    case "Customer":
                        Customer customer;
                        try
                        {

                            if (TxtBx_ID.Text.Length != TxtBx_ID.MaxLength || !TxtBx_ID.Text.All(char.IsDigit) || !CheckIDNo(TxtBx_ID.Text))
                            {
                                TxtBx_ID.Background = Brushes.Red;
                                MessageBox.Show("מספר תעודת הזהות צריך להיות 9 ספרות בדיוק, ולייצג מספר ת.ז. אמיתי.", "קלט שגוי", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            }
                            else
                            customer = BLGui.GetCustomer(int.Parse( TxtBx_ID.Text)); 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                        Close();
                        break;
                    case "Drone":
                         Drone drone;
                        try
                        {
                            drone = BLGui.GetDrone(int.Parse(TxtBx_ID.Text));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                    Close();
                    break;
                case "Station":
                         Station station ;
                        try
                        {
                            station = BLGui.GetStation(int.Parse(TxtBx_ID.Text));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                    Close();
                    break;
                case "Parcel":
                         Parcel parcel ;
                        try
                        {
                            parcel = BLGui.GetParcel(int.Parse(TxtBx_ID.Text));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                            return;
                        }
                        Close();
                        break;
                    case "":
                        IsClosedByButton = true;
                        Close();
                        break;
                    default:
                        MessageBox.Show("שגיאה פנימית. case לא קיים", "שגיאה פנימית", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None, MessageBoxOptions.RtlReading | MessageBoxOptions.RightAlign);
                        break;
                }

            
        }
        private void Button_Click_Cancel(object sender, RoutedEventArgs e)
        {
            Close();
        }
        //---------------------------------------------------------------------------------
    }
}
