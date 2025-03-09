using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageTests.xaml
    /// </summary>
    public partial class PageTests : Page
    {
        Entities entities = new Entities();
        private Tests test = new Tests();
        public PageTests()
        {
            InitializeComponent();
            dGridTests.ItemsSource = Entities.GetContext().Tests.ToList();
            DataContext = test;
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                dGridTests.ItemsSource = Entities.GetContext().Tests.ToList();
                if (GlobalUser.global_test != null)
                {
                    foreach (var item in dGridTests.Items)
                    {
                        if (item == GlobalUser.global_test)
                        {
                            dGridTests.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageAddTest(null));
        }
        private void BtnQuestions_Click(object sender, RoutedEventArgs e)
        {
            GlobalUser.global_test = dGridTests.SelectedItem as Tests;
            Navigation.MainFrame.Navigate(new PageQuestions());
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageAddTest((sender as Button).DataContext as Tests));
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить следующие {dGridTests.SelectedItems.Count} элементов(а)?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities context = Entities.GetContext();
                    using (var newContext = new Entities())
                    {
                        foreach (var zapis in dGridTests.SelectedItems.Cast<Tests>().ToList())
                        {
                            var remove_zapis = newContext.Tests.Find(zapis.Id_Test);
                            var exist = entities.Questions.Any(question => question.id_test == remove_zapis.Id_Test);
                            if (exist)
                            {
                                MessageBox.Show("Тест удалить нельзя!\nСуществуют вопросы привязанные к этому тесту!\nУдалите их и повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                if (remove_zapis != null)
                                    newContext.Tests.Remove(remove_zapis);
                            }
                        }
                        newContext.SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        dGridTests.ItemsSource = context.Tests.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
        }
    }
}
