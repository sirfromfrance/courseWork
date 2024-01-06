using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Data.SqlClient;

namespace corseWork
{
    public partial class student : Window
    {
        private const string ConnectionString = @"Server=LAPTOP-ACER; Database=distantMaterial; Integrated Security=True; Encrypt=False;";
       
        public student()
        {
            InitializeComponent();
            yearComboBox.SelectionChanged += ComboBox_Year;
            languageComboBox.SelectionChanged += ComboBox_Language;
            courseComboBox.SelectionChanged += ComboBox_Course;
            markComboBox.SelectionChanged += ComboBox_Mark;
        }


        private void searchMaterial_btn(object sender, RoutedEventArgs e)
        {
            string title = searchField.Text;
            string year = (string)yearComboBox.SelectedValue;
            string language = (string)languageComboBox.SelectedValue;
            string course = (string)courseComboBox.SelectedValue;
            string mark = (string)markComboBox.SelectedValue;

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM material WHERE title_material LIKE @Title AND year_material = ISNULL(NULLIF(@Year, ''), year_material) AND language_material = ISNULL(NULLIF(@Language, ''), language_material) AND course_material = ISNULL(NULLIF(@Course, ''), course_material)";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Title", "%" + title + "%");
                    command.Parameters.AddWithValue("@Year", year);
                    command.Parameters.AddWithValue("@Language", language);
                    command.Parameters.AddWithValue("@Course", course);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Далі можна використовувати dataTable для відображення результатів в DataGrid або іншій частині інтерфейсу.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void TextBox_SearchField(object sender, TextChangedEventArgs e)
        {

        }

        private void ComboBox_Year(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Language(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Course(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ComboBox_Mark(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
