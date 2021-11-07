using Common;
using Logic_BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Library_View
{
    public sealed partial class ItemsPage : Page
    {
        User user;
        public ItemsPage()
        {
            this.InitializeComponent();
        }

        // initiate the page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as User;
            Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
            Item_Type_ComboBox.ItemsSource = Enum.GetValues(typeof(ItemType));
            Item_Type_ComboBox.SelectedItem = ItemType.Book;
            await New_Item_UI_Set(false);
            await Update_An_Item_UI_SetAsync(false);
        }

        // Show every item selected on the update board
        private async void Item_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Item_ListView.SelectedItem as Item) != null)
                await Update_An_Item_UI_SetAsync(true);
        }
        
        //Show or Hide the Update item board
        public async Task Update_An_Item_UI_SetAsync(bool state)
        {
            if (state)
            {
                await New_Item_UI_Set(false);
                Book_Name_TextBlock.Visibility = Price_TextBox.Visibility = Discount_TextBox.Visibility = Amount_In_Stock_TextBox.Visibility = Update_Button.Visibility = Delete.Visibility = Visibility.Visible;
                Book_Name_TextBlock.Text = (Item_ListView.SelectedItem as Item).Name;
                Discount_TextBox.Text = ((Item_ListView.SelectedItem as Item).Discount * 100).ToString();
                Amount_In_Stock_TextBox.Text = (Item_ListView.SelectedItem as Item).Amount_In_Stock.ToString();
                Price_TextBox.Text = (Item_ListView.SelectedItem as Item).Price.ToString();
                Genre_ListView.SelectionMode = ListViewSelectionMode.Single;

                Price_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                Discount_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                Amount_In_Stock_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                Book_Name_TextBlock.Visibility = Price_TextBox.Visibility = Discount_TextBox.Visibility = Amount_In_Stock_TextBox.Visibility = Update_Button.Visibility = Delete.Visibility = Visibility.Collapsed;
                Genre_ListView.SelectionMode = ListViewSelectionMode.Single;
                Genre_ListView.SelectionChanged += Genre_ListView_SelectionChanged;
            }
        }

        //Show or Hide the item creating board
        public async Task New_Item_UI_Set(bool state)
        {
            if (state)
            {
                await Update_An_Item_UI_SetAsync(false);
                Add_Button.Visibility = Price_TextBox.Visibility = Discount_TextBox.Visibility = Amount_In_Stock_TextBox.Visibility = New_Book_Name_TextBox.Visibility = Item_Type_ComboBox.Visibility = Author_TextBox.Visibility = Date_Printed_DatePicker.Visibility = Summary_Issue_TextBox.Visibility = Visibility.Visible;
                Price_TextBox.Text = Discount_TextBox.Text = Amount_In_Stock_TextBox.Text = New_Book_Name_TextBox.Text = Author_TextBox.Text = Summary_Issue_TextBox.Text = "";
                Genre_ListView.SelectionMode = ListViewSelectionMode.Multiple;
                Genre_ListView.SelectionChanged -= Genre_ListView_SelectionChanged;

                Price_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                Discount_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                Amount_In_Stock_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                New_Book_Name_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                Item_Type_ComboBox.BorderBrush = new SolidColorBrush(Colors.White);
                Author_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                Date_Printed_DatePicker.BorderBrush = new SolidColorBrush(Colors.White);
                Summary_Issue_TextBox.BorderBrush = new SolidColorBrush(Colors.White);
                Genre_ListView.BorderBrush = new SolidColorBrush(Colors.White);
            }
            else
            {
                Add_Button.Visibility = Price_TextBox.Visibility = Discount_TextBox.Visibility = Amount_In_Stock_TextBox.Visibility = New_Book_Name_TextBox.Visibility = Item_Type_ComboBox.Visibility = Author_TextBox.Visibility = Date_Printed_DatePicker.Visibility = Summary_Issue_TextBox.Visibility = Visibility.Collapsed;
            }
            if ((ItemType)Item_Type_ComboBox.SelectedItem == ItemType.Book)
                Summary_Issue_TextBox.PlaceholderText = "Summary";
            else
                Summary_Issue_TextBox.PlaceholderText = "Issue";
        }

        //clear all selections "restarts the page"
        private async void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            Name_Search_Box.Text = ISBN_Search_Box.Text = "";
            Start_Date.Date = DateTimeOffset.MinValue;
            End_Date.Date = DateTimeOffset.MaxValue;
            Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
            Item_Type_ComboBox.ItemsSource = Enum.GetValues(typeof(ItemType));
            Item_Type_ComboBox.SelectedItem = ItemType.Book;
            await New_Item_UI_Set(false);
            await Update_An_Item_UI_SetAsync(false);
        }

        // update the item's list by partial item's name
        private async void Name_Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Name_Search_Box.Text == "")
                Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            else
                Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Partial_Name(Name_Search_Box.Text, Item_ListView.ItemsSource as List<Item>);
        }

        // update the items list by an exsact isbn
        private async void ISBN_Search_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ISBN_Search_Box.Text == "")
                Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            else
                Item_ListView.ItemsSource = new List<Item>() { await Service_Manager.Instance.Item_Service_Instance.Get_Item_By_ISBN(int.Parse(ISBN_Search_Box.Text)) };
        }

        //update the items list by a selected genre
        private void Genre_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((Genre_ListView.SelectedItem as Genre) != null)
                Item_ListView.ItemsSource = Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Genre(Genre_ListView.SelectedItem as Genre, Item_ListView.ItemsSource as List<Item>);
        }

        // update the items list by starting date change
        private void Start_Date_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
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
            catch (ArgumentNullException) { }
        }

        // update the items list by ending date change
        private void End_Date_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            try
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
            catch (ArgumentNullException) { }
        }

        // go back to the main page by permission level
        private void Back_Click(object sender, RoutedEventArgs e)
        {
            if (user.Permission_Level == Level.Guest)
                Frame.Navigate(typeof(GuestMainPage), user);
            if (user.Permission_Level == Level.Employee)
                Frame.Navigate(typeof(EmployeeMainPage), user);
            if (user.Permission_Level == Level.Manager)
                Frame.Navigate(typeof(ManagerMainPage), user);
        }

        // make sure the Integeric textbox stays Integeric
        private void Integer_TextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        => args.Cancel = args.NewText.Any(c => char.IsLetter(c)) || sender.Text.Length > 8;

        // make sure the Integeric textbox stays Integeric based on item's type
        private void Summary_Issue_TextBox_TextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            if ((ItemType)Item_Type_ComboBox.SelectedItem == ItemType.Journal)
                args.Cancel = args.NewText.Any(c => char.IsLetter(c)) || sender.Text.Length > 8;
        }

        // set the new item board on the UI
        private async void Add_New_Item_Button_Click(object sender, RoutedEventArgs e)
        => await New_Item_UI_Set(true);

        // gets the values of the creation item board and creating it if all checks up
        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Price_TextBox.Text != "" && Discount_TextBox.Text != "" && Amount_In_Stock_TextBox.Text != "" && New_Book_Name_TextBox.Text != "" && Item_Type_ComboBox.SelectedItem != null && Author_TextBox.Text != "" && Date_Printed_DatePicker.Date != null && Summary_Issue_TextBox.Text != "" && Genre_ListView.SelectedItems.Count != 0)
            {
                try
                {
                    List<Genre> genres = new List<Genre>();
                    foreach (object item in Genre_ListView.SelectedItems)
                        genres.Add(item as Genre);
                    if ((ItemType)Item_Type_ComboBox.SelectedValue == ItemType.Book)
                        await Service_Manager.Instance.Item_Service_Instance.Generate_New_BookAsync(New_Book_Name_TextBox.Text, genres, Date_Printed_DatePicker.Date.Value.DateTime, int.Parse(Amount_In_Stock_TextBox.Text), double.Parse(Price_TextBox.Text), double.Parse(Discount_TextBox.Text) / 100, Author_TextBox.Text, Summary_Issue_TextBox.Text);
                    else
                        await Service_Manager.Instance.Item_Service_Instance.Generate_New_JournalAsync(New_Book_Name_TextBox.Text, genres, Date_Printed_DatePicker.Date.Value.DateTime, int.Parse(Amount_In_Stock_TextBox.Text), double.Parse(Price_TextBox.Text), double.Parse(Discount_TextBox.Text) / 100, Author_TextBox.Text, int.Parse(Summary_Issue_TextBox.Text));
                    Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
                }
                catch (Item_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
            }
            else
            {
                if (Price_TextBox.Text == "")
                    Price_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Discount_TextBox.Text == "")
                    Discount_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Amount_In_Stock_TextBox.Text == "")
                    Amount_In_Stock_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (New_Book_Name_TextBox.Text == "")
                    New_Book_Name_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Item_Type_ComboBox.SelectedItem == null)
                    Item_Type_ComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Author_TextBox.Text == "")
                    Author_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Date_Printed_DatePicker.Date == null)
                    Date_Printed_DatePicker.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Summary_Issue_TextBox.Text == "")
                    Summary_Issue_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Genre_ListView.SelectedItems.Count == 0)
                    Genre_ListView.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        // Updates an item selected by the update board if all checks up
        private async void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Item_ListView.SelectedItem != null && Price_TextBox.Text != "" && Discount_TextBox.Text != "" && Amount_In_Stock_TextBox.Text != "")
            {
                await Service_Manager.Instance.Item_Service_Instance.Update_Item(Item_ListView.SelectedItem as Item, int.Parse(Amount_In_Stock_TextBox.Text), double.Parse(Price_TextBox.Text), double.Parse(Discount_TextBox.Text) / 100);
                Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
            }
            else
            {
                if (Price_TextBox.Text == "")
                    Price_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Discount_TextBox.Text == "")
                    Discount_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Amount_In_Stock_TextBox.Text == "")
                    Amount_In_Stock_TextBox.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        // Delets the selected item
        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (Item_ListView.SelectedItem != null)
            {
                try
                {
                    await Service_Manager.Instance.Item_Service_Instance.Remove_Element_From_Database(Item_ListView.SelectedItem as Item);
                    Item_ListView.ItemsSource = await Service_Manager.Instance.Item_Service_Instance.Get_AllAsync();
                }
                catch (Item_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
            }
        }

        //sets the value between book and journals extras parameter
        private void Item_Type_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Item_Type_ComboBox.SelectedValue != null)
            {
                if ((ItemType)Item_Type_ComboBox.SelectedValue == ItemType.Book)
                    Summary_Issue_TextBox.PlaceholderText = "Summary";
                else
                {
                    Summary_Issue_TextBox.PlaceholderText = "Issue";
                    Summary_Issue_TextBox.Text = "";
                }   
            }
        }
    }
}
