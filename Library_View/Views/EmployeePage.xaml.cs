using Common;
using Logic_BLL;
using System;
using System.Collections.Generic;
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
    public sealed partial class EmployeePage : Page
    {
        User user;
        public EmployeePage()
        {
            this.InitializeComponent();
        }

        // initiate the page
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            user = e.Parameter as User;
            User_ListView.ItemsSource = await Service_Manager.Instance.User_Service_Instance.Get_AllAsync();
            Premission_Level_ComboBox.ItemsSource = new List<Level>() { Level.Employee, Level.Guest, Level.Manager };
        }

        //enables the user editing an employee/users
        private void Employee_ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (User_ListView.SelectedItem != null)
            {
                Delete_Button.Visibility = Update_Button.Visibility = Delete_Button.Visibility = Visibility.Visible;
                Employee_Name_Box.Text = (User_ListView.SelectedItem as User).User_Name;
                Employee_Password_Box.Text = (User_ListView.SelectedItem as User).Password;
                Premission_Level_ComboBox.SelectedItem = (User_ListView.SelectedItem as User).Permission_Level;
                Premission_Level_ComboBox.BorderBrush = new SolidColorBrush(Colors.White);
                Employee_Name_Box.BorderBrush = new SolidColorBrush(Colors.White);
                Employee_Password_Box.BorderBrush = new SolidColorBrush(Colors.White);
            }
        }

        //update the user list based on partial user-name
        private async void Search_emplioyee_Box_TextChangedAsync(object sender, TextChangedEventArgs e)
        {
            if (Search_emplioyee_Box.Text == "")
                User_ListView.ItemsSource = await Service_Manager.Instance.User_Service_Instance.Get_AllAsync();
            else
                User_ListView.ItemsSource = await Service_Manager.Instance.User_Service_Instance.User_SearchAsync(Search_emplioyee_Box.Text);
        }

        //allowing the user to enter new user
        private void Add_New_Employee_Button_Click(object sender, RoutedEventArgs e)
        {
            Employee_Name_Box.Text = Employee_Password_Box.Text = "";
            Add_New_Employee_Button.Visibility = Visibility.Collapsed;
            Add.Visibility = Visibility.Visible;
        }

        // deleting the selected user from the user list
        private async void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            if (User_ListView.SelectedItem != null)
            {
                try
                {
                    await Service_Manager.Instance.User_Service_Instance.Remove_Element_From_Database(User_ListView.SelectedItem as User);
                    User_ListView.ItemsSource = await Service_Manager.Instance.User_Service_Instance.Get_AllAsync();
                }
                catch (User_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
            }
        }

        // updates users password and permission level of a selected user
        private async void Update_Button_Click(object sender, RoutedEventArgs e)
        {
            if (User_ListView.SelectedItem != null && Premission_Level_ComboBox.SelectedItem != null && Employee_Name_Box.Text != "" && Employee_Password_Box.Text != "")
            {
                Employee_Name_Box.Text = (User_ListView.SelectedItem as User).User_Name;
                await Service_Manager.Instance.User_Service_Instance.Update_UserAsync(User_ListView.SelectedItem as User, Employee_Password_Box.Text, (Level)Premission_Level_ComboBox.SelectedItem);
                User_ListView.ItemsSource = await Service_Manager.Instance.User_Service_Instance.Get_AllAsync();
            }
            else
            {
                if (Premission_Level_ComboBox.SelectedItem == null)
                    Premission_Level_ComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Employee_Name_Box.Text == "")
                    Employee_Name_Box.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Employee_Password_Box.Text == "")
                    Employee_Password_Box.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }
        
        // Adds new user based on parameters on the board
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if (Premission_Level_ComboBox.SelectedItem != null && Employee_Name_Box.Text != "" && Employee_Password_Box.Text != "")
            {
                try
                {
                    await Service_Manager.Instance.User_Service_Instance.Generate_UserAsync(Employee_Name_Box.Text, Employee_Password_Box.Text, (Level)Premission_Level_ComboBox.SelectedItem);
                    User_ListView.ItemsSource = await Service_Manager.Instance.User_Service_Instance.Get_AllAsync();
                }
                catch (Customer_Exception exception)
                {
                    MessageDialog message = new MessageDialog(exception.Message);
                    await message.ShowAsync();
                }
                Add.Visibility = Visibility.Collapsed;
                Add_New_Employee_Button.Visibility = Visibility.Visible;

            }
            else
            {
                if (Premission_Level_ComboBox.SelectedItem == null)
                    Premission_Level_ComboBox.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Employee_Name_Box.Text == "")
                    Employee_Name_Box.BorderBrush = new SolidColorBrush(Colors.Red);
                if (Employee_Password_Box.Text == "")
                    Employee_Password_Box.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        // navigate back to main page based on the permission level
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
