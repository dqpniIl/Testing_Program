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
    /// Логика взаимодействия для PageSpravochnik.xaml
    /// </summary>
    public partial class PageSpravochnik : Page
    {
        private ReferenceBookEntries zapis = new ReferenceBookEntries();
        public PageSpravochnik()
        {
            InitializeComponent();
            dGridSpravoch.ItemsSource = Entities.GetContext().ReferenceBookEntries.ToList();
            DataContext = zapis;
        }
        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                dGridSpravoch.ItemsSource = Entities.GetContext().ReferenceBookEntries.ToList();
                if (GlobalUser.global_zapisSpravochnik != null)
                {
                    foreach (var item in dGridSpravoch.Items)
                    {
                        if (item == GlobalUser.global_zapisSpravochnik)
                        {
                            dGridSpravoch.SelectedItem = item;
                            break;
                        }
                    }
                }
            }
        }
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbSearch.Text.ToLower();
            var collectionView = CollectionViewSource.GetDefaultView(dGridSpravoch.ItemsSource);
            if (collectionView != null)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    collectionView.Filter = null;
                }
                else
                {
                    collectionView.Filter = item =>
                    {
                        ReferenceBookEntries zapis = item as ReferenceBookEntries;

                        if (zapis != null)
                        {
                            return zapis.topic_Entries.ToLower().Contains(searchText);
                        }
                        return false;
                    };
                }
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageQuestions());
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Navigation.MainFrame.Navigate(new PageSpravochnikInformation(null));
        }
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            GlobalUser.global_zapisSpravochnik = dGridSpravoch.SelectedItem as ReferenceBookEntries;
            Navigation.MainFrame.Navigate(new PageSpravochnikInformation((sender as Button).DataContext as ReferenceBookEntries));
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы точно хотите удалить следующие {dGridSpravoch.SelectedItems.Count} элементов(а)?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    Entities context = Entities.GetContext();
                    using (var newContext = new Entities())
                    {
                        foreach (var zapis in dGridSpravoch.SelectedItems.Cast<ReferenceBookEntries>().ToList())
                        {
                            var remove_zapis = newContext.ReferenceBookEntries.Find(zapis.Id_Entries);
                            if (remove_zapis != null)
                                newContext.ReferenceBookEntries.Remove(remove_zapis);
                        }
                        newContext.SaveChanges();
                        MessageBox.Show("Данные удалены!");
                        dGridSpravoch.ItemsSource = context.ReferenceBookEntries.ToList();
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
