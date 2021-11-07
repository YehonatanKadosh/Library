using Common;
using Logic_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Library_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CustomerPage : Page
    {
        User user;
        public CustomerPage()
        {
            this.InitializeComponent();
        }

        // initiate the page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as User;
            Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync();
            Discount_ComboBox.ItemsSource = new List<double>() { 0, 5, 10, 15, 20, 25, 30, 35, 40, 45, 50, 65, 70, 75, 80, 85, 90, 95 };
        }

        // display the selected customer and leting the user the ability to edit him
        private void Customer_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Customer_ListView.SelectedItem != null)
            {
                Add_New_Customer.Visibility = Delete_Button.Visibility = Update_Button.Visibility = Delete_Button.Visibility = Visibility.Visible;
                ID.Visibility = Birthday_Date.Visibility = Add.Visibility = Visibility.Collapsed;
                Customer_First_Name_Box.Text = (Customer_ListView.SelectedItem as Customer).First_name;
                Customer_Last_Name_Box.Text = (Customer_ListView.SelectedItem as Customer).Last_name;
                Discount_ComboBox.SelectedItem = (Customer_ListView.SelectedItem as Customer).Discount * 100;
                Customer_First_Name_Box.BorderBrush = Customer_Last_Name_Box.BorderBrush =
                Discount_ComboBox.BorderBrush = Birthday_Date.BorderBrush =
                ID.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }

        // update the customer list according to partial name
        private async void Customer_Name_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Customer_Name.Text == "")
                Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync();
            else
                Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Customer_Search_Partial_NameAsync(Customer_Name.Text);
        }

        // updates selected customer witht the parameters within the update board
        private async void Update_Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            if (Customer_ListView.SelectedItem != null && Discount_ComboBox.SelectedItem != null && Customer_First_Name_Box.Text != "" && Customer_Last_Name_Box.Text != "")
            {
                await Service_Manager.Instance.Customer_Service_Instance.Update_CustomerAsync(Customer_ListView.SelectedItem as Customer, Customer_First_Name_Box.Text, Customer_Last_Name_Box.Text, ((double)Discount_ComboBox.SelectedItem) / 100);
                Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync();
            }
            else
            {
                if (Discount_ComboBox.SelectedItem == null)
                    Discount_ComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Customer_First_Name_Box.Text == "")
                    Customer_First_Name_Box.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Customer_Last_Name_Box.Text == "")
                    Customer_Last_Name_Box.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        //deleting a selected customer
        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Customer_ListView.SelectedItem != null)
            {
                try
                {
                    await Service_Manager.Instance.Customer_Service_Instance.Remove_Element_From_Database(Customer_ListView.SelectedItem as Customer);
                    Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync();
                }
                catch (Customer_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
            }
        }

        // letting the user insert parameters for a new customer
        private void Add_New_Customer_Click(object sender, RoutedEventArgs e)
        {
            Customer_First_Name_Box.Text = Customer_Last_Name_Box.Text = "";
            Add_New_Customer.Visibility = Visibility.Collapsed;
            Add.Visibility = Birthday_Date.Visibility = ID.Visibility = Visibility.Visible;
        }

        // adding new customer based on the parameters on the adding board
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Discount_ComboBox.SelectedItem != null && ID.Text != "" && Birthday_Date.Date != null && Customer_First_Name_Box.Text != "" && Customer_Last_Name_Box.Text != "")
            {
                try
                {
                    await Service_Manager.Instance.Customer_Service_Instance.Generate_CustomerAsync(Customer_First_Name_Box.Text, Customer_Last_Name_Box.Text, int.Parse(ID.Text), Birthday_Date.Date.Value.DateTime, ((double)Discount_ComboBox.SelectedItem) / 100);
                    Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync();
                }
                catch (Customer_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
                Update_Button.Visibility = Delete_Button.Visibility = Add.Visibility = Birthday_Date.Visibility = ID.Visibility = Visibility.Collapsed;
                Add_New_Customer.Visibility = Visibility.Visible;

            }
            else
            {
                if (Discount_ComboBox.SelectedItem == null)
                    Discount_ComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Customer_First_Name_Box.Text == "")
                    Customer_First_Name_Box.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Customer_Last_Name_Box.Text == "")
                    Customer_Last_Name_Box.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Birthday_Date.Date == null)
                    Birthday_Date.BorderBrush = new SolidColorBrush(Colors.Red);
                if (ID.Text == "")
                    ID.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        //update the Customer list based on tpartial id
        private async void Search_Customer_By_ID_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search_Customer_By_ID_Box.Text == "")
                Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync();
            else
                Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Customer_Search_Partial_IDAsync(int.Parse(Search_Customer_By_ID_Box.Text));
        }

        // navigate bacl to the main page based on permission level
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (user.Permission_Level == Level.Guest)
                Frame.Navigate(typeof(GuestMainPage), user);
            if (user.Permission_Level == Level.Employee)
                Frame.Navigate(typeof(EmployeeMainPage), user);
            if (user.Permission_Level == Level.Manager)
                Frame.Navigate(typeof(ManagerMainPage), user);
        }

        // make sure integeric textbox stays integeric
        private void ID_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = (args.NewText.Any(c => char.IsLetter(c)) || sender.Text.Length > 8);
        }
    }
}
