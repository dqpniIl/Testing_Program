using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Логика взаимодействия для PageSpravochnikInformation.xaml
    /// </summary>
    public partial class PageSpravochnikInformation : Page
    {
        private ReferenceBookEntries zapis = new ReferenceBookEntries();
        public PageSpravochnikInformation(ReferenceBookEntries selectZapis)
        {
            InitializeComponent();
            if (selectZapis != null)
            {
                zapis = selectZapis;
            }
            DataContext = zapis;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.GoBack();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(zapis.topic_Entries))
                errors.AppendLine("Нет темы");
            if (string.IsNullOrWhiteSpace(zapis.description_Entries))
                errors.AppendLine("Нет пояснения");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (zapis.Id_Entries == 0)
                Entities.GetContext().ReferenceBookEntries.Add(zapis);
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены");
                GlobalUser.global_zapisSpravochnik = zapis;
                Navigation.MainFrame.Navigate(new PageSpravochnik());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
