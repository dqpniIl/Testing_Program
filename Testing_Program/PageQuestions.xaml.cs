using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageQuestions.xaml
    /// </summary>
    public partial class PageQuestions : Page
    {
        Entities entities = new Entities();
        private Questions question = new Questions();
        int indexsave;
        public PageQuestions()
        {
            InitializeComponent();
            dGridQuestions.ItemsSource = Entities.GetContext().Questions.ToList();
            DataContext = question;
            comboTest.Items.Add(GlobalUser.global_test);
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                dGridQuestions.ItemsSource = Entities.GetContext().Questions.ToList();
                comboTest.SelectedIndex = indexsave;
                UpdateDataGrid_in_comboBox();
                if (GlobalUser.global_question != null)
                {
                    foreach (var item in dGridQuestions.Items)
                    {
                        if (item == GlobalUser.global_question)
                        {
                            dGridQuestions.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageTests());
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            indexsave = comboTest.SelectedIndex;
            Navigation.MainFrame.Navigate(new PageAddQuestion(null));
        }
        private void BtnAnswers_Click(object sender, RoutedEventArgs e)
        {
            indexsave = comboTest.SelectedIndex;
            GlobalUser.global_question = dGridQuestions.SelectedItem as Questions;
            Navigation.MainFrame.Navigate(new PageAnswers());
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            indexsave = comboTest.SelectedIndex;
            GlobalUser.global_question = dGridQuestions.SelectedItem as Questions;
            Navigation.MainFrame.Navigate(new PageAddQuestion((sender as Button).DataContext as Questions));
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            indexsave = comboTest.SelectedIndex;
            if (MessageBox.Show($"Вы точно хотите удалить следующие {dGridQuestions.SelectedItems.Count} элементов(а)?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities context = Entities.GetContext();
                    using (var newContext = new Entities())
                    {
                        foreach (var zapis in dGridQuestions.SelectedItems.Cast<Questions>().ToList())
                        {
                            var remove_zapis = newContext.Questions.Find(zapis.Id_Question);
                            var exist = entities.Answers.Any(answers => answers.id_quest == remove_zapis.Id_Question);
                            if (exist)
                            {
                                MessageBox.Show("Вопрос удалить нельзя!\nСуществуют ответы привязанные к этому вопросу!\nУдалите их и повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                            else
                            {
                                if (remove_zapis != null)
                                    newContext.Questions.Remove(remove_zapis);
                            }
                        }
                        newContext.SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        dGridQuestions.ItemsSource = context.Questions.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            comboTest.SelectedIndex = indexsave;
            UpdateDataGrid_in_comboBox();
        }
        private void comboTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid_in_comboBox();
        }
        public void UpdateDataGrid_in_comboBox()
        {
            var questionsList = Entities.GetContext().Questions.ToList();
            if (comboTest.SelectedIndex > -1)
            {
                var selected_item = comboTest.SelectedItem as Tests;
                questionsList = questionsList.Where(a => a.id_test == selected_item.Id_Test).ToList();
            }
            dGridQuestions.ItemsSource = questionsList;
        }
    }
}
