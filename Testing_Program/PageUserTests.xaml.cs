using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageUserTests.xaml
    /// </summary>
    public partial class PageUserTests : Page
    {
        public PageUserTests()
        {
            InitializeComponent();
            dGridUserTests.ItemsSource = Entities.GetContext().Tests.ToList();
        }
        private void BtnBegin_Click(object sender, RoutedEventArgs e)
        {
            GlobalUser.global_test = dGridUserTests.SelectedItem as Tests;
            Navigation.MainFrame.Navigate(new PageUserAnswersOnTest());
        }
    }
}
