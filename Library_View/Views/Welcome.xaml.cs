using Common;
using Logic_BLL;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Library_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Welcome : Page
    {
        public Welcome()
        {
            this.InitializeComponent();
            Service_Manager.Instance.ToString();
        }

        /// <summary>
        /// Starter Pack For the Progran to Run
        /// </summary>
        private async void Init_Click(object sender, RoutedEventArgs e)
        {
            if ((await Service_Manager.Instance.User_Service_Instance.Get_AllAsync()).Find(user => user.User_Name == "Guest") == null)
                await Service_Manager.Instance.User_Service_Instance.Generate_UserAsync("Guest", "", Level.Guest);
            if ((await Service_Manager.Instance.User_Service_Instance.Get_AllAsync()).Find(user => user.User_Name == "Yehonatan") == null)
                await Service_Manager.Instance.User_Service_Instance.Generate_UserAsync("Yehonatan", "1234", Level.Manager);

            if ((await Service_Manager.Instance.Customer_Service_Instance.Get_AllAsync()).Find(customer => customer.First_name == "Not A Customer") == null)
                await Service_Manager.Instance.Customer_Service_Instance.Generate_CustomerAsync("Not A Customer", "", 0, DateTime.Now, 0);

            if ((await Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Partial_Name("First Book")).Count == 0)
                await Service_Manager.Instance.Item_Service_Instance.Generate_New_BookAsync("First Book", new List<Genre>(), DateTime.MinValue, 7, 40.70, 0, "A author", "This is the first summary");
            if ((await Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Partial_Name("Second Book")).Count == 0)
                await Service_Manager.Instance.Item_Service_Instance.Generate_New_BookAsync("Second Book", new List<Genre>(), DateTime.Now, 10, 2020.11, 0, "B author", "This is the second summary");
            if ((await Service_Manager.Instance.Item_Service_Instance.Get_Items_By_Partial_Name("First journal")).Count == 0)
                await Service_Manager.Instance.Item_Service_Instance.Generate_New_JournalAsync("First journal", new List<Genre>(), DateTime.MaxValue, 12, 13.20, 0, "journal author", 122);

            if ((await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_FictionAsync("Horror")).Count == 0)
                await Service_Manager.Instance.Genre_Service_Instance.Create_New_GenreAsync("Horror", GenreType.Fiction);
            if ((await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_FictionAsync("Comedy")).Count == 0)
                await Service_Manager.Instance.Genre_Service_Instance.Create_New_GenreAsync("Comedy", GenreType.Fiction);
            if ((await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_FictionAsync("Romance")).Count == 0)
                await Service_Manager.Instance.Genre_Service_Instance.Create_New_GenreAsync("Romance", GenreType.Fiction);
            if ((await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_NonfictionAsync("Inter-galactic")).Count == 0)
                await Service_Manager.Instance.Genre_Service_Instance.Create_New_GenreAsync("Inter-galactic", GenreType.Nonfiction);
            if ((await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_NonfictionAsync("Sience-Fiction")).Count == 0)
                await Service_Manager.Instance.Genre_Service_Instance.Create_New_GenreAsync("Sience-Fiction", GenreType.Nonfiction);
            if ((await Service_Manager.Instance.Genre_Service_Instance.Genre_Search_NonfictionAsync("Space")).Count == 0)
                await Service_Manager.Instance.Genre_Service_Instance.Create_New_GenreAsync("Space", GenreType.Nonfiction);
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
