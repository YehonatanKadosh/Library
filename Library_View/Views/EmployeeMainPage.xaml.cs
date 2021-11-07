using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Library_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EmployeeMainPage : Page
    {
        User user;
        public EmployeeMainPage()
        => this.InitializeComponent();

        // initiate the page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        => user = e.Parameter as User;

        // navigate to the search page
        private void Button_Click(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(SearchPage), user);

        //navigate to the invoice search page
        private void Button_Click_1(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(InvoicePage), user);

        // navigate to the genre edit page
        private void Button_Click_2(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(GenrePage), user);

        // navigate back to the log in page
        private void LogOut_Click(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(LoginPage));

        // navigate to customer edit page
        private void Button_Click_4(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(CustomerPage), user);
        
        // navigate to the item edit page
        private void Button_Click_5(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(ItemsPage), user);
    }
}
