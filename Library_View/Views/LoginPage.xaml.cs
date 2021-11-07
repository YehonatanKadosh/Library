using Common;
using Logic_BLL;
using System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Library_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // login check and rout - set specific messeges to the user
        private async void Login_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Button Selected_Button = sender as Button;
            if (Selected_Button.Name == "Guest_Button")
            {
                LogInRout((await Service_Manager.Instance.User_Service_Instance.User_SearchAsync("Guest"))[0]);
            }
            else if (User_Name_Box.Text != "" && Password_Box.Password != "")
            {
                try
                {
                    LogInRout(await Service_Manager.Instance.User_Service_Instance.Log_In(User_Name_Box.Text, Password_Box.Password));
                }
                catch(User_Exception message)
                {
                    ContentDialog alertUser = new ContentDialog
                    {
                        Content = message.Message,
                        PrimaryButtonText = "try again"
                    };
                    await alertUser.ShowAsync();
                    User_Name_Box.Text = ""; 
                    Password_Box.Password = "";
                }
            }
            if(User_Name_Box.Text == "")
                User_Name_Box.BorderBrush = new SolidColorBrush(Colors.Red);
            if (Password_Box.Password == "")
                Password_Box.BorderBrush = new SolidColorBrush(Colors.Red);
        }

        // rout to login page by user premission level
        private void LogInRout(User user)
        {
            if(user.Permission_Level == Level.Guest)
                Frame.Navigate(typeof(GuestMainPage), user);
            if (user.Permission_Level == Level.Employee)
                Frame.Navigate(typeof(EmployeeMainPage), user);
            if (user.Permission_Level == Level.Manager)
                Frame.Navigate(typeof(ManagerMainPage), user);
        }
    }
}
