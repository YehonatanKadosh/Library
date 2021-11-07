using Common;
using Logic_BLL;
using System;
using System.Collections.Generic;
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
    public sealed partial class GenrePage : Page
    {
        User user;
        public GenrePage()
        {
            this.InitializeComponent();
        }

        // initiate the page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as User;
            Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
            Type_ComboBox.ItemsSource = new List<GenreType>() { GenreType.Fiction, GenreType.Nonfiction };

        }

        //enabling the user to remove the genre from database
        private void Genre_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Genre_ListView.SelectedItem != null)
                Genre_Remove_Button.Visibility = Visibility.Visible;
            else
                Genre_Remove_Button.Visibility = Visibility.Collapsed;
        }

        // allow the user to add new genre as the parameters cheks up
        private void Genre_Name_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Genre_Name_Box.Text != "" && Type_ComboBox.SelectedItem != null)
                Add_Button.Visibility = Visibility.Visible;
            else
                Add_Button.Visibility = Visibility.Collapsed;
        }

        // removes the genre selected from the database
        private async void Genre_Remove_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Genre_ListView.SelectedItem != null)
            {
                try
                {
                    await Service_Manager.Instance.Genre_Service_Instance.Remove_Element_From_Database(Genre_ListView.SelectedItem as Genre);
                    Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
                }
                catch (Genre_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
            }
        }

        // update genre list based on genre partial name
        private async void Search_Genre_Box_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Search_Genre_Box.Text == "")
                Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
            else
            {
                List<Genre> result = await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_FictionAsync(Search_Genre_Box.Text);
                result.AddRange(await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_NonfictionAsync(Search_Genre_Box.Text));
                Genre_ListView.ItemsSource = result;
            }
        }

        // adds a new genre to the database based on the parameters inputed
        private async void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            if (Type_ComboBox.SelectedItem != null && Genre_Name_Box.Text != "")
            {
                try
                {
                    await Service_Manager.Instance.Genre_Service_Instance.Create_New_GenreAsync(Genre_Name_Box.Text, (GenreType)Type_ComboBox.SelectedItem);
                    Genre_ListView.ItemsSource = await Service_Manager.Instance.Genre_Service_Instance.Get_AllAsync();
                }
                catch (Genre_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
            }
        }

        // allow the user to add new genre as the parameters cheks up
        private void Type_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Genre_Name_Box.Text != "" && Type_ComboBox.SelectedItem != null)
                Add_Button.Visibility = Visibility.Visible;
            else
                Add_Button.Visibility = Visibility.Collapsed;
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
    }
}
