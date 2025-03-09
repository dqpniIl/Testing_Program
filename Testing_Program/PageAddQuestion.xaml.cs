using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageAddQuestion.xaml
    /// </summary>
    public partial class PageAddQuestion : Page
    {
        private Questions question = new Questions();
        public PageAddQuestion(Questions selectQuestion)
        {
            InitializeComponent();
            if (selectQuestion != null)
            {
                question = selectQuestion;
            }
            lblTest.Visibility = Visibility.Hidden;
            comboAddTest.Visibility = Visibility.Hidden;
            DataContext = question;
            comboAddTest.ItemsSource = Entities.GetContext().Tests.ToList();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.GoBack();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboAddTest.Visibility == Visibility.Hidden)
                comboAddTest.SelectedItem = GlobalUser.global_test;

            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(question.question))
                errors.AppendLine("Вопрос не введен");
            if (comboAddTest.SelectedIndex == -1)
                errors.AppendLine("Нет привязки к тесту");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (question.Id_Question == 0)
                Entities.GetContext().Questions.Add(question);
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены");
                GlobalUser.global_question = question;
                Navigation.MainFrame.Navigate(new PageQuestions());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
