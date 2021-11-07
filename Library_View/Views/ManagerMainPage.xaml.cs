using Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Library_View
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ManagerMainPage : Page
    {
        User user;
        public ManagerMainPage()
        => this.InitializeComponent();

        //initiate the page
        protected override void OnNavigatedTo(NavigationEventArgs e)
        => user = e.Parameter as User;

        //Go to item search and purchase page
        private void Button_Click(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(SearchPage), user);

        // go to Invoices search page
        private void Button_Click_1(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(InvoicePage), user);

        // go to genre edit page
        private void Button_Click_2(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(GenrePage), user);

        // go back to the log in page
        private void LogOut_Click(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(LoginPage));

        // go to user / employee edit page
        private void Button_Click_3(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(EmployeePage),user);

        // go to customers edit page
        private void Button_Click_4(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(CustomerPage),user);

        // go to items edit page
        private void Button_Click_5(object sender, RoutedEventArgs e)
        => Frame.Navigate(typeof(ItemsPage), user);
    }
}
