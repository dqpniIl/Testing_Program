using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageLog.xaml
    /// </summary>
    public partial class PageLog : Page
    {
        Entities entities = new Entities();
        Users userForId;
        public PageLog()
        {
            InitializeComponent();
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tblogin.Text) || string.IsNullOrEmpty(tbpassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            bool proverka = false;
            foreach (var user in entities.Users)
            {
                if (tblogin.Text == user.login || tblogin.Text == user.Id_User.ToString())
                {
                    if (tbpassword.Text == user.password)
                    {
                        if (user.role == "Admin")
                            GlobalUser.admin = true;
                        else
                            GlobalUser.admin = false;
                        var mainwin = new MainWindow();
                        mainwin.Show();
                        Window currentWindow = Window.GetWindow(this);
                        currentWindow.Close();
                        proverka = true;
                        userForId = user;
                    }
                }
            }
            if (!proverka)
            {
                MessageBox.Show("Неверный Логин или Пароль. Пожалуйста проверьте правильность введённых данных и повторите попытку.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                GlobalUser.globalIdUser = userForId.Id_User;
            }
        }
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageReg());
        }
    }
}
