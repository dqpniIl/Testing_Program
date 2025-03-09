using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageUchetnyaZapis.xaml
    /// </summary>
    public partial class PageUchetnyaZapis : Page
    {
        Entities entities = new Entities();
        public PageUchetnyaZapis()
        {
            InitializeComponent();
            tbSurname.PreviewTextInput += tb_Text;
            tbName.PreviewTextInput += tb_Text;
            var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
            if (User.surname != null)
                tbSurname.Text = User.surname;
            if (User.name != null)
                tbName.Text = User.name;
            if (User.groupe != null)
                tbGroupe.Text = User.groupe;
        }
        private void btnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var User = entities.Users.FirstOrDefault(x => x.Id_User == GlobalUser.globalIdUser);
            if (tbSurname.Text == "" && tbName.Text == "" ||tbGroupe.Text=="")
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                User.surname = tbSurname.Text;
                User.name = tbName.Text;
                User.groupe = tbGroupe.Text;
            }
            entities.SaveChanges();
            MessageBox.Show("Сохранено успешно!", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        private void tb_Text(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^а-яА-Я]");
            if (regex.IsMatch(e.Text))
            {
                e.Handled = true;
            }
        }
    }
}
