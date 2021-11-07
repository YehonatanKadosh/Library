using Common;
using Logic_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Library_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class InvoicePage : Page
    {
        User user;
        public InvoicePage()
        {
            this.InitializeComponent();
        }

        // initiate the page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as User;
            Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Get_AllAsync();
        }

        // update the invoice list based on partial customer name
        private async void Customer_Name_Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Customer_Name_Search_Box.Text == "")
                Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Get_AllAsync();
            else
                Invoice_ListView.ItemsSource = Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Customer_Partial_Name(Customer_Name_Search_Box.Text, Invoice_ListView.ItemsSource as List<Invoice>);
        }

        // update invoice list based on partial customers id
        private async void Customer_ID_Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Customer_ID_Search_Box.Text == "")
                Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Get_AllAsync();
            else
                Invoice_ListView.ItemsSource = Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Customer_Partial_ID(int.Parse(Customer_ID_Search_Box.Text), Invoice_ListView.ItemsSource as List<Invoice>) ;
           
        }

        // update invoice list based on partial invoice's id
        private async void Invoice_ID_Search_Box_Copy_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Invoice_ID_Search_Box.Text == "")
                Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Get_AllAsync();
            else
                Invoice_ListView.ItemsSource = Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Partial_Invoice_ID(int.Parse(Invoice_ID_Search_Box.Text), Invoice_ListView.ItemsSource as List<Invoice>);    
        }

        // update invoice list based on date
        private async void DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
            {
                if (sender.Name == "Start_Date")
                {
                    if (End_Date.Date/*.Value.DateTime*/ == null)
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(Start_Date.Date.Value.DateTime, DateTime.MaxValue);
                    else if (Start_Date.Date > End_Date.Date)
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(Start_Date.Date.Value.DateTime, DateTime.MaxValue);
                    else if (Start_Date.Date == null)
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(DateTime.MinValue, DateTime.MaxValue);
                    else
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(Start_Date.Date.Value.DateTime, End_Date.Date.Value.DateTime);
                }
                else
                {
                    if (Start_Date.Date == null)
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(DateTime.MinValue, End_Date.Date.Value.DateTime);
                    else if (Start_Date.Date > End_Date.Date)
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(DateTime.MinValue, End_Date.Date.Value.DateTime); 
                    else if (End_Date.Date == null)
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(DateTime.MinValue, DateTime.MaxValue);
                    else
                        Invoice_ListView.ItemsSource = await Service_Manager.Instance.Invoice_Service_Instance.Invoice_Search_By_Date(Start_Date.Date.Value.DateTime, End_Date.Date.Value.DateTime);
                }
            }
            catch (ArgumentNullException) { } // case that both of the dates are null
        }
        
        // navigate back to the main page based on permission level
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (user.Permission_Level == Level.Guest)
                Frame.Navigate(typeof(GuestMainPage), user);
            if (user.Permission_Level == Level.Employee)
                Frame.Navigate(typeof(EmployeeMainPage), user);
            if (user.Permission_Level == Level.Manager)
                Frame.Navigate(typeof(ManagerMainPage), user);
        }

        // make sure the integeric textbox stays integeric
        private void ID_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        => args.Cancel = args.NewText.Any(c => char.IsLetter(c)) || sender.Text.Length > 8;
    }
}
