using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
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
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace Testing_Program
{
    /// <summary>
    /// Логика взаимодействия для PageRezults.xaml
    /// </summary>
    public partial class PageRezults : Page
    {
        Entities entities=new Entities();
        public PageRezults()
        {
            InitializeComponent();
            if (GlobalUser.admin)
            {
                lblForUser.Visibility = Visibility.Collapsed;
                dGridRezults.ItemsSource= Entities.GetContext().Rezults.ToList();
                comboSearch.ItemsSource=Entities.GetContext().Tests.ToList();
            }
            else
            {
                btnClearSearch.Visibility = Visibility.Collapsed;
                lblSearch.Visibility = Visibility.Collapsed;
                tbSearch.Visibility = Visibility.Collapsed;
                comboSearch.Visibility = Visibility.Collapsed;
                btnLoadData.Visibility = Visibility.Collapsed;
                foreach (var zapis in entities.Rezults)
                    if (zapis.id_user == GlobalUser.globalIdUser)
                        dGridRezults.Items.Add(zapis);
            }
        }
        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            tbSearch.Clear();
            comboSearch.SelectedIndex = -1;
        }
        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = tbSearch.Text.ToLower();
            var collectionView = CollectionViewSource.GetDefaultView(dGridRezults.ItemsSource);
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
                        Rezults zapis_Rezult = item as Rezults;

                        if (zapis_Rezult != null)
                        {
                            return zapis_Rezult.Users.groupe.ToLower().Contains(searchText)
                            || zapis_Rezult.Users.surname.ToLower().Contains(searchText)
                            || zapis_Rezult.Users.name.ToString().ToLower().Contains(searchText)
                            || zapis_Rezult.Tests.name_Test.ToString().ToLower().Contains(searchText)
                            || zapis_Rezult.grade.ToString().ToLower().Contains(searchText)
                            || zapis_Rezult.countRightQuestions.ToString().ToLower().Contains(searchText)
                            || zapis_Rezult.percent.ToString().ToLower().Contains(searchText)
                            || zapis_Rezult.date.ToString().ToLower().Contains(searchText);
                        }
                        return false;
                    };
                }
            }
        }
        private void comboSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid_in_comboBox();
        }
        public void UpdateDataGrid_in_comboBox()
        {
            var questionsList = Entities.GetContext().Rezults.ToList();
            if (comboSearch.SelectedIndex > -1)
            {
                var selected_item = comboSearch.SelectedItem as Tests;
                questionsList = questionsList.Where(a => a.id_test == selected_item.Id_Test).ToList();
            }
            dGridRezults.ItemsSource = questionsList;
        }
        private void btnLoadData_Click(object sender, RoutedEventArgs e)
        {
            string reportFolderPath = "C:\\Report\\";
            Directory.CreateDirectory(reportFolderPath);
            string fileName = $"RezultsReport_{DateTime.Now:yyyyMMddHHmmss}.docx";
            string filePath = System.IO.Path.Combine(reportFolderPath, fileName);
            using (DocX document = DocX.Create(filePath))
            {
                document.InsertParagraph("Отчет о результатах тестов").FontSize(14).Font(new Font("TimesNewRoman")).Bold().Alignment = Alignment.center;
                Xceed.Document.NET.Table table = document.AddTable(1, 9);
                table.Alignment = Alignment.center;
                table.Rows[0].Cells[0].Paragraphs.First().Append("ID результата");
                table.Rows[0].Cells[1].Paragraphs.First().Append("Группа");
                table.Rows[0].Cells[2].Paragraphs.First().Append("Фамилия");
                table.Rows[0].Cells[3].Paragraphs.First().Append("Имя");
                table.Rows[0].Cells[4].Paragraphs.First().Append("Тест");
                table.Rows[0].Cells[5].Paragraphs.First().Append("Оценка");
                table.Rows[0].Cells[6].Paragraphs.First().Append("Количество правильных ответов");
                table.Rows[0].Cells[7].Paragraphs.First().Append("Процент");
                table.Rows[0].Cells[8].Paragraphs.First().Append("Дата");
                using (Entities entities = new Entities())
                {
                    var rezults = entities.Rezults.ToList();
                    foreach (var rezult in rezults)
                    {
                        Row row = table.InsertRow();

                        row.Cells[0].Paragraphs[0].Append(rezult.Id_Rezult.ToString());
                        row.Cells[1].Paragraphs[0].Append(rezult.Users.groupe.ToString());
                        row.Cells[2].Paragraphs[0].Append(rezult.Users.surname.ToString());
                        row.Cells[3].Paragraphs[0].Append(rezult.Users.name);
                        row.Cells[4].Paragraphs[0].Append(rezult.Tests.name_Test);
                        row.Cells[5].Paragraphs[0].Append(rezult.grade.ToString());
                        row.Cells[6].Paragraphs[0].Append(rezult.countRightQuestions);
                        row.Cells[7].Paragraphs[0].Append(rezult.percent.ToString());
                        row.Cells[8].Paragraphs[0].Append(rezult.date);
                    }
                    document.InsertTable(table);
                }
                try
                {
                    document.Save();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при сохранении файла: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                System.Diagnostics.Process.Start("explorer.exe", "/select," + filePath);
            }
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            string excelFilePath = "C:\\Report\\RezultsReport.xlsx";
            FileInfo excelFile = new FileInfo(excelFilePath);
            using (ExcelPackage package = new ExcelPackage(excelFile))
            {
                string sheetName = "Отчёт_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
                worksheet.Cells["A1"].Value = "Отчет о результатах тестов";
                worksheet.Cells["A1"].Style.Font.Size = 14;
                worksheet.Cells["A1"].Style.Font.Name = "Times New Roman";
                worksheet.Cells["A1"].Style.Font.Bold = true;
                worksheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells["A3"].Value = "ID результата";
                worksheet.Cells["B3"].Value = "Группа";
                worksheet.Cells["C3"].Value = "Фамилия";
                worksheet.Cells["D3"].Value = "Имя";
                worksheet.Cells["E3"].Value = "Тест";
                worksheet.Cells["F3"].Value = "Оценка";
                worksheet.Cells["G3"].Value = "Количество правильных ответов";
                worksheet.Cells["H3"].Value = "Процент";
                worksheet.Cells["I3"].Value = "Дата";
                int row = 4;
                using (Entities entities = new Entities())
                {
                    var rezults = entities.Rezults.ToList();
                    foreach (var rezult in rezults)
                    {
                        worksheet.Cells["A" + row].Value = rezult.Id_Rezult;
                        worksheet.Cells["B" + row].Value = rezult.Users.groupe;
                        worksheet.Cells["C" + row].Value = rezult.Users.surname;
                        worksheet.Cells["D" + row].Value = rezult.Users.name;
                        worksheet.Cells["E" + row].Value = rezult.Tests.name_Test;
                        worksheet.Cells["F" + row].Value = rezult.grade;
                        worksheet.Cells["G" + row].Value = rezult.countRightQuestions;
                        worksheet.Cells["H" + row].Value = rezult.percent;
                        worksheet.Cells["I" + row].Value = rezult.date;
                        row++;
                    }
                    row++;
                    worksheet.Cells["A" + row].Value = "Общее количество результатов";
                    worksheet.Cells["A" + row].Style.Font.Size = 12;
                    worksheet.Cells["A" + row].Style.Font.Bold = true;
                    worksheet.Cells["A" + row].Style.Font.Name = "Times New Roman";
                    worksheet.Cells["A" + row].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    row += 2;
                    worksheet.Cells["A" + row].Value = "Процент успеваемости";
                    worksheet.Cells["B" + row].Value = "Средняя оценка";
                    row++;
                    double totalPercent = rezults.Average(r => r.percent);
                    double averageGrade = rezults.Average(r => r.grade);
                    worksheet.Cells["A" + row].Value = totalPercent;
                    worksheet.Cells["B" + row].Value = averageGrade;
                    package.Save();
                }
            }
            MessageBox.Show("Файлы успешно сохранены!", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
