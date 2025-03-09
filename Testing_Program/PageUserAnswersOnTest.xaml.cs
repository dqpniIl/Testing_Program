using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageUserAnswersOnTest.xaml
    /// </summary>
    public partial class PageUserAnswersOnTest : Page
    {
        Entities entities = new Entities();
        Questions[] massivQuestions;
        int currentQuestionIndex = 0; // Индекс текущего вопроса
        int tekballs = 0, allballs = 0, percent = 0, grade = 0;
        Answers selectAnswer = null;
        public PageUserAnswersOnTest()
        {
            InitializeComponent();
            LoadQuestionAndAnswers();
        }
        private void LoadQuestionAndAnswers()
        {
            int parentId = GlobalUser.global_test.Id_Test;
            var childQuestions = entities.Questions.Where(q => q.id_test == parentId).ToArray();
            massivQuestions = childQuestions;
            if (currentQuestionIndex == 0)
                allballs = massivQuestions.Length;
            txtTest.Text = $"{GlobalUser.global_test.name_Test}";
            txtQuestion.Text = $"{massivQuestions[currentQuestionIndex].question}";
            int parentIdAnswer = massivQuestions[currentQuestionIndex].Id_Question;
            var childAnswers = entities.Answers.Where(a => a.id_quest == parentIdAnswer).ToArray();
            foreach (var answer in childAnswers)
                listboxAnswers.Items.Add(answer);
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (selectAnswer == null)
            {
                MessageBox.Show("Выберите ответ!");
                return;
            }
            else
            {
                if (selectAnswer.status_answer == "Правильно")
                    tekballs += 1;
            }
            if (currentQuestionIndex < massivQuestions.Length - 1) // Выполняем следующие действия только если есть еще вопросы
            {
                listboxAnswers.Items.Clear(); // Очищаем текущие ответы в листбоксе
                currentQuestionIndex++; // Увеличиваем индекс текущего вопроса
                txtQuestion.Text = $"{massivQuestions[currentQuestionIndex].question}"; // Загружаем следующий вопрос и ответы
                int parentIdAnswer = massivQuestions[currentQuestionIndex].Id_Question;
                var childAnswers = entities.Answers.Where(a => a.id_quest == parentIdAnswer).ToArray();
                foreach (var answer in childAnswers)
                    listboxAnswers.Items.Add(answer);
            }
            else
            {
                percent = (tekballs * 100) / allballs;
                if (percent < 50)
                    grade = 2;
                if (percent >= 50)
                    grade = 3;
                if (percent >= 75)
                    grade = 4;
                if (percent >= 90)
                    grade = 5;
                Rezults rezult = new Rezults()
                {
                    percent = percent,
                    countRightQuestions = $"{tekballs}/{allballs}",
                    grade = grade,
                    date = DateTime.Now.ToString(),
                    id_user = GlobalUser.globalIdUser,
                    id_test = GlobalUser.global_test.Id_Test
                };
                entities.Rezults.Add(rezult);
                entities.SaveChanges();
                MessageBox.Show("Тест выполнен!");
                Navigation.MainFrame.Navigate(new PageRezults());
            }
        }
        private void listboxAnswers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected_answer = listboxAnswers.SelectedItem as Answers;
            if (selected_answer != null)
            {
                selectAnswer = selected_answer;
            }
        }
    }
}
