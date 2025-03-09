﻿using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageAddTest.xaml
    /// </summary>
    public partial class PageAddTest : Page
    {
        private Tests test = new Tests();
        public PageAddTest(Tests selectTest)
        {
            InitializeComponent();
            if (selectTest != null)
            {
                test = selectTest;
            }
            DataContext = test;
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            GlobalUser.global_test = test;
            Navigation.MainFrame.GoBack();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(test.name_Test))
                errors.AppendLine("Название теста не введено");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            if (test.Id_Test == 0)
                Entities.GetContext().Tests.Add(test);
            try
            {
                Entities.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены");
                GlobalUser.global_test = test;
                Navigation.MainFrame.Navigate(new PageTests());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
