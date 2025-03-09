using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageReg.xaml
    /// </summary>
    public partial class PageReg : Page
    {
        Entities entities = new Entities();
        public PageReg()
        {
            InitializeComponent();
            tblogin.PreviewTextInput += tblogin_TextInput;
        }
        private void tblogin_TextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^a-zA-Z0-9@._-]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageLog());
        }
        private void btnReg_Click(object sender, RoutedEventArgs e)
        {
            string Login = tblogin.Text;
            if (string.IsNullOrEmpty(tblogin.Text) || string.IsNullOrEmpty(tbpassword.Text) || string.IsNullOrEmpty(tbPovtorpassword.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (tbpassword.Text != tbPovtorpassword.Text)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, проверьте правильность данных и повторите попытку.", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var existingUser = entities.Users.FirstOrDefault(u => u.login == Login);
            if (existingUser != null)
            {
                MessageBox.Show("Пользователь с таким логином уже существует.", "Регистрация", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                var newUser = new Users
                {
                    login = Login,
                    password = tbpassword.Text,
                    role = "User"
                };
                entities.Users.Add(newUser);
                try
                {
                    entities.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                        }
                    }
                }
                var mainWindow = new MainWindow();
                mainWindow.Show();
                Window currentWindow = Window.GetWindow(this);
                currentWindow.Close();
            }
            string LLOogin = tblogin.Text;
            var RegistrationUser = entities.Users.FirstOrDefault(u => u.login == LLOogin);
            GlobalUser.globalIdUser = RegistrationUser.Id_User;
            entities.SaveChanges();
        }
    }
}
