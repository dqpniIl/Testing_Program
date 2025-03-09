using System;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageAddAnswer.xaml
    /// </summary>
    public partial class PageAddAnswer : Page
    {
        private Answers answer = new Answers();
        public PageAddAnswer(Answers selectAnswer)
        {
            InitializeComponent();
            if (selectAnswer != null)
            {
                answer = selectAnswer;
            }
                lblQuestion.Visibility = Visibility.Hidden;
                comboAddQuestion.Visibility = Visibility.Hidden;
            DataContext = answer;
            if (answer.status_answer == "Правильно")
            {
                checkBoxStatus.IsChecked = true;
            }
            else
                checkBoxStatus.IsChecked = false;
            comboAddQuestion.ItemsSource = Entities.GetContext().Questions.ToList();
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.GoBack();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (comboAddQuestion.Visibility == Visibility.Hidden)
                comboAddQuestion.SelectedItem = GlobalUser.global_question;
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(answer.answer))
                errors.AppendLine("Ответ не введен");
            if (comboAddQuestion.SelectedIndex == -1)
                errors.AppendLine("Нет привязки к тесту");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (checkBoxStatus.IsChecked == true)
            {
                answer.status_answer = "Правильно";
            }
            else
                answer.status_answer = "";
            if (answer.Id_Answer == 0)
                Entities.GetContext().Answers.Add(answer);
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены");
                GlobalUser.global_answer = answer;
                Navigation.MainFrame.Navigate(new PageAnswers());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
