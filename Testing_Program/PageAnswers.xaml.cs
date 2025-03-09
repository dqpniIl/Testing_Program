using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageAnswers.xaml
    /// </summary>
    public partial class PageAnswers : Page
    {
        private Answers answer = new Answers();
        int indexsave;
        public PageAnswers()
        {
            InitializeComponent();
            dGridAnswers.ItemsSource = Entities.GetContext().Answers.ToList();
            DataContext = answer;
            comboQuestion.Items.Add(GlobalUser.global_question);
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                dGridAnswers.ItemsSource = Entities.GetContext().Answers.ToList();
                comboQuestion.SelectedIndex = indexsave;
                UpdateDataGrid_in_comboBox();
                if (GlobalUser.global_answer != null)
                {
                    foreach (var item in dGridAnswers.Items)
                    {
                        if (item == GlobalUser.global_answer)
                        {
                            dGridAnswers.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageQuestions());
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            indexsave = comboQuestion.SelectedIndex;
            Navigation.MainFrame.Navigate(new PageAddAnswer(null));
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            indexsave = comboQuestion.SelectedIndex;
            GlobalUser.global_answer = dGridAnswers.SelectedItem as Answers;
            Navigation.MainFrame.Navigate(new PageAddAnswer((sender as Button).DataContext as Answers));
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            indexsave = comboQuestion.SelectedIndex;
            if (MessageBox.Show($"Вы точно хотите удалить следующие {dGridAnswers.SelectedItems.Count} элементов(а)?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities context = Entities.GetContext();
                    using (var newContext = new Entities())
                    {
                        foreach (var zapis in dGridAnswers.SelectedItems.Cast<Answers>().ToList())
                        {
                            var remove_zapis = newContext.Answers.Find(zapis.Id_Answer);
                            if (remove_zapis != null)
                                newContext.Answers.Remove(remove_zapis);
                        }
                        newContext.SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        dGridAnswers.ItemsSource = context.Answers.ToList();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            comboQuestion.SelectedIndex = indexsave;
            UpdateDataGrid_in_comboBox();
        }
        private void comboQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid_in_comboBox();
        }
        public void UpdateDataGrid_in_comboBox()
        {
            var answersList = Entities.GetContext().Answers.ToList();
            if (comboQuestion.SelectedIndex > -1)
            {
                var selected_item = comboQuestion.SelectedItem as Questions;
                answersList = answersList.Where(a => a.id_quest == selected_item.Id_Question).ToList();
            }
            dGridAnswers.ItemsSource = answersList;
        }
    }
}
