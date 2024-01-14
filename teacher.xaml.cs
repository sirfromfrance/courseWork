using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static corseWork.student;

namespace corseWork
{
    /// <summary>
    /// Interaction logic for teacher.xaml
    /// </summary>
    public partial class teacher : Window
    {
       private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
        public teacher()
        {
            InitializeComponent();
            ComboBoxHelper.MaterialYears(yearComboBox);
            ComboBoxHelper.MaterialLanguages(languageComboBox);
            ComboBoxHelper.MaterialCourses(courseComboBox);
            ComboBoxHelper.MaterialMark(markComboBox);
        }

        private void TextBox_SearchTeacher(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_ToMaterials(object sender, RoutedEventArgs e)
        {
            string title = searchField.Text;
            string year = (string)yearComboBox.SelectedValue;
            string language = (string)languageComboBox.SelectedValue;
            string course = (string)courseComboBox.SelectedValue;
            string mark = (string)markComboBox.SelectedValue;
            string username = CurrentUser.username;
            TeacherMaterials TmaterialsWindow = new TeacherMaterials(title,"",year,language,course,mark, username);
            TmaterialsWindow.Show();
        }

        private void Button_Search(object sender, RoutedEventArgs e)
        {
            string title = searchField.Text;
            string year = (string)yearComboBox.SelectedValue;
            string language = (string)languageComboBox.SelectedValue;
            string course = (string)courseComboBox.SelectedValue;
            string mark = (string)markComboBox.SelectedValue;
            List<string> errors = new List<string>();

            if (string.IsNullOrEmpty(title))
            {
                errors.Add("Title is null or empty");
            }
            string error = default;
            foreach (string tmp in errors)
            {
                error += $"{tmp}\n";
            }

            if (!string.IsNullOrEmpty(error))
            {
                MessageBox.Show(error);
                return;
            }

            Materials materialsWindow = new Materials(title, "", year, language, course,  "");
            if (materialsWindow.materialGrid.ItemsSource != null)
            {
                materialsWindow.Show();
            }
        }

        private void Button_ResetFilters(object sender, RoutedEventArgs e)
        {
            searchField.Text = string.Empty;
            yearComboBox.SelectedIndex = -1;
            languageComboBox.SelectedIndex = -1;
            courseComboBox.SelectedIndex = -1;
            markComboBox.SelectedIndex = -1;
        }

        private void ComboBox_Language(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Mark(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Course(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Year(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
