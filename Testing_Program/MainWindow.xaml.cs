using System.Windows;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigation.MainFrame = MainFrame;
            if (GlobalUser.admin)
            {
                MainFrame.Navigate(new PageTests());
            }
            else
            {
                btnTest.Visibility = Visibility.Collapsed;
                MainFrame.Navigate(new PageUserTests());
            }
        }
        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageTests());
        }
        private void btnUserTests_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageUserTests());
        }
        private void btnRezults_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageRezults());
        }
        private void btnSpravoch_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageSpravochnik());
        }
        private void btnUchetnyaZapis_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new PageUchetnyaZapis());
        }
        private void btnLogOut_Click(object sender, RoutedEventArgs e)
        {
            var windowLogin = new WindowLogin();
            windowLogin.Show();
            Window currentWindow = Window.GetWindow(this);
            currentWindow.Close();
        }
    }
    public class GlobalUser
    {
        public static int globalIdUser;
        public static bool admin = false;
        public static Tests global_test;
        public static Questions global_question;
        public static Answers global_answer;
        public static Questions global_StatusQuestions;
        public static ReferenceBookEntries global_zapisSpravochnik;
    }
}
