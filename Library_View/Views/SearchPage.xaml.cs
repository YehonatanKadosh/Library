using Common;
using Logic_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Library_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SearchPage : Page
    {
        User user;
        public SearchPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Initiate the page
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as User;
            Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
        }

        // updates the Items list by partial item's name
        private async void Name_Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Name_Search_Box.Text == "")
                Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            else
                Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Partial_Name(Name_Search_Box.Text, Item_ListView.ItemsSource as List<Item>);
        }

        // updates the items list by exact isbn
        private async void ISBN_Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ISBN_Search_Box.Text == "")
                Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            else
                Item_ListView.ItemsSource = new List<Item>() { await Service_Manager.Instance.Item_Service_Instance.Get_Item_By_ISBN(int.Parse(ISBN_Search_Box.Text)) };
        }

        // pudates the items list by date range
        private void DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
            {
                if (sender.Name == "Start_Date")
                {
                    if (End_Date.Date == null)
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(Start_Date.Date.Value.DateTime, DateTime.MaxValue, Item_ListView.ItemsSource as List<Item>);
                    else if (Start_Date.Date > End_Date.Date)
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(Start_Date.Date.Value.DateTime, DateTime.MaxValue, Item_ListView.ItemsSource as List<Item>);
                    else if (Start_Date.Date == null)
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(DateTime.MinValue, DateTime.MaxValue, Item_ListView.ItemsSource as List<Item>);
                    else
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(Start_Date.Date.Value.DateTime, End_Date.Date.Value.DateTime, Item_ListView.ItemsSource as List<Item>);
                }
                else
                {
                    if (Start_Date.Date == null)
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(DateTime.MinValue, End_Date.Date.Value.DateTime, Item_ListView.ItemsSource as List<Item>);
                    else if (Start_Date.Date > End_Date.Date)
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(DateTime.MinValue, End_Date.Date.Value.DateTime, Item_ListView.ItemsSource as List<Item>); // try catch null/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    else if (End_Date.Date == null)
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(DateTime.MinValue, DateTime.MaxValue, Item_ListView.ItemsSource as List<Item>);
                    else
                        Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Date(Start_Date.Date.Value.DateTime, End_Date.Date.Value.DateTime, Item_ListView.ItemsSource as List<Item>);
                }
            }
            catch (ArgumentNullException) { }//Start or end is nullable

        }

        // resets the grid's textboxs and choices
        private async void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            Name_Search_Box.Text = ISBN_Search_Box.Text = "";
            Start_Date.Date = DateTimeOffset.MinValue;
            End_Date.Date = DateTimeOffset.MaxValue;
            Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
        }

        // generates an invoice of the selected item by user type and customer
        private async void Purchase_Button_Click(object sender, RoutedEventArgs e)
        {
            if ((Item_ListView.SelectedItem as Item) != null)
            {
                    Customer chosen;
                if (user.Permission_Level == Level.Guest)
                {
                    chosen = (await Service_Manager.Instance.Customer_Service_Instance.Customer_Search_Partial_NameAsync("Not"))[0];
                    MessageDialog message = new MessageDialog((await Service_Manager.Instance.Invoice_Service_Instance.Generate_InvoiceAsync(chosen, Item_ListView.SelectedItem as Item)).ToString());
                    await message.ShowAsync();
                    Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();

                }
                else 
                {
                    ShatterBackground(true);
                    Customer_ListView.Visibility = Visibility.Visible;
                    Customer_ListView.ItemsSource = await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync();
                }
                   // chosen = ShowCustomers();
            }
        }

        // switch to customer choosing if the user isnt a guest
        private void ShatterBackground(bool v)
        {
            if(v)
              Item_ListView.Visibility = Genre_ListView.Visibility = Start_Date.Visibility = End_Date.Visibility = ISBN_Search_Box.Visibility = Name_Search_Box.Visibility = Clear_Button.Visibility = Purchase_Button.Visibility = Visibility.Collapsed; 
            else
                Item_ListView.Visibility = Genre_ListView.Visibility = Start_Date.Visibility = End_Date.Visibility = ISBN_Search_Box.Visibility = Name_Search_Box.Visibility = Clear_Button.Visibility = Visibility.Visible;

        }

        // check if the item is avalable for purchase  when selected
        private void Item_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ListView).SelectedItem as Item) != null)
            {
                if (Service_Manager.Instance.Item_Service_Instance.Available((sender as ListView).SelectedItem as Item))
                    Purchase_Button.Visibility = Visibility.Visible;
                else Purchase_Button.Visibility = Visibility.Collapsed;
            }
        }

        // updates the item list based on the genre selected
        private void Genre_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((sender as ListView).SelectedItem as Genre) != null)
            {
                Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Genre(Genre_ListView.SelectedItem as Genre, Item_ListView.ItemsSource as List<Item>);
            }
        }

        // selected customer goes to the invoice generated and shown
        private async void Customer_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Customer_ListView.SelectedItem as Customer) != null)
            {
                MessageDialog message = new MessageDialog((await Service_Manager.Instance.Invoice_Service_Instance.Generate_InvoiceAsync(Customer_ListView.SelectedItem as Customer, Item_ListView.SelectedItem as Item)).ToString());
                await message.ShowAsync();
                Customer_ListView.Visibility = Visibility.Collapsed;
                ShatterBackground(false);
                Clear_Button_Click(sender, e);
            }
        }

        // go back to the main page
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (user.Permission_Level == Level.Guest)
                Frame.Navigate(typeof(GuestMainPage), user);
            if (user.Permission_Level == Level.Employee)
                Frame.Navigate(typeof(EmployeeMainPage), user);
            if (user.Permission_Level == Level.Manager)
                Frame.Navigate(typeof(ManagerMainPage), user);
        }

        // make sure the integeric value stays integeric
        private void ISBN_Search_Box_BeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => char.IsLetter(c)) || ISBN_Search_Box.Text.Length > 8;
        }
    }
}
